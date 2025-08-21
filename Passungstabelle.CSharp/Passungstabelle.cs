using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

using System.Data;
using static System.Environment;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using My = Passungstabelle.CSharp.My;
using Microsoft.Win32;
using Passungstabellen.CSharp;

namespace Passungstabelle.CSharp;


public class Passungstabelle
{
    public string Macro_pfad { get; set; }
    public string Log_pfad { get; set; }
    public string Setup_pfad { get; set; }
    public DataSet XMLDaten { get; set; } = new DataSet();
    public GeneralSettings Attr_generell { get; set; } = new GeneralSettings();
    public Dictionary<string, Dictionary<string, string>> Attr_Übersetzungen { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    public Dictionary<string, FormatSettings> Attr_Formate { get; set; } = new();
    public Dictionary<string, TableSettings> Attr_Tabelle { get; set; } = new();
    // 2022-06-24
    public Dictionary<string, Dictionary<string, bool>> Attr_Meldungen { get; set; } = new Dictionary<string, Dictionary<string, bool>>();
    public Dictionary<string, string> Res_Meldungen { get; set; } = new Dictionary<string, string>();
    // ******************

    public DateTime Setup_Date_Time { get; set; }

    private LogFile Log = new LogFile();

    public void ResTest()
    {
        throw new NotImplementedException();
        //string stf = Passungstabellen.My.MyProject.Application.Info.DirectoryPath + @"\Resources.resx";
        //var rr = new ResXResourceReader(stf);

        //rr.UseResXDataNodes = true;
        //string k1 = "";
        //string v1 = "";
        //string c1 = "";
        //IDictionaryEnumerator dict;

        //try
        //{
        //    dict = rr.GetEnumerator();
        //}
        //catch (Exception ex)
        //{
        //    Interaction.MsgBox("Datei nicht gefunden", Constants.vbOKOnly, "Meldung");
        //    Interaction.MsgBox(Passungstabellen.My.MyProject.Application.Info.DirectoryPath, Constants.vbOKOnly, "Meldung");
        //    return;
        //}

        //ResXDataNode node;

        //while (dict.MoveNext())
        //{
        //    node = (ResXDataNode)dict.Value;
        //    // Debug.Print(node.Name + " - " + node.Comment)
        //    // If dict.Value Then
        //    ITypeResolutionService typeres = null;
        //    c1 = node.Comment;
        //    if (c1 == "K1")
        //    {
        //        k1 = node.Name;
        //        v1 = Conversions.ToString(node.GetValue(typeres));
        //    }
        //}
        //Interaction.MsgBox(k1 + " - " + v1 + " - " + c1);
    }

    public void ReadResources()
    {
        var myDictionary = this.GetMyResourcesDictionary();
        foreach (KeyValuePair<string, object> kvp in myDictionary)
        {
            string name = kvp.Key;
            if (kvp.Key is string)
            {
                if (kvp.Key.ToString().Substring(0, 1) == "#")
                {
                    this.Res_Meldungen.Add(kvp.Key, Conversions.ToString(kvp.Value));
                }
            }
        }
    }

    public Dictionary<string, object> GetMyResourcesDictionary()
    {
        throw new InvalidOperationException();
        //var ItemDictionary = new Dictionary<string, object>();
        //IDictionaryEnumerator ItemEnumerator;
        //ResourceSet ItemResourceSet;
        //var ResourceNameList = new List<string>();
        //var cinfo = Thread.CurrentThread.CurrentUICulture;

        //if (cinfo.TwoLetterISOLanguageName != "de")
        //{
        //    // Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        //    ItemResourceSet = My.Resources.ResourceManager.GetResourceSet(new CultureInfo("en-US"), true, true);
        //}
        //else
        //{
        //    // Thread.CurrentThread.CurrentUICulture = New CultureInfo("de-DE")
        //    ItemResourceSet = My.Resources.ResourceManager.GetResourceSet(new CultureInfo("de-DE"), true, true);
        //}

        //// ItemResourceSet = My.Resources.ResourceManager.GetResourceSet(New System.Globalization.CultureInfo("en"), True, True)

        //// Get the enumerator for My.Resources
        //ItemEnumerator = ItemResourceSet.GetEnumerator();

        //while (ItemEnumerator.MoveNext())
        //    ResourceNameList.Add(ItemEnumerator.Key.ToString());

        //foreach (string resourceName in ResourceNameList)
        //    ItemDictionary.Add(resourceName, this.GetItem(resourceName));

        //ResourceNameList = null;

        //return ItemDictionary;
    }

    public object GetItem(string resourceName)
    {
        throw new InvalidOperationException();
        //return My.Resources.ResourceManager.GetObject(resourceName);
    }


    // Sub Main
    // Parameter: swapp       (SolidWorks ModelDoc Objekt)
    // iDrawingDoc (SolidWorks ModelDoc2 Objekt) oder kein Parameter
    // der optionale Parameter ist notwendig, weil diese Sub 
    // einmal vom Commandmanager ohne 2. Parameter und
    // einmal vom Event mit 2. Parameter aufgerufen wird
    public void Main(SldWorks swapp, ModelDoc2 iDrawingDoc = null)
    {
        ModelDoc2 swmod;
        var pd = new Passungstabelle_Datei(swapp);
        bool Passungengefunden = false;

        // ResTest()

        this.ReadResources();

        // Wenn keine Parameter vorhanden ist
        if (iDrawingDoc is not null)
        {
            swmod = iDrawingDoc;
        }
        else
        {
            swmod = (ModelDoc2)swapp.ActiveDoc;
        }

        // Makropfad ermitteln
        this.Macro_pfad = this.GetAppPath();
        this.Setup_pfad = this.GetSetupPath();

        // Pfad für Log-Datei setzen
        this.Log_pfad = this.GetLogPath() + @"\" + Definitionen.LOGName;

        // Wenn keine Daten geladen sind oder sich die Setup Datei geändert hat, dann Setup Daten einlesen
        if (this.Setup_has_changed())
        {
            // Wenn keine Setup-Datei gefunden werden kann, dann Sub beenden
            if (!this.Check_for_setup())
                return;
        }

        // Setup Daten der Datei zuordnen
        pd.Attr_generell = this.Attr_generell;
        pd.Attr_Übersetzungen = this.Attr_Übersetzungen;
        pd.Attr_Formate = this.Attr_Formate;
        pd.Attr_Tabelle = this.Attr_Tabelle;
        // pd.Attr_Meldungen = Attr_Meldungen

        pd.Swapp = swapp;

        // Eigenschaften für das Log-Objekt setzen
        this.Log.Attr_generell = this.Attr_generell;
        this.Log.Attr_Meldungen = this.Attr_Meldungen;

        // Log-Info schreiben
        this.Log.WriteInfo("Start", "", false);

        // Dim s As DateTime = Now.ToLocalTime (Das war nur mal um die Geschwindigkeit zu bestimmen)

        // Das Log-Datei-Objekt dem Passungstabellen-Datei-Objekt zuordnen
        pd.Log = this.Log;

        // Prüfung ob eine Zeichnung aktiv ist
        if (!this.Check_for_drawing(swmod))
        {
            // Log.WriteInfo("Keine Zeichnung geladen", True)
            return;
        }

        // Wenn Keine Blätter in der Zeichnung vorhanden sind, dann beenden
        if (!pd.PassungsTabelleGetSheets(swmod))
        {
            this.Log.WriteInfo(My.Resources._Keine_Blätter_in_der_Zeichnung, "", true);
            return;
        }

        // Tabelle einfügen
        pd.InsertTableOnSheets();
        // Log-Info schreiben
        this.Log.WriteInfo(My.Resources.Fertig, "", false);

        pd = null;
    }
    // Function:  Check_for_drawing
    // Parameter: swmod (SolidWorks ModelDoc Objekt)
    // Ergebnis:  True wenn das Dokument eine Zeichnung ist sonst False
    public bool Check_for_drawing(ModelDoc2 swmod)
    {
        throw new NotImplementedException();
        //bool Check_for_drawingRet = default;
        //// Wenn Swmod keinen Wert hat, dann ist auch nichts geladen
        //if (swmod is null)
        //{
        //    // Log.WriteInfo("Keine Datei geladen", True)
        //    this.Log.WriteInfo(Passungstabellen.My.Resources.Resources._Keine_Datei_geladen, "", true);
        //    Check_for_drawingRet = false;
        //    return Check_for_drawingRet;
        //}
        //// Wenn swmod keine Zeichnung ist
        //if (swmod.GetType() != (int)swDocumentTypes_e.swDocDRAWING)
        //{
        //    // Log.WriteInfo("Keine Zeichnung geladen", True)
        //    this.Log.WriteInfo(Passungstabellen.My.Resources.Resources._Keine_Zeichnung_geladen, "", true);
        //    Check_for_drawingRet = false;
        //    return Check_for_drawingRet;
        //}
        //Check_for_drawingRet = true;
        //return Check_for_drawingRet;
    }
    // Function:  Check_For_setup
    // Parameter: keine
    // Ergebnis:  True wenn eine Setup-Datei vorhanden ist und diese auch gelesen werden konnte
    public bool Check_for_setup()
    {
        throw new NotImplementedException();
        //bool Check_for_setupRet = default;
        //string pfad;
        //bool ok;


        //// Macro_pfad = GetAppPath()
        //this.Setup_pfad = this.GetSetupPath();
        //pfad = this.Setup_pfad + Passungstabellen.Definitionen.INI_File;

        //var xmlSR = new StringReader(Passungstabellen.My.Resources.Resources.Setup_Schema);

        //// Schema initialisieren
        //this.XMLDaten.Clear();
        //this.XMLDaten.ReadXmlSchema(xmlSR);

        //// Versuch die Setup-Datei einzulesen
        //try
        //{
        //    this.XMLDaten.ReadXml(pfad);
        //    ok = true;
        //}
        //catch
        //{
        //    ok = false;
        //    // MsgBox("Keine Setup.XML Datei gefunden" & Chr(10) & "Bitte verwenden Sie das Setup-Makro um die Einstellungen zu erzeugen", vbOKOnly, "Passungstabelle Addin")
        //    Interaction.MsgBox(Passungstabellen.My.Resources.Resources.Keine_Setup_XML_Datei_gefunden + '\n' + Passungstabellen.My.Resources.Resources.Bitte_verwenden_Sie_das_Setup_Makro_um_die_Einstellungen_zu_erzeugen, Constants.vbOKOnly, Passungstabellen.My.Resources.Resources.Passungstabelle_Addin);
        //    Check_for_setupRet = false;
        //    return Check_for_setupRet;
        //}

        //// Wenn die Setup-Datei gelesen werden konnte werden die Attribute eingelesen
        //if (this.Attr_read() == false)
        //{
        //    Check_for_setupRet = false;
        //}
        //else
        //{
        //    Check_for_setupRet = true;
        //}

        //return Check_for_setupRet;
    }
    // Sub:       Attr_read
    // Parameter: keine
    // liest die Setup-Datei ein
    public bool Attr_read()
    {
        bool Attr_readRet = default;
        this.Attr_generell = this.Attr_get_generell();
        this.Attr_Übersetzungen = this.Attr_get_übersetzungen();
        this.Attr_Formate = this.Attr_get_formate();
        this.Attr_Tabelle = this.Attr_get_Tabelle();
        this.Attr_Meldungen = this.Attr_get_Meldungen();

        if (this.Attr_generell is null | this.Attr_Übersetzungen is null | this.Attr_Formate is null | this.Attr_Tabelle is null)
        {
            Attr_readRet = false;
            return Attr_readRet;
        }

        this.Set_Setup_date();
        Attr_readRet = true;
        return Attr_readRet;
    }
    // Sub:       Set_Setup_date
    // Parameter: keine
    // Speichert das Änderungsdatum der Setup-Datei 
    public void Set_Setup_date()
    {
        throw new NotImplementedException();
        //string pfad;
        //// pfad = macro_pfad & "\" & Definitionen.INI_File
        //pfad = this.Setup_pfad + Passungstabellen.Definitionen.INI_File;
        //this.Setup_Date_Time = File.GetLastWriteTime(pfad);
    }
    // Function:  Setup_has_changed
    // Parameter: keine
    // Ergebnis:  True wenn sich das Änderungsdatum der Setup-Datei geändert hat sonst False
    // Prüft ob sich das Datum der letzten Speicherung geändert hat
    public bool Setup_has_changed()
    {
        throw new NotImplementedException();
        //bool Setup_has_changedRet = default;
        //string pfad;

        //// pfad = macro_pfad & "\" & Definitionen.INI_File
        //pfad = this.Setup_pfad + Passungstabellen.Definitionen.INI_File;

        //if (this.Setup_Date_Time < File.GetLastWriteTime(pfad))
        //{
        //    this.Setup_Date_Time = File.GetLastWriteTime(pfad);
        //    Setup_has_changedRet = true;
        //    return Setup_has_changedRet;
        //}
        //Setup_has_changedRet = false;
        //return Setup_has_changedRet;
    }

    public void SaveSetup()
    {
        throw new NotImplementedException();
        //FileInfo fInfo;
        //fInfo = new FileInfo(this.Setup_pfad + Passungstabellen.Definitionen.INI_File);

        //if (fInfo.IsReadOnly)
        //{
        //    this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Setupdatei_ist_schreibgeschützt, "", false);
        //    return;
        //}

        //// XMLWriterSettings intialisieren
        //var settings = new XmlWriterSettings() { Indent = true, IndentChars = "   ", NewLineOnAttributes = true };
        //var XmlWrt = XmlWriter.Create(this.Setup_pfad + Passungstabellen.Definitionen.INI_File, settings);

        //// Änderungen im Dataset speichern
        //this.XMLDaten.AcceptChanges();
        //// Daten schreiben
        //this.XMLDaten.WriteXml(XmlWrt, (XmlWriteMode)Conversions.ToInteger(true));
        //// Datei schließen
        //XmlWrt.Close();
    }
    // Liest die Generellen Einstellungen ein
    public GeneralSettings Attr_get_generell()
    {
        throw new NotImplementedException();
        //Dictionary<string, string> Attr_get_generellRet = default;
        //var temp = new Dictionary<string, string>();
        //DataTable dt;
        //DataRow dr;
        //string attrname = "";
        //bool SaveNeeded = false;

        //dt = this.XMLDaten.Tables["GenerelleAttribute"];
        //dr = dt.Rows[0];
        //foreach (KeyValuePair<string, string> n in Passungstabellen.Definitionen.GENERELLE_ATTR)
        //{
        //    try
        //    {
        //        attrname = n.Key;
        //        temp[n.Key] = Conversions.ToString(dr[n.Key]);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log.WriteInfo("Fehler beim Lesen des Attributes '" & attrname & "' im Abschnitt 'generelle Attribute'" & Chr(10) & "Makro 'Passungstabelle' abgebrochen", False)
        //        this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Fehler_beim_Lesen_des_Attributes + "'" + attrname + Passungstabellen.My.Resources.Resources.im_Abschnitt__generelle_Attribute + '\n' + Passungstabellen.My.Resources.Resources.Makro_Passungstabelle_abgebrochen, "", false);
        //        // Attr_get_generell = Nothing
        //        // Exit Function
        //        dr[n.Key] = Passungstabellen.Definitionen.GENERELLE_ATTR_Init[attrname];
        //        temp[n.Key] = Passungstabellen.Definitionen.GENERELLE_ATTR_Init[attrname];
        //        SaveNeeded = true;
        //    }
        //}
        //if (SaveNeeded)
        //    this.SaveSetup();
        //Attr_get_generellRet = temp;
        //return Attr_get_generellRet;
    }
    // Liest die Übersetzungen ein
    public Dictionary<string, Dictionary<string, string>> Attr_get_übersetzungen()
    {
        throw new NotImplementedException();
        //Dictionary<string, Dictionary<string, string>> Attr_get_übersetzungenRet = default;
        //var temp = new Dictionary<string, Dictionary<string, string>>();
        //DataTable dt;
        //DataRow dr;
        //int i;
        //string attrname = "";
        //bool SaveNeeded = false;
        //dt = this.XMLDaten.Tables["Übersetzung"];

        //var loopTo = dt.Rows.Count - 1;
        //for (i = 0; i <= loopTo; i++)
        //{
        //    dr = dt.Rows[i];
        //    var temp1 = new Dictionary<string, string>();
        //    foreach (KeyValuePair<string, string> n in Passungstabellen.Definitionen.ÜBERSETZUNGSATTR)
        //    {
        //        try
        //        {
        //            attrname = n.Key;
        //            temp1[n.Key] = Conversions.ToString(dr[n.Key]);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log.WriteInfo("Fehler beim Lesen des Attributes '" & attrname & "' im Abschnitt 'Übersetzung'" & Chr(10) & "Vorgabewert wird gesetzt", False)
        //            this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Fehler_beim_Lesen_des_Attributes + "'" + attrname + Passungstabellen.My.Resources.Resources.im_Abschnitt_Übersetzung + '\n' + Passungstabellen.My.Resources.Resources.Vorgabewert_wird_gesetzt, "", false);
        //            // Attr_get_übersetzungen = Nothing
        //            // Exit Function
        //            dr[n.Key] = Passungstabellen.Definitionen.ÜBERSETZUNGSATTR_Init[attrname];
        //            temp1[n.Key] = Passungstabellen.Definitionen.ÜBERSETZUNGSATTR_Init[attrname];
        //            SaveNeeded = true;
        //        }
        //    }
        //    temp[Conversions.ToString(dr["Kürzel"])] = temp1;
        //}
        //if (SaveNeeded)
        //    this.SaveSetup();
        //Attr_get_übersetzungenRet = temp;
        //return Attr_get_übersetzungenRet;
    }

    // Liest die Übersetzungen ein
    public Dictionary<string, Dictionary<string, bool>> Attr_get_Meldungen()
    {
        throw new NotImplementedException();
        //Dictionary<string, Dictionary<string, bool>> Attr_get_MeldungenRet = default;
        //var temp = new Dictionary<string, Dictionary<string, bool>>();
        //DataTable dt;
        //DataRow dr;
        //int i;
        //string attrname = "";
        //bool SaveNeeded = false;

        //if (this.Res_Meldungen.Count == 0)
        //{
        //    Attr_get_MeldungenRet = temp;
        //    return Attr_get_MeldungenRet;
        //}

        //dt = this.XMLDaten.Tables["Meldungen"];

        //// Wenn Daten für die Meldungen vorhanden sind
        //if (dt is not null)
        //{
        //    // Jeden Datensatz durchlaufen
        //    var loopTo = dt.Rows.Count - 1;
        //    for (i = 0; i <= loopTo; i++)
        //    {
        //        dr = dt.Rows[i];
        //        var temp1 = new Dictionary<string, bool>();
        //        try
        //        {
        //            // Datensatz/Meldung hinzufügen
        //            temp1.Add(Conversions.ToString(dr["Meldung_Text"]), Conversions.ToBoolean(dr["Meldung_anzeigen"]));
        //            temp.Add(Conversions.ToString(dr["Meldung"]), temp1);
        //        }
        //        catch (Exception ex)
        //        {
        //            this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Fehler_beim_Lesen_des_Attributes + "'" + attrname + Passungstabellen.My.Resources.Resources.im_Abschnitt_Übersetzung + '\n' + Passungstabellen.My.Resources.Resources.Vorgabewert_wird_gesetzt, "", false);
        //            // SaveNeeded = True
        //        }
        //    }
        //}

        //// Wenn keine Datensätze gefunden wurden
        //// ODER
        //// die Anzahl der gefundenen unterschiedlich zu den gefundenen Einträgen in der Resource sind
        //if (temp.Count == 0 | temp.Count != this.Res_Meldungen.Count)
        //{
        //    // Alle Resourcen durchlaufen
        //    foreach (var n in this.Res_Meldungen)
        //    {
        //        // Wenn kein Datensatz gefunden wurde
        //        if (!temp.ContainsKey(n.Key))
        //        {
        //            // Datensatz hinzufügen
        //            var temp2 = new Dictionary<string, bool>();
        //            temp2.Add(n.Value, false);
        //            temp.Add(n.Key, temp2);
        //        }
        //    }
        //}

        //// Wenn keine Datensätze in der Tabelle vorhanden sind
        //if (dt.Rows.Count == 0)
        //{
        //    // Alle Datensätze intialisieren
        //    foreach (var n in temp)
        //    {
        //        dr = dt.NewRow();
        //        dr["Meldung"] = n.Key;
        //        foreach (var v1 in n.Value)
        //        {
        //            dr["Meldung_Text"] = v1.Key;
        //            dr["Meldung_anzeigen"] = v1.Value;
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //}
        //// 20240712 SaveSetup()
        //else if (dt.Rows.Count != this.Res_Meldungen.Count)
        //{
        //    foreach (var n in temp)
        //    {
        //        DataRow[] drs;
        //        drs = dt.Select("Meldung='" + n.Key + "'");
        //        if (drs.Length == 0)
        //        {
        //            dr = dt.NewRow();
        //            dr["Meldung"] = n.Key;
        //            foreach (var v1 in n.Value)
        //            {
        //                dr["Meldung_Text"] = v1.Key;
        //                dr["Meldung_anzeigen"] = v1.Value;
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    // 20240712 SaveSetup()
        //}

        //var loopTo1 = dt.Rows.Count - 1;
        //for (i = 0; i <= loopTo1; i++)
        //{
        //    dr = dt.Rows[i];
        //    var temp1 = new Dictionary<string, bool>();
        //    try
        //    {
        //        if (this.Res_Meldungen.ContainsKey(Conversions.ToString(dr["Meldung"])))
        //        {
        //            dr["Meldung_Text"] = this.Res_Meldungen[Conversions.ToString(dr["Meldung"])];
        //        }
        //    }
        //    // Datensatz/Meldung hinzufügen
        //    catch (Exception ex)
        //    {
        //        // Log.WriteInfo(My.Resources.Fehler_beim_Lesen_des_Attributes & "'" & attrname & My.Resources.im_Abschnitt_Übersetzung & Chr(10) & My.Resources.Vorgabewert_wird_gesetzt, False)
        //        // SaveNeeded = True
        //    }
        //}
        //// 20240712 SaveSetup()
        //Attr_get_MeldungenRet = temp;
        //return Attr_get_MeldungenRet;
    }


    // Liest die Formateinstellungen ein
    public Dictionary<string, FormatSettings> Attr_get_formate()
    {
        throw new NotImplementedException();
        //Dictionary<string, Dictionary<string, string>> Attr_get_formateRet = default;
        //var temp = new Dictionary<string, Dictionary<string, string>>();
        //DataTable dt;
        //DataRow dr;
        //DataTable dtf;
        //DataRow drf;
        //int i;
        //string attrname = "";
        //int id;
        //bool SaveNeeded = false;

        //dt = this.XMLDaten.Tables["FormatAttribute"];
        //dtf = this.XMLDaten.Tables["Format"];

        //// Formate durchlaufen
        //var loopTo = dtf.Rows.Count - 1;
        //for (i = 0; i <= loopTo; i++)
        //{
        //    drf = dtf.Rows[i];
        //    // ID des Datensatzes ermittel
        //    id = Conversions.ToInteger(drf["Format_Id"]);
        //    dr = dt.Select("Format_Id=" + id)[0];
        //    var temp1 = new Dictionary<string, string>();
        //    foreach (KeyValuePair<string, string> n in Passungstabellen.Definitionen.FORMATATTR)
        //    {
        //        try
        //        {
        //            attrname = n.Key;
        //            temp1[n.Key] = Conversions.ToString(dr[n.Key]);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log.WriteInfo("Fehler beim Lesen des Attributes '" & attrname & "' im Abschnitt 'Format'" & Chr(10) & "Makro 'Passungstabelle' abgebrochen", False)
        //            this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Fehler_beim_Lesen_des_Attributes + "'" + attrname + Passungstabellen.My.Resources.Resources.im_Abschnitt_Format + '\n' + Passungstabellen.My.Resources.Resources.Makro_Passungstabelle_abgebrochen, "", false);
        //            // Attr_get_formate = Nothing
        //            // Exit Function
        //            dr[n.Key] = Passungstabellen.Definitionen.FORMATATTR_Init[attrname];
        //            temp1[n.Key] = Passungstabellen.Definitionen.FORMATATTR_Init[attrname];
        //            SaveNeeded = true;
        //        }
        //    }
        //    dr.AcceptChanges();
        //    temp[Conversions.ToString(drf["Formatname"])] = temp1;
        //    if (SaveNeeded)
        //        this.SaveSetup();

        //Attr_get_formateRet = temp;
        //    return Attr_get_formateRet;
    }

    // Function   GetAppPath
    // Paramter:  keine
    // Ergebnis:  liefert den Pfad der Applikation
    // Liest die Tabelleneinstellungen ein
    public Dictionary<string, TableSettings> Attr_get_Tabelle()
    {
        throw new NotImplementedException();
        //Dictionary<string, TableSettings> Attr_get_TabelleRet = default;
        //var temp = new Dictionary<string, TableSettings>();
        //DataTable dt;
        //DataRow dr;
        //DataTable dtf;
        //DataRow drf;
        //DataTable dtt;
        //DataRow drt;
        //int i;
        //int id;
        //string attrname = "";
        //bool SaveNeeded = false;

        //dt = this.XMLDaten.Tables["TabellenAttribute"]!;
        //dtf = this.XMLDaten.Tables["Format"]!;
        //dtt = this.XMLDaten.Tables["Tabelle"]!;

        //// Formate durchlaufen
        //var loopTo = dtf.Rows.Count - 1;
        //for (i = 0; i <= loopTo; i++)
        //{
        //    drf = dtf.Rows[i];
        //    // ID des Datensatzes ermittel
        //    id = Conversions.ToInteger(drf["Format_Id"]);
        //    // Datensatz des Knotens Tabelle ermitteln
        //    drt = dtt.Select("Format_Id=" + id)[0];
        //    // ID des Datensatzes ermitteln
        //    id = Conversions.ToInteger(drt["Tabelle_Id"]);
        //    // Tabellenattribute auswählen
        //    dr = dt.Select("Tabelle_Id=" + id)[0];
        //    var temp1 = new Dictionary<string, string>();

        //    foreach (KeyValuePair<string, string> n in Passungstabellen.Definitionen.TABELLENATTR)
        //    {
        //        try
        //        {
        //            attrname = n.Key;
        //            temp1[n.Key] = Conversions.ToString(dr[n.Key]);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log.WriteInfo("Fehler beim Lesen des Attributes '" & attrname & "' im Abschnitt 'Tabelle'" & Chr(10) & "Vorgabewert wird gesetzt", False)
        //            this.Log.WriteInfo(Passungstabellen.My.Resources.Resources.Fehler_beim_Lesen_des_Attributes + "'" + attrname + Passungstabellen.My.Resources.Resources.im_Abschnitt_Tabelle + '\n' + Passungstabellen.My.Resources.Resources.Vorgabewert_wird_gesetzt, "", false);
        //            // Attr_get_Tabelle = Nothing
        //            // Exit Function
        //            dr[n.Key] = Passungstabellen.Definitionen.TABELLENATTR_Init[attrname];
        //            temp1[attrname] = Passungstabellen.Definitionen.TABELLENATTR_Init[attrname];
        //            SaveNeeded = true;
        //        }
        //    }
        //    dr.AcceptChanges();
        //    temp[Conversions.ToString(drf["Formatname"])] = temp1;
        //}
        //if (SaveNeeded)
        //    this.SaveSetup();
        //Attr_get_TabelleRet = temp;
        //return Attr_get_TabelleRet;
    }

    // Function   GetAppPath
    // Paramter:  keine
    // Ergebnis:  liefert den Pfad der Applikation
    public string GetAppPath()
    {
        return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    }

    // Function   GetSetupPath
    // Paramter:  keine
    // Ergebnis:  liefert den Pfad der Setup-Datei
    public string GetSetupPath()
    {
        string path = Registry.LocalMachine.OpenSubKey("Software\\nahe")?.GetValue("SetupPfad", "") as string ?? string.Empty;

        if (path == string.Empty)
        {
            path = this.GetAppPath();
        }

        return path;
    }
    // Function   GetLogPath
    // Paramter:  keine
    // Ergebnis:  liefert den Pfad der Log-Datei
    public string GetLogPath()
    {
        string path;

        // path = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
        // GetLogPath = path

        path = GetFolderPath(SpecialFolder.CommonApplicationData);

        path = path + @"\" + Application.CompanyName;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = path + @"\" + Application.ProductName;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }
}