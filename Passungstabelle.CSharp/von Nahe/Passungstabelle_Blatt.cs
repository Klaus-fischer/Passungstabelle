using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Passungstabelle.CSharp;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using My = Passungstabelle.CSharp.My;

namespace Passungstabelle.CSharp;


public partial class Passungstabelle_Blatt
{
    public Sheet Blatt { get; set; }                         // Verweis auf das Blatt
    public ModelDoc2 BlattMod { get; set; }                  // Verweis auf die Datei von der die erste Ansicht abgeleitet ist
    public List<Ansicht> Ansichten { get; set; } = new List<Ansicht>();      // Liste der Ansichten auf dem Blatt
    public int AnzahlAnsichten { get; set; }             // Anzahl der Ansichten
    public Passungstabelle_Tabelle Tabelle { get; set; }     // Verweis auf die Tabelle

    // Attribute
    public GeneralSettings Attr_generell { get; set; }
    public Dictionary<string, Dictionary<string, string>> Attr_Übersetzungen { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    public FormatSettings Attr_Format { get; set; } = new();
    public TableSettings Attr_Tabelle { get; set; } = new();
    public Definitionen.BlattEigenschaften Attr_Sheet { get; set; }

    public ITableAnnotation? AlteTabelle { get; set; }         // Verweis auf eine eventuell vorhandene alte PassungstabellenHandler
    public double AlteTabelleX { get; set; }                 // X-Positon der alten Tabelle
    public double AlteTabelleY { get; set; }                 // Y-Positon der alten Tabelle

    public LogFile Log { get; set; }                         // Verweis auf das Log-Datei Element
    public SldWorks Swapp { get; set; }                      // Verweis auf SolidWorks

    private DrawingDoc swdraw;                        // Verweis auf die Zeichnungsdatei
    private double[] Einfügeposition = new double[2];                   // Einfügeposition für die PassungstabellenHandler

    /// <summary>
    /// Intialisierungsfuntkion für die Klasse "Passungstabelle_Blatt"
    /// </summary>
    public Passungstabelle_Blatt(SldWorks iswapp, GeneralSettings iAttr_generell, Dictionary<string, Dictionary<string, string>> iAttr_Übersetzungen, Sheet iBlatt, DrawingDoc model, LogFile iLog)
    {
        this.Swapp = iswapp;
        this.Attr_generell = iAttr_generell;
        this.Attr_Übersetzungen = iAttr_Übersetzungen;
        this.swdraw = model;
        this.Blatt = iBlatt;
        // Log = New LogFile(Attr_generell)
        this.Log = iLog;
    }

    /// <summary>
    /// Erstellt eine Liste der Ansicht in dem angegebenen Blatt
    /// es werden nur Ansichten berücksichtigt, die auch vom konfigurierten Modell-Type abgeleitet sind
    /// sollen z.B.: Teile nicht berücksichtigt werden, dann werden Ansicht die von Teilen abgeleitet sind
    /// nicht in die Liste aufgenommen
    /// </summary>
    /// <param name="swmodel">Verweis SolidWorks Datei</param>
    /// <param name="blattname">Blattname</param>
    /// <returns></returns>
    public bool PassungsTabelleGetViews(ModelDoc2 swmodel, string blattname)
    {
        bool PassungsTabelleGetViewsRet = default;
        View swView;
        Ansicht AnsichtRec;
        int DocType = 0;

        this.BlattMod = swmodel;
        // * Verweis auf Blatt
        this.swdraw.ActivateSheet(blattname);
        this.Blatt = this.swdraw.get_Sheet(blattname);

        this.swdraw.GetViewCount();

        // * Erste Ansicht auf dem Blatt
        swView = (View)this.swdraw.GetFirstView();
        this.AnzahlAnsichten = 0;


        // Dokumenttyp der auszuwertenden Ansichten holen (Teil, Baugruppe, ...)
        DocType = this.GetDokumentTypeFromSetup();

        // * So lange Ansichten gefunden werden
        while (swView is not null)
        {
            // Nur wenn der Dokumenttyp passt dann muss die Ansicht ausgewertet werden
            if ((this.GetDocumentTypeFromRef(swView) & DocType) > 0)
            {
                // Neuen Ansichtsrekord
                AnsichtRec = new Ansicht(swView.Name, swView.ReferencedDocument, -1, swView);
                // Wenn eine Referenz extiert
                if (AnsichtRec.arefernz is not null)
                {
                    AnsichtRec.doctype = AnsichtRec.arefernz.GetType();
                }
                else
                {
                    AnsichtRec.doctype = 0;
                }
                this.AnzahlAnsichten = this.AnzahlAnsichten + 1;
                // Auf Bohrungstabelle prüfen
                AnsichtRec.holetab = this.CheckForHoleTable(swView);
                // Ansicht zur Liste hinzufügen
                this.Ansichten.Add(AnsichtRec);
                // Prüfen ob eine alte PassungstabellenHandler an der Ansicht hänt
                this.CheckForOldTable(swView);
            }
            // Nächste Ansicht holen
            swView = (View)swView.GetNextView();
        }

        PassungsTabelleGetViewsRet = true;
        return PassungsTabelleGetViewsRet;
    }

    public List<Dictionary<string, List<MathPoint>>> GetHoleTabletags(List<HoleTable> HoleTab)
    {
        List<Dictionary<string, List<MathPoint>>> GetHoleTabletagsRet = default;
        var zoneList = new List<MathPoint>();
        var result = new List<Dictionary<string, List<MathPoint>>>();

        for (int j = 0; j <= HoleTab.Count - 1; j++)
        {
            var tabs = HoleTab[j].GetTableAnnotations().AsArrayOfType<TableAnnotation>();
            var temp = new Dictionary<string, List<MathPoint>>();

            for (int i = 1; i <= tabs[0].RowCount - 1; i++)
            {
                temp[HoleTab[j].HoleTag[i]] = zoneList;
            }

            result.Add(temp);
        }

        return result;
    }

    public List<Dictionary<string, List<string>>> GetHoleTabletags1(List<HoleTable> HoleTab)
    {
        TableAnnotation[] tabs;
        var ZoneList = new List<string>();
        var result = new List<Dictionary<string, List<string>>>();

        for (int j = 0; j <= HoleTab.Count - 1; j++)
        {
            tabs = HoleTab[j].GetTableAnnotations().AsArrayOfType<TableAnnotation>();
            var temp = new Dictionary<string, List<string>>();

            for (int i = 1; i <= tabs[0].RowCount - 1; i++)
            {
                temp[HoleTab[j].HoleTag[i]] = ZoneList;
            }

            result.Add(temp);
        }

        return result;
    }

    public List<Dictionary<string, List<MathPoint>>> GetHoleTabletagsPosition(Ansicht swAnsicht)
    {
        // Dim swviews As Object
        IView swView;
        INote swNote;
        string noteText;
        MathPoint ip;
        string zz;
        var tagNew = new Dictionary<string, List<MathPoint>>();
        var pointList = new List<MathPoint>();
        var tagList = new List<Dictionary<string, List<MathPoint>>>();
        int zz1 = 0;
        int i = 0;
        // swviews = Blatt.GetViews
        // tagList = swAnsicht.HoletableTags


        foreach (var Tag in swAnsicht.HoletableTags)
        {
            // For i = 0 To UBound(swviews)
            // swView = swviews(i)
            swView = (View)swAnsicht.holetab[zz1].GetFeature().GetOwnerFeature().GetSpecificFeature2();
            var viewNotes = swView.GetNotes().AsArrayOfType<INote>();
            for (int j = 0; j <= viewNotes.Length; j++)
            {
                swNote = viewNotes[j];
                noteText = swNote.PropertyLinkedText;
                if (Tag.ContainsKey(noteText))
                {
                    if (tagList.Count == 0)
                    {
                        pointList = new List<MathPoint>();
                    }
                    else if (i >= tagList.Count)
                    {
                        pointList = new List<MathPoint>();
                    }
                    else
                    {
                        pointList = tagList[i][noteText];
                    }

                    ip = swNote.IGetTextPoint2();
                    var point = ip.ArrayData.AsArrayOfType<double>();
                    zz = this.Blatt.GetDrawingZone(point[0], point[1]);

                    pointList.Add(ip);
                    // Tag(noteText).Add(ip)
                    tagNew[noteText] = pointList;
                    // Tag(noteText) = pointList
                }
            }
            // Next
            tagList.Add(tagNew);
            tagNew = new Dictionary<string, List<MathPoint>>();
            i = i + 1;
            zz1 = zz1 + 1;
        }

        return tagList;
    }

    public List<Dictionary<string, List<string>>> GetHoleTabletagsPosition1(Ansicht swAnsicht)
    {
        var temp = new Dictionary<string, List<string>>();
        var result = new List<Dictionary<string, List<string>>>();
        var plist = new List<MathPoint>();
        string Zone = "";
        string zz;
        // Dim tagList As List(Of Dictionary(Of String, List(Of MathPoint)))
        var Tagnew = new Dictionary<string, List<string>>();

        // tagList = swAnsicht.HoletableTags

        // Für jede Tabelle
        foreach (var Tag in swAnsicht.HoletableTags)
        {
            // Punkteliste
            Tagnew = new Dictionary<string, List<string>>();
            foreach (var n in Tag)
            {
                plist = Tag[n.Key];
                var temp2 = new List<string>();
                // Für jeden Punkt
                foreach (var k in plist)
                {
                    var point = k.AsArrayOfType<double>();

                    zz = this.Blatt.GetDrawingZone(point[0], point[1]);
                    temp2.Add(zz);
                }
                // tag(n.Key) = pointList
                Tagnew[n.Key] = temp2;
            }
            result.Add(Tagnew);
        }

        return result;
    }

    /// <summary>
    /// ermittelt die Passungen für jede Ansicht
    /// </summary>
    /// <returns></returns>
    public bool PassungsTabelleGetDimensions()
    {
        bool PassungsTabelleGetDimensionsRet = default;
        int dokt_setup = 0;
        int dokt = 0;
        string tempzone;
        bool MarkerCominedHoleTab = false;

        // Falls es noch keine Tabelle gibt, dann eine neue erzeugen
        if (this.Tabelle is null)
        {
            this.Tabelle = new Passungstabelle_Tabelle(this.Attr_generell, this.Attr_Tabelle, this.Attr_Übersetzungen, this.Blatt);
            this.Tabelle.Log = this.Log;
        }

        foreach (var ans in this.Ansichten)
        {
            MarkerCominedHoleTab = false;
            // Ansichtsnamen in Log-Datei schreiben
            this.Log.WriteInfo(ans.ansichtsName, "", false);

            // Passungen in der Ansicht suchen
            this.Tabelle.GetViewDimension(ans.ViewRef);

            // Wenn Bohrungstabellen gefunden wurde, dann wir sie untersucht
            if (ans.holetab is not null)
            {
                if (ans.holetab.Count > 0)
                {
                    if (ans.holetab[0].CombineTags)
                    {
                        MarkerCominedHoleTab = true;

                        var annotation = ans.holetab[0].GetTableAnnotations().AsArrayOfType<ITableAnnotation>().FirstOrDefault()?.GetAnnotation();

                        annotation!.Visible = (int)swAnnotationVisibilityState_e.swAnnotationHidden;
                        ans.holetab[0].CombineTags = false;
                    }
                    //ToDo: WTF?
                    ans.HoletableTags = this.GetHoleTabletags(ans.holetab);
                    ans.HoletableZones = this.GetHoleTabletags1(ans.holetab);
                    ans.HoletableTags = this.GetHoleTabletagsPosition(ans);
                    ans.HoletableZones = this.GetHoleTabletagsPosition1(ans);
                    this.Tabelle.GetHoleTableDimension(ans.holetab, ans.ViewRef, ans.HoletableZones);
                }
                if (MarkerCominedHoleTab)
                {
                    ans.holetab[0].CombineTags = true;

                    var annotation = ans.holetab[0].GetTableAnnotations().AsArrayOfType<ITableAnnotation>().FirstOrDefault()?.GetAnnotation();
                    annotation!.Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
                }
            }
            // Tabelle.getHoleTableDimension(ans.ViewRef)
        }

        // Einfügeposition setzen
        this.Tabelle.EinfügePosition = this.Einfügeposition;

        // Sortiert die Tabelle und entfernt doppelte Einträge
        this.Tabelle.SetTabellenzeilenGefiltert();

        // Zonen addieren und für die gefilterten Zeilen setzen
        foreach (var gef in this.Tabelle.TabellenZeilengefiltert)
        {
            tempzone = gef.Zone;
            gef.Zone = "";
            foreach (var zeile in this.Tabelle.TabellenZeilen)
            {
                if ((zeile.Maß.ToString() + zeile.Passung ?? "") == (gef.Maß.ToString() + gef.Passung ?? ""))
                {
                    if (string.IsNullOrEmpty(gef.Zone))
                    {
                        gef.Zone = tempzone;
                    }
                    else
                    {
                        gef.Zone = gef.Zone + "/" + zeile.Zone;
                    }
                }
            }
        }

        // Zonen sortieren
        string[] stringarray;
        foreach (var gef in this.Tabelle.TabellenZeilengefiltert)
        {
            stringarray = gef.Zone.Split('/');
            Array.Sort(stringarray);
            gef.Zone = string.Join("/", stringarray);
        }

        PassungsTabelleGetDimensionsRet = true;
        return PassungsTabelleGetDimensionsRet;
    }

    /// <summary>
    /// Ermittelt den Dokumenttyp der ausgewertet werden soll
    /// </summary>
    /// <returns></returns>
    private int GetDokumentTypeFromSetup()
    {
        var dokt_setup = default(int);

        if (this.Attr_generell.AnsichtsTypSkizzen)
            dokt_setup = 1;
        if (this.Attr_generell.AnsichtsTypTeile)
            dokt_setup ^= 2;
        if (this.Attr_generell.AnsichtsTypBaugruppen)
            dokt_setup ^= 4;

        return dokt_setup;
    }

    /// <summary>
    /// bestimmt den Ansichtstyp der Ansicht
    /// </summary>
    /// <param name="ans"></param>
    /// <returns></returns>
    private int GetDocumentTypeFromRef(View ans)
    {
        int dokt = 0;
        if (ans.ReferencedDocument is null)
        {
            dokt = dokt ^ 1;
        }
        else
        {
            if (ans.ReferencedDocument.GetType() == (int)swDocumentTypes_e.swDocPART)
            {
                dokt = dokt ^ 2;
            }
            else if (ans.ReferencedDocument.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                dokt = dokt ^ 4;
            }
        }
        return dokt;
    }

    /// <summary>
    /// ermittelt die Blatteigenschaften
    /// </summary>
    /// <returns></returns>
    private Definitionen.BlattEigenschaften GetSheetProperties()
    {
        Definitionen.BlattEigenschaften GetSheetPropertiesRet = default;
        double[] ss;
        var prop = new Definitionen.BlattEigenschaften();

        // Blatteigenschaften
        ss = this.Blatt.GetProperties2().AsArrayOfType<double>();
        // MsgBox(Blatt.GetTemplateName, vbOKOnly, "Meldung1")
        if (this.Blatt.GetTemplateName() == "*.drt" | string.IsNullOrEmpty(this.Blatt.GetTemplateName()))
        {
            prop.Formatname = "";
            this.Log.WriteInfo(My.Resources._Keine_Formatvorlage, " - " + this.Blatt.GetName(), true);
        }
        else
        {
            // Formatname ohne Pfad
            // Formatname ohne Erweiterung
            prop.Formatname = Path.GetFileNameWithoutExtension(this.Blatt.GetTemplateName());
        }

        // Blatteigenschaften
        prop.Eigenschaften = ss;
        // Sprache der SolidWorks-Installation
        prop.Sprache = this.Swapp.GetCurrentLanguage();
        GetSheetPropertiesRet = prop;
        return GetSheetPropertiesRet;
    }

    private void SetSheetAttr(Dictionary<string, FormatSettings> Attr_Formate, Dictionary<string, TableSettings> Attr_Tabellen)
    {
        // Die Eigenschaften vom Blatt holen
        this.Attr_Sheet = this.GetSheetProperties();
        // Wenn ein Name für die Formatvorlage gefunden wurde
        if (!string.IsNullOrEmpty(this.Attr_Sheet.Formatname))
        {
            // Die entsprechenden Format Einstellungen laden
            this.SetSheetFormatAttr(this.Attr_Sheet.Formatname, Attr_Formate, Attr_Tabellen);
        }
        else
        {
            // Sonst, Versuch die Formateinstellungen über die Blattabmessungen zu erhalten
            this.SetSheetFormatFromDimension(Attr_Formate, Attr_Tabellen);
        }
    }

    /// <summary>
    /// Ermittelt die Setupeinstellungen für das übergebene Format
    /// </summary>
    /// <param name="formatname"></param>
    /// <param name="Attr_Formate"></param>
    /// <param name="Attr_Tabellen"></param>
    private void SetSheetFormatAttr(string formatname, Dictionary<string, FormatSettings> Attr_Formate, Dictionary<string, TableSettings> Attr_Tabellen)
    {
        FormatSettings temp;

        // Zuerst wird versucht den Formatnamen in den Einstellungen zu finden
        try
        {
            temp = Attr_Formate[formatname];
            // Das Format wurde gefunden und wird gesetzt
            this.Attr_Format = temp;
            this.Attr_Tabelle = Attr_Tabellen[formatname];
        }
        catch (Exception ex)
        {
            // Format wurde nicht gefunden, dann müssen wir nach den Abmessungen suchen
            this.SetSheetFormatFromDimension(Attr_Formate, Attr_Tabellen);
        }
    }


    /// <summary>
    /// sucht nach einem Format in den Setup Einstellungen, das die gleichen Abmessungen hat wie das aktuelle Blattformat
    /// </summary>
    /// <param name="Attr_Formate">Format Attribute</param>
    /// <param name="Attr_Tabellen">Tabellen Attribute</param>
    private void SetSheetFormatFromDimension(Dictionary<string, FormatSettings> Attr_Formate, Dictionary<string, TableSettings> Attr_Tabellen)
    {
        FormatSettings temp;
        string tempName = "";
        bool isFirstLoop = false;

        // Suche nach einem Format das die gleichen Abmessungen hat wie das Blatt
        foreach (KeyValuePair<string, FormatSettings> n in Attr_Formate)
        {
            temp = n.Value;
            // beim ersten Durchlauf wird der Setup-Formatname des ersten Eintrags gespeichert
            // für den Fall, dass kein Format mit den selben Abemssungen gefunden wird,
            // werden die Werte vom ersten Eintrag im Setup übernommen
            if (!isFirstLoop)
            {
                isFirstLoop = true;
                tempName = n.Key;
            }
            // Wenn ein Setupeintrag mit den gleichen Abmessungen gefunden wird
            if (temp.Höhe == Attr_Sheet.Eigenschaften[6] * 1000 && temp.Breite == Attr_Sheet.Eigenschaften[5] * 1000)
            {
                // Format Attribut zuweisen
                this.Attr_Format = temp;
                // Tabellen Attribut zuweisen
                this.Attr_Tabelle = Attr_Tabellen[n.Key];
                // Log-Info schreiben
                Log.WriteInfo(My.Resources._nicht_gefunden, $" {My.Resources.Format} {Attr_Sheet.Formatname}", true);
                this.Log.WriteInfo(My.Resources._Abmessungen_von, " " + n.Key, true);
                return;
            }
        }

        // Es wurde keine Format mit dem Formatvorlagenamen gefunden
        // deshalb werden die Werte vom ersten Format genommen

        this.Log.WriteInfo(My.Resources.Kein_Format_mit_den_Abmessungen, $" {Attr_Sheet.Eigenschaften[6] * 1000} x {Attr_Sheet.Eigenschaften[5] * 1000} {My.Resources.gefunden}", false);
        this.Log.WriteInfo(My.Resources._Abmessungen_von_ersten_definierten_Format, " " + tempName, true);

        // Format Attribut vom ersten Eintrag zuweisen
        this.Attr_Format = Attr_Formate[tempName];
        // Tabellen Attribut vom ersten Eintrag zuweisen
        this.Attr_Tabelle = Attr_Tabellen[tempName];
    }

    // Sub:       DeleteTab
    // löscht die PassungstabellenHandler
    // es wird davon ausgegangen, dass das Blatt, auf dem sich die Tabelle befindet aktiv ist
    // Parameter: keine
    public void DeleteTab()
    {
        ModelDoc2 modeldoc;

        // Wenn keine Tabelle gesetzt wurde, dann beenden
        if (this.AlteTabelle is null)
        {
            return;
        }

        modeldoc = (ModelDoc2)this.swdraw;
        modeldoc.Extension.SelectByID2("PASSUNGSTABELLE@" + this.Blatt.GetName(), "ANNOTATIONTABLES", 0d, 0d, 0d, false, 0, null, 0);
        modeldoc.EditDelete();

        this.AlteTabelle = null;
    }
    // Sub:       CheckForOldTable
    // sucht nach einer vorhandenen PassungstabellenHandler und setzt die entsprechenden Werte
    // Parameter: Ansicht in der gesucht werden soll
    public void CheckForOldTable(View swView)
    {
        // Alle Tabellen in der Ansicht ermitteln
        var tables = swView.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();

        // Alle Tabellen durchlaufen

        foreach (ITableAnnotation table in tables)
        {
            // Das Annotation-objekt ermitteln
            var annotation = table.GetAnnotation();

            // Wenn es sich um eine PassungstabellenHandler handelt
            if (annotation.GetName() == "PASSUNGSTABELLE")
            {
                // Wenn die PassungstabellenHandler nicht verdeckt ist
                var visible = annotation.Visible != (int)swAnnotationVisibilityState_e.swAnnotationHidden;

                // Einfügeposition speichern
                var position = annotation.GetPosition().AsArrayOfType<double>();
                this.AlteTabelleX = position[0];
                this.AlteTabelleY = position[1];
                // Verweis auf Tabellen-Objekt speichern
                this.AlteTabelle = table;

                break;
            }
        }
    }
    // Function:  CheckForHoleTable
    // prüft ob sich in der übergebenen Ansicht eine Bohrungstabelle befindet
    // Parameter: swView SWX Ansicht
    // Ergebnis:  Bohrungstabelle-Objekt 
    public List<HoleTable> CheckForHoleTable(View swView)
    {
        var swholetables = new List<HoleTable>();

        // Alle Tabellen in der Ansicht ermitteln
        var tables = swView.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();

        // Alle Tabellen durchlaufen
        foreach (ITableAnnotation table in tables)
        {
            // Wenn es sich um eine Bohrungstabelle handelt
            // dann Ende der Funktion
            // Wir gehen davon aus, dass es nur eine Bohrungstabelle in einer Ansicht gibt
            if (table.Type == (int)swTableAnnotationType_e.swTableAnnotation_HoleChart)
            {
                var holeTable = (HoleTableAnnotation)table;
                swholetables.Add(holeTable.HoleTable);
            }
        }

        return swholetables;
    }
    // Sub:       SetEinfügepunkt
    // setzt die Koordinaten des Einfügepunkts der Tabelle an Hand der Setup Einstellungen
    // Parameter: keine
    public void SetEinfügepunkt()
    {
        var temp = new double[2];

        // X/Y Koordinaten des Einfügepunkts setzen
        // Links-Oben
        if (this.Attr_Format.Einfügepunkt == Einfügepunkt.TopLeft)
        {
            temp[0] = 0.0d;
            temp[1] = this.Attr_Format.Höhe / 1000.0d;
        }
        // Links-Unten
        else if (this.Attr_Format.Einfügepunkt == Einfügepunkt.TopRight)
        {
            temp[0] = 0.0d;
            temp[1] = 0.0d;
        }
        // Rechts-Oben
        else if (this.Attr_Format.Einfügepunkt == Einfügepunkt.TopRight)
        {
            temp[0] = this.Attr_Format.Breite / 1000.0d;
            temp[1] = this.Attr_Format.Höhe / 1000.0d;
        }
        // Rechts-Unten
        else if (this.Attr_Format.Einfügepunkt == Einfügepunkt.BottomRight)
        {
            temp[0] = this.Attr_Format.Breite / 1000.0d;
            temp[1] = 0.0d;
        }

        // Offset berücksichtigen
        temp[0] = temp[0] + this.Attr_Format.Offset_X / 1000.0d;
        temp[1] = temp[1] + this.Attr_Format.Offset_Y / 1000.0d;

        this.Einfügeposition = temp;
    }
    // Function:  GetColumnsCount
    // ermittelt die Anzahl der Spalten der Tabelle
    // Parameter: keine
    // Ergebnis:  Anzahl der Spalten
    private int GetColumnsCount()
    {
        return this.Attr_Tabelle.Spalten.Count(o => o.Visible);
    }
    // Sub        SetEinfügePunktPosition
    // setzt den Verankerungspunkt der Tabelle
    // Parameter: keine
    public void SetEinfügePunktPosition()
    {
        this.Tabelle.Einfügepunkt = this.Attr_Format.Einfügepunkt;
    }

    public void InsertTable()
    {

        this.SetEinfügepunkt();

        // Wenn nicht neu positioniert werden soll und es eine alte Tabelle gibt
        // dann wird der Einfügeposition der alten Tabelle übernommen
        if (!this.Attr_generell.NeuPositionieren & this.AlteTabelle is not null)
        {
            this.Tabelle.EinfügePosition[0] = this.AlteTabelleX;
            this.Tabelle.EinfügePosition[1] = this.AlteTabelleY;
        }

        // Spalten bestimmen
        this.Tabelle.TabellenSpaltenCount = this.GetColumnsCount();

        // Neue Tabelle einfügen
        if (this.Tabelle.TabellenZeilen.Count > 0)
            this.Tabelle.InsertTable(this.swdraw, this.Blatt);
    }
}