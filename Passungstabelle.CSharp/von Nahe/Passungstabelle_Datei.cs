namespace Passungstabelle.CSharp;

using System.Collections.Generic;

using SolidWorks.Interop.sldworks;


public class Passungstabelle_Datei
{
    public Passungstabelle_Blatt[] Blätter { get; set; }
    public string[] BlätterStr { get; set; }

    public GeneralSettings Attr_generell { get; set; } = new();
    public Dictionary<string, Dictionary<string, string>> Attr_Übersetzungen { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    public Dictionary<string, FormatSettings> Attr_Formate { get; set; } = new();
    public Dictionary<string, TableSettings> Attr_Tabelle { get; set; } = new();

    public LogFile Log { get; set; }

    public SldWorks Swapp { get; set; }
    public DrawingDoc SwDraw { get; set; }

    private Sheet Swcsheet { get; set; }

    public Passungstabelle_Datei(SldWorks iSwapp)
    {
        this.Swapp = iSwapp;
    }

    // Erstellt eine Liste der Blätter in der Datei
    public bool PassungsTabelleGetSheets(ModelDoc2 swmodel)
    {
        bool PassungsTabelleGetSheetsRet = default;
        bool ok;

        ok = false;
        this.SwDraw = (DrawingDoc)swmodel;

        this.Log.WriteInfo(swmodel.GetPathName(), "", false);

        // * Namen der Blätter speichern
        this.BlätterStr = (string[])this.SwDraw.GetSheetNames();
        if (this.BlätterStr.Length == 0)
        {
            this.Log.WriteInfo(My.Resources._Keine_Blätter_in_der_Zeichnung, "", true);
            PassungsTabelleGetSheetsRet = ok;
            return PassungsTabelleGetSheetsRet;
        }

        // * Aktuelles Blatt speichern um diese später wieder zu aktivieren
        this.Swcsheet = (Sheet)this.SwDraw.GetCurrentSheet();

        // Blätter suchen und initialisieren
        this.SetBlätter(swmodel);

        this.SwDraw.ActivateSheet(this.Swcsheet.GetName());
        PassungsTabelleGetSheetsRet = true;
        return PassungsTabelleGetSheetsRet;
    }

    public void SetBlätter(ModelDoc2 swmodel)
    {
        var i = default(int);
        int z;

        // Wenn nur die Tabelle nur auf dem ersten Blatt erscheinen soll und 
        // auf den restlichen Blättern nicht gelöscht werden soll
        // brauchen wir nur das erste Blatt untersuchen, sonst müssen auch die 
        // restlichen Blätter durchsucht werden ob eine alte Tabelle vorhanden ist
        if (this.Attr_generell.NurAufErstemBlatt && !this.Attr_generell.LöschenAufRestlichenBlättern)
        {
            z = 0;
            this.Blätter = new Passungstabelle_Blatt[i + 1];
        }
        else
        {
            z = this.BlätterStr.Length;
            this.Blätter = new Passungstabelle_Blatt[z + 1];
        }

        var loopTo = z;
        for (i = 0; i <= loopTo; i++)
        {
            this.Blätter[i] = new Passungstabelle_Blatt(this.Swapp, this.Attr_generell, this.Attr_Übersetzungen, this.SwDraw.get_Sheet(this.BlätterStr[i]), (DrawingDoc)swmodel, this.Log);
            this.Blätter[i].SetSheetAttr(this.Attr_Formate, this.Attr_Tabelle);
            // * Ermittlung der Ansichten auf dem Blatt
            this.Blätter[i].PassungsTabelleGetViews(swmodel, this.BlätterStr[i]);
            // Blätter(i).Log = Log
        }

        // suchen nach Passungen auf den Blättern
        this.PassungstabelleGetdimensionFromSheets();
    }

    // Löscht die Tabellen auf allen Blättern außer dem ersten
    public void DelTableOnOtherSheets()
    {
        Sheet sh;
        sh = (Sheet)this.SwDraw.GetCurrentSheet();

        for (int i = 1, loopTo = this.BlätterStr.Length - 1; i <= loopTo; i++)
        {
            // sh = swDraw.Sheet(BlätterStr(i))
            this.SwDraw.ActivateSheet(this.Blätter[i].Blatt.GetName());
            this.Blätter[i].DeleteTab();
        }

        this.SwDraw.ActivateSheet(sh.GetName());
    }

    // Löscht die Tabellen auf allen Blättern 
    public void DelAllTables()
    {
        Sheet sh;
        sh = (Sheet)this.SwDraw.GetCurrentSheet();

        for (int i = 0, loopTo = this.BlätterStr.Length - 1; i <= loopTo; i++)
        {
            // sh = SwDraw.Sheet(BlätterStr(i))
            this.SwDraw.ActivateSheet(this.Blätter[i].Blatt.GetName());
            this.Blätter[i].DeleteTab();
        }
        this.SwDraw.ActivateSheet(sh.GetName());
    }

    public bool PassungstabelleGetdimensionFromSheets()
    {
        bool PassungstabelleGetdimensionFromSheetsRet = default;
        int i = 0;
        int z = 0;

        // Wenn Tabelle nur auf dem ersten Blatt erscheinen soll,
        // dann wird auch nur auf dem ersten Blatt nach Passungen gesucht
        if (this.Attr_generell.NurAufErstemBlatt)
            z = 0;
        else
            z = this.Blätter.Length - 1;

        var loopTo = z;
        for (i = 0; i <= loopTo; i++)
        {
            this.Blätter[i].SetEinfügepunkt();
            this.Blätter[i].PassungsTabelleGetDimensions();
            this.Blätter[i].SetEinfügePunktPosition();
        }

        PassungstabelleGetdimensionFromSheetsRet = true;
        return PassungstabelleGetdimensionFromSheetsRet;
    }

    public void InsertTableOnSheets()
    {
        bool PassungenGefunden = false;
        Sheet swsheet;

        swsheet = (Sheet)this.SwDraw.GetCurrentSheet();

        // Prüfung ob auf zumindest einem Blatt, Passungen gefunden wurden
        foreach (var blatt in this.Blätter)
        {
            if (blatt.Tabelle is not null)
            {
                if (blatt.Tabelle.TabellenZeilen.Count > 0)
                {
                    PassungenGefunden = true;
                }
                // Check ob auf einem der Blätter ev. eine Tabelle vorhanden ist
                // Wenn ja, dann wird die Tabelle gelöscht
                else if (blatt.AlteTabelle is not null)
                {
                    this.SwDraw.ActivateSheet(blatt.Blatt.GetName());
                    blatt.DeleteTab();
                }
            }
        }

        this.SwDraw.ActivateSheet(swsheet.GetName());

        // Keine Passungen gefunden
        if (!PassungenGefunden)
        {
            // Wenn Fehlermeldung ausgegeben werden soll
            this.Log.WriteInfo(My.Resources._Keine_Passungen_gefunden, "", true);
            return;
        }


        // Wenn die Tabelle auf den restlichen Blättern gelöscht werden soll
        if (this.Attr_generell.NurAufErstemBlatt && this.Attr_generell.LöschenAufRestlichenBlättern)
        {
            this.DelTableOnOtherSheets();
            this.Blätter[0].InsertTable();
        }
        if (!this.Attr_generell.NurAufErstemBlatt && !this.Attr_generell.LöschenAufRestlichenBlättern)
        {
            foreach (var n in this.Blätter)
            {
                n.InsertTable();
            }
        }
        if (!this.Attr_generell.NurAufErstemBlatt && this.Attr_generell.LöschenAufRestlichenBlättern)
        {
            this.DelAllTables();
        }
        if (this.Attr_generell.NurAufErstemBlatt && !this.Attr_generell.LöschenAufRestlichenBlättern)
        {
            this.Blätter[0].InsertTable();
        }

        this.SwDraw.ActivateSheet(this.Swcsheet.GetName());
    }

}