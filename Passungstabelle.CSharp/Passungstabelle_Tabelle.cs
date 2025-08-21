using Microsoft.VisualBasic;
using Passungstabelle.CSharp;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using My = Passungstabelle.CSharp.My;


public class Passungstabelle_Tabelle
{
    private readonly GeneralSettings Attr_generell;

    public Dictionary<string, Dictionary<string, string>> Attr_Übersetzungen { get; set; } = new Dictionary<string, Dictionary<string, string>>();
    public TableSettings Attr_Tabelle { get; set; } = new();
    public string Attr_Sprache { get; set; }
    public double[] Einfügepunkt { get; set; }
    public int Einfügepunktposition { get; set; }

    public List<Passungstabelle_Zeile> TabellenZeilen { get; set; } = new List<Passungstabelle_Zeile>();
    public IEnumerable<Passungstabelle_Zeile> TabellenZeilengefiltert { get; set; }
    public int Tabellenzeilencount { get; set; }
    public int TabellenSpaltenCount { get; set; }

    public TextFormat HeadStyle { get; set; }
    public TextFormat RowStyle { get; set; }

    public LogFile Log { get; set; }

    public List<List<string>> HoletableTags { get; set; } = new List<List<string>>();

    public Sheet Blatt { get; set; }

    private Dictionary<string, int> SpaltenBreite = new Dictionary<string, int>() { { "Spalte1", 0 }, { "Spalte2", 0 }, { "Spalte3", 0 }, { "Spalte4", 0 }, { "Spalte5", 0 }, { "Spalte6", 0 }, { "Spalte7", 0 }, { "Spalte8", 0 }, { "Spalte9", 0 }, { "Spalte10", 0 } };
    private int RundenAuf;
    private double SchichtStärke;
    private DrawingDoc swdraw;
    private double fac;

    private string HeadColor;
    private string RowColor;

    // Dim Log As LogFile = Nothing

    public Passungstabelle_Tabelle()
    {
        Tabellenzeilencount = 0;
        TabellenSpaltenCount = 0;
    }

    // Tabelle initialisieren
    public Passungstabelle_Tabelle(GeneralSettings iAttr_generell, TableSettings iAttr_Tabelle, Dictionary<string, Dictionary<string, string>> iAttr_Übersetzungen, Sheet swSheet)
    {
        this.Attr_generell = iAttr_generell;
        Attr_Übersetzungen = iAttr_Übersetzungen;
        this.Attr_Tabelle = iAttr_Tabelle;
        Attr_Sprache = Attr_Tabelle.HeaderLanguage;
        RundenAuf = Attr_generell.RundenAuf;
        SchichtStärke = Attr_generell.SchichtStärke;
        fac = 1000.0;
        Log = new LogFile(Attr_generell);
        Blatt = swSheet;
    }

    public string GetZoneFromDisplayDimension(IDisplayDimension dispdim, View swView, Sheet swsheet)
    {
        double[] dimPosition;
        Annotation swAnnotation;

        swAnnotation = (Annotation)dispdim.GetAnnotation();
        dimPosition = (double[])swAnnotation.GetPosition();
        if (swView.Sheet == null)
            return Blatt.GetDrawingZone(dimPosition[0], dimPosition[1]);
        else
            return swView.Sheet.GetDrawingZone(dimPosition[0], dimPosition[1]);
    }

    public bool GetViewDimension(View swView)
    {
        IDisplayDimension[] dimensions = swView.GetDisplayDimensions().AsArrayOfType<IDisplayDimension>();

        foreach (var dispdim in dimensions)
        {
            // Keine Freistehenden Bemaßungen und Bemaßungen bei denen der Bemaßungswert 0 ist
            // Bemaßungswert 0 kommt bei abgelösten Zeichnungen vor
            if (dispdim.GetDimension2(0) == null)
            {
                Log.WriteInfo(My.Resources._keine_Bemaßung_gefunden, "", true);
                continue;
            }

            IAnnotation? annotation = dispdim.GetAnnotation().As<IAnnotation>();
            IDimension? dimension = dispdim.GetDimension2(0);
            string prefix = string.Empty;
            string zone = string.Empty;

            if (annotation is null || dimension is null)
            {
                // Reading error.
                continue;
            }

            if (annotation.IsDangling() == true)
            {
                Log.WriteInfo(My.Resources._ist_eine_freistehende_Bemaßung, " " + My.Resources.Bemaßung + ": " + dispdim.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (dispdim.GetDimension2(0).SystemValue * fac).ToString() + Strings.Chr(9), true);
                continue;
            }

            if (dispdim.GetDimension2(0).Value == 0)
            {
                Log.WriteInfo(My.Resources._hat_den_Wert_0, " " + My.Resources.Bemaßung + ": " + dispdim.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (dispdim.GetDimension2(0).SystemValue * fac).ToString() + Strings.Chr(9), true);
                continue;
            }

            if (annotation.Visible == (int)swAnnotationVisibilityState_e.swAnnotationHalfHidden ||
                annotation.Visible == (int)swAnnotationVisibilityState_e.swAnnotationVisible)
            {
                // Wenn es sich um einen Durchmsser handelt dann wird dem Maß ein Ø Symbol vorangestellt
                // If dispdim.Type2 = swDimensionType_e.swDiameterDimension Or dispdim.GetText(swDimensionTextParts_e.swDimensionTextPrefix) = "<MOD-DIAM>" Or InStr(dispdim.GetText(swDimensionTextParts_e.swDimensionTextPrefix), "<MOD-DIAM>") <> 0 Then
                if (CheckForDiameter(dispdim) == true)
                    prefix = "Ø";
                else
                    prefix = "";

                // Test für Zone ****************
                zone = GetZoneFromDisplayDimension(dispdim, swView, Blatt);
                // Test für Zone ****************

                // Passung und Toleranzen ermitteln
                Gettolfromdim(dimension, prefix, zone);

                // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
                var holeVariables = dispdim.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

                // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden

                Gettolfromcalloutvar(prefix, holeVariables, dimension, zone);
            }
        }

        return true;
    }



    public bool CheckForDiameter(IDisplayDimension dispdim)
    {
        string temp = "";

        if (dispdim.Type2 == (int)swDimensionType_e.swDiameterDimension)
        {
            return true;
        }

        temp = dispdim.GetText((int)swDimensionTextParts_e.swDimensionTextPrefix);
        // ToDo: WTF?
        temp = Strings.StrReverse(temp);
        temp = temp.Trim();

        if (Strings.Left(temp, 1) == "Ø")
        {
            return true;
        }

        // komisch das StrReverse die Zeichen "<>" umdreht
        // --> heißt im Original "<MOD-DIAM>" wenn ich das mit StrReverse umdrehe, dann kein wunder??? 
        if (Strings.Left(temp, 10).ToUpper() == ">MAID-DOM<")
        {
            return true;
        }

        return false;
    }

    // ermittelt die Passung und Toleranzen aus dem Dimension-Objekt
    private bool Gettolfromdim(IDimension dimen, string prefix, string zone)
    {
        Passungstabelle_Zeile temp = new Passungstabelle_Zeile();
        Passungstabelle_Zeile temp1 = new Passungstabelle_Zeile();
        List<Passungstabelle_Zeile> tempz = new List<Passungstabelle_Zeile>();
        DimensionTolerance tol;
        bool flag; // Marker um zu erkennen ob Passung manuell eingetragen wurde

        // Toleranz holen
        tol = dimen.Tolerance;
        flag = false;

        // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
        if (tol.Type != (int)swTolType_e.swTolFIT &&
            tol.Type != (int)swTolType_e.swTolFITTOLONLY &&
            tol.Type != (int)swTolType_e.swTolFITWITHTOL)
        {
            return false;
        }

        // Umrechnungsfaktor ermitteln
        // normalerweise gibt SWX die Werte in Meter zurück
        // fac = GetDimFactor(dimension)

        // Prüfung ob auch Passungswerte eingetragen sind
        // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
        // Wenn kein Passungswert gefunden wird, dann Abbruch der Funktion
        // If Not CheckForFitValues(tol.GetHoleFitValue, tol.GetShaftFitValue, "Bemaßung: " & dimension.FullName & " Maß: " & dimension.GetSystemValue2("") * fac) Then
        if (!CheckForFitValues(tol.GetHoleFitValue(), tol.GetShaftFitValue(), My.Resources.Bemaßung + ": " + dimen.FullName + " " + My.Resources.Maß + ": " + dimen.SystemValue * fac))
        {
            return false;
        }

        // Toleranzen von Bohrungspassung
        if (tol.GetHoleFitValue() != "" & tol.GetShaftFitValue() == "")
        {
            temp = SetColumnsFromDim(dimen, true, zone);
            temp.Prefix = prefix;
            // wenn die Passungswerte nicht gewählt wurden sondern manuel eingetragen wurden
            if (tol.GetMinValue() == 0.0 & tol.GetMaxValue() == 0.0)
            {
                // tempz = Gettolfromfit(dimension)
                CheckForFitToleranceValues(temp);
                temp = null/* TODO Change to default(_) if this is not a reference type */;
            }
            else
            {
                tempz.Add(temp);
                temp1 = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
        else if (tol.GetShaftFitValue() != "" & tol.GetHoleFitValue() == "")
        {
            temp = SetColumnsFromDim(dimen, false, zone);
            temp.Prefix = prefix;

            if (tol.GetMinValue() == 0.0 & tol.GetMaxValue() == 0.0)
            {
                // tempz = Gettolfromfit(dimension)
                CheckForFitToleranceValues(temp);
                temp = null/* TODO Change to default(_) if this is not a reference type */;
            }
            else
            {
                tempz.Add(temp);
                temp1 = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
        else if (tol.GetHoleFitValue() != "" & tol.GetShaftFitValue() != "")
        {
            temp = SetColumnsFromDim(dimen, true, zone);
            temp.Prefix = prefix;

            // * Bohungswerte ermitteln
            // tol.SetFitValues(temp.Passung, ""]
            if (temp.ToleranzO == 0.0 & temp.ToleranzU == 0.0)
                flag = true;

            // * wellenwerte ermitteln
            temp1 = SetColumnsFromDim(dimen, false, zone);
            temp1.Prefix = prefix;
            // tol.SetFitValues("", temp1.Passung)
            if (temp.ToleranzO == 0.0 & temp.ToleranzU == 0.0)
                flag = true;

            // * Alten Wert wieder setzen
            tol.SetFitValues(temp.Passung, temp1.Passung);

            if (flag == true)
            {
                if (CheckForFitToleranceValues(temp))
                    tempz.Add(temp);
                if (CheckForFitToleranceValues(temp1))
                    tempz.Add(temp1);
            }
            else
            {
                tempz.Add(temp);
                tempz.Add(temp1);
            }
        }

        foreach (var temp2 in tempz)
        {
            if (temp2 != null)
            {
                TabellenZeilen.Add(temp);
                Log.WriteInfo(My.Resources.Bemaßung + ": " + temp.Name + " " + My.Resources.Maß + ": " + temp.Maß + Strings.Chr(9) + temp.Passung, "", false);
                temp = null/* TODO Change to default(_) if this is not a reference type */;
                Tabellenzeilencount = Tabellenzeilencount + 1;
            }
        }
        return true;
    }


    // ermittelt die Toleranzen, wenn die Passungswerte nicht gewählt wurden 
    // sondern manuel eingetragen wurden
    private List<Passungstabelle_Zeile>? GettolfromfitCallOut(ICalloutVariable swCalloutVariable, ICalloutLengthVariable swCalloutLengthVariable, IDimension dimen, string zone)
    {
        Passungstabelle_Zeile temp = new Passungstabelle_Zeile();
        Passungstabelle_Zeile temp1 = new Passungstabelle_Zeile();
        List<Passungstabelle_Zeile> tempz = new List<Passungstabelle_Zeile>();

        temp.Maß = swCalloutLengthVariable.Length * fac;
        temp1.Maß = swCalloutLengthVariable.Length * fac;
        temp.Passung = swCalloutVariable.HoleFit;
        temp.Name = dimen.FullName;
        temp1.Passung = swCalloutVariable.ShaftFit;
        temp1.Name = dimen.FullName;
        temp.Zone = zone;
        temp1.Zone = zone;

        // Prüfung ob auch Passungswerte eingetragen sind
        // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
        if (!CheckForFitValues(swCalloutVariable.HoleFit, swCalloutVariable.ShaftFit, My.Resources.Bemaßung + ": " + swCalloutVariable.VariableName + " " + My.Resources.Maß + ": " + swCalloutLengthVariable.Length * fac))
        {
            return null;
        }

        // Toleranzen von Bohrungspassung
        if (temp.Passung != "" & temp1.Passung == "")
        {
            swCalloutVariable.HoleFit = temp.Passung;
            temp.ToleranzU = swCalloutVariable.ToleranceMin * fac;
            temp.ToleranzO = swCalloutVariable.ToleranceMax * fac;
        }
        else if (temp1.Passung != "" & temp.Passung == "")
        {
            swCalloutVariable.ShaftFit = temp1.Passung;
            temp1.ToleranzU = swCalloutVariable.ToleranceMin * fac;
            temp1.ToleranzO = swCalloutVariable.ToleranceMax * fac;
        }
        else if (temp.Passung != "" & temp1.Passung != "")
        {
            // * Bohungswerte ermitteln
            swCalloutVariable.HoleFit = temp.Passung;
            temp.ToleranzU = swCalloutVariable.ToleranceMin * fac;
            temp.ToleranzO = swCalloutVariable.ToleranceMax * fac;
            // * wellenwerte ermitteln
            swCalloutVariable.ShaftFit = temp1.Passung;
            temp1.ToleranzU = swCalloutVariable.ToleranceMin * fac;
            temp1.ToleranzO = swCalloutVariable.ToleranceMax * fac;

            // * Alten Wert wieder setzen
            swCalloutVariable.HoleFit = temp.Passung;
            swCalloutVariable.ShaftFit = temp1.Passung;
        }
        if (temp is not null)
        {
            if (CheckForFitToleranceValues(temp))
            {
                tempz.Add(temp);
                Log.WriteInfo(My.Resources.Bemaßung + ": " + temp.Name + " " + My.Resources.Maß + ": " + temp.Maß.ToString() + Strings.Chr(9) + temp.Passung, "", false);
            }
        }
        if (temp1 is not null)
        {
            if (CheckForFitToleranceValues(temp1))
            {
                tempz.Add(temp1);
                Log.WriteInfo(My.Resources.Bemaßung + ": " + temp1.Name + " " + My.Resources.Maß + ": " + temp1.Maß.ToString() + Strings.Chr(9) + temp1.Passung, "", false);
            }
        }

        return tempz;
    }

    // ermittelt die Passung und Toleranzen aus einer Bohrungsbeschreibung
    private bool Gettolfromcalloutvar(string prefix, ICalloutVariable[] calloutvar, IDimension dimen, string zone)
    {
        Passungstabelle_Zeile temp = new Passungstabelle_Zeile();
        Passungstabelle_Zeile temp1 = new Passungstabelle_Zeile();
        List<Passungstabelle_Zeile> tempz = new List<Passungstabelle_Zeile>();
        bool flag; // Marker um zu erkennen ob Passung manuell eingetragen wurde
        int i;

        temp.Zone = zone;
        temp1.Zone = zone;

        foreach (ICalloutVariable swCalloutVariable in calloutvar)
        {
            if (swCalloutVariable.Type != (int)swCalloutVariableType_e.swCalloutVariableType_Length ||
                swCalloutVariable is not CalloutLengthVariable swCalloutLengthVariable)
            {
                continue;
            }

            flag = false;

            // MsgBox(dimension.Value & " / " & tol.Type & "/ " & swTolType_e.swTolFIT)

            // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
            if (swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFIT &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITTOLONLY &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITWITHTOL)
            {
                continue;
            }

            // Prüfung ob auch Passungswerte eingetragen sind
            // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
            // Wenn kein Passungswert gefunden wird, dann Abbruch der Funktion
            if (!CheckForFitValues(swCalloutVariable.HoleFit, swCalloutVariable.ShaftFit, My.Resources.Bemaßung + ": " + swCalloutVariable.VariableName + " " + My.Resources.Maß + ": " + swCalloutLengthVariable.Length * fac))
            {
                return false;
            }

            // Toleranzen von Bohrungspassung
            if (swCalloutVariable.HoleFit != "" & swCalloutVariable.ShaftFit == "")
            {
                temp = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, true, zone);
                temp.Prefix = prefix;
                temp.Type = ZeilenTyp.Hole;
                // wenn die Passungswerte nicht gewählt wurden sondern manuel eingetragen wurden
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, dimen, zone);
                else
                {
                    tempz.Add(temp);
                    temp1 = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            else if (swCalloutVariable.ShaftFit != "" & swCalloutVariable.HoleFit == "")
            {
                temp = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, false, zone);
                temp.Prefix = prefix;
                temp.Type = ZeilenTyp.Shaft;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, dimen, zone);
                else
                {
                    tempz.Add(temp);
                    temp1 = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            else if (swCalloutVariable.HoleFit != "" & swCalloutVariable.ShaftFit != "")
            {
                // * Bohungswerte ermitteln
                temp = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, true, zone);
                temp.Prefix = prefix;
                temp.Type = ZeilenTyp.Hole;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    flag = true;

                // * wellenwerte ermitteln
                temp1 = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, false, zone);
                temp1.Prefix = prefix;
                temp.Type = ZeilenTyp.Shaft;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    flag = true;

                // * Alten Wert wieder setzen
                swCalloutVariable.HoleFit = temp.Passung;
                swCalloutVariable.ShaftFit = temp1.Passung;

                if (flag == true)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, dimen, zone);
                else
                {
                    tempz.Add(temp);
                    tempz.Add(temp1);
                }
            }

            foreach (var temp2 in tempz)
            {
                if (temp2 != null)
                {
                    // 8.2.0.3 Änderung wegen falscher Passungen
                    if (CheckForFitToleranceValues(temp))
                    {
                        TabellenZeilen.Add(temp);
                        Log.WriteInfo(My.Resources.Bohrungsbeschreibung, "", false);
                        Log.WriteInfo(My.Resources.Bemaßung + ": " + temp.Name + " " + My.Resources.Maß + ": " + temp.Maß.ToString() + Strings.Chr(9) + temp.Passung, "", false);
                        temp = null/* TODO Change to default(_) if this is not a reference type */;
                        Tabellenzeilencount = Tabellenzeilencount + 1;
                    }
                }
            }

        }
        return true;
    }

    // Prüfung ob auch Passungswerte eingetragen sind
    // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
    public bool CheckForFitValues(string HoleFitStr, string ShaftFitStr, string MaszStr)
    {
        if (ShaftFitStr == "" & HoleFitStr == "")
        {
            // If (Attr_generell("ReaktionAufLeerePassung") = True) And (Attr_generell("Fehlermeldung") = True) Then
            if (Attr_generell.ReaktionAufLeerePassung == false)
                Log.WriteInfo(My.Resources._Keine_Passung_für, " " + MaszStr + " " + My.Resources.eingetragen, true);
            return false;
        }

        return true;
    }
    // Prüfung ob auch Toleranzwertwerte für die Passung gefunden wurden
    // z.B.: Passung M3 ist nur bis zu einer Größe von max. 50mm definiert
    public bool CheckForFitToleranceValues(Passungstabelle_Zeile temp)
    {
        LogFile log = new LogFile(Attr_generell);

        if (temp.Passung != "" & temp.ToleranzO == 0.0 & temp.ToleranzU == 0.0)
        {
            log.WriteInfo(My.Resources._Keine_Passungswerte_für, " " + temp.Maß + "/" + temp.Passung + " " + My.Resources.gefunden, true);
            // Update 8.2.03 Hinweis auf falsche Passungen
            if (temp.Passung[0] >= 'A' & temp.Passung[0] <= 'Z' & temp.Type == ZeilenTyp.Shaft)
                // log.WriteInfo("Passung " & temp.Passung & " passt nicht zu Wellenpassung", True)
                log.WriteInfo(My.Resources._passt_nicht_zu_Wellenpassung, " " + My.Resources.Passung + " " + temp.Passung, true);
            else if (temp.Passung[0] >= 'a' & temp.Passung[0] <= 'Z' & temp.Type == ZeilenTyp.Hole)
                // log.WriteInfo("Passung " & temp.Passung & " passt nicht zu Bohrungspassung", True)
                log.WriteInfo(My.Resources._passt_nicht_zu_Bohrungspassung, " " + My.Resources.Passung + " " + temp.Passung, true);
            return false;
        }
        else if (temp.Passung[0] >= 'A' & temp.Passung[0] <= 'Z' & temp.Type == ZeilenTyp.Shaft)
        {
            // log.WriteInfo("Passung " & temp.Passung & " passt nicht zu Wellenpassung", True)
            log.WriteInfo(My.Resources._passt_nicht_zu_Wellenpassung, " " + My.Resources.Passung + " " + temp.Passung, true);
            return false;
        }
        else if (temp.Passung[0] >= 'a' & temp.Passung[0] <= 'z' & temp.Type == ZeilenTyp.Hole)
        {
            // log.WriteInfo("Passung " & temp.Passung & " passt nicht zu Bohrungspassung", True)
            log.WriteInfo(My.Resources._passt_nicht_zu_Bohrungspassung, " " + My.Resources.Passung + " " + temp.Passung, true);
            return false;
        }
        return true;
    }
    // setzt die Zeileneinträge für diese Maß/Passungskombination
    public Passungstabelle_Zeile SetColumnsFromDim(IDimension dimen, bool Hole, string zone)
    {
        Passungstabelle_Zeile temp = new Passungstabelle_Zeile();
        Passungstabelle_Zeile temp1 = new Passungstabelle_Zeile();
        List<Passungstabelle_Zeile> tempz = new List<Passungstabelle_Zeile>();
        DimensionTolerance tol;
        bool flag;
        string Shaftvalue = "";
        string HoleValue = "";

        tol = dimen.Tolerance;
        flag = false;

        HoleValue = tol.GetHoleFitValue();
        Shaftvalue = tol.GetShaftFitValue();

        if (Hole)
        {
            tol.SetFitValues(tol.GetHoleFitValue(), "");
            temp.Type = ZeilenTyp.Hole;
        }
        else
        {
            tol.SetFitValues("", tol.GetShaftFitValue());
            temp.Type = ZeilenTyp.Shaft;
        }
        temp.Zone = zone;
        temp.Maß = Math.Round((dimen.GetSystemValue2("") * fac), RundenAuf);
        if (Attr_generell.PlusZeichen & tol.GetMaxValue() > 0)
            temp.ToleranzO = tol.GetMaxValue() * fac;
        else
            temp.ToleranzO = tol.GetMaxValue() * fac;
        if (Attr_generell.PlusZeichen & tol.GetMinValue() > 0)
            temp.ToleranzU = tol.GetMinValue() * fac;
        else
            temp.ToleranzU = tol.GetMinValue() * fac;

        temp.AbmaßO = Convert.ToDouble(temp.Maß) + Convert.ToDouble(temp.ToleranzO);
        temp.AbmaßU = Convert.ToDouble(temp.Maß) + Convert.ToDouble(temp.ToleranzU);
        temp.AbmaßToleranzMitte = Convert.ToDouble(temp.AbmaßU) + (Convert.ToDouble(temp.AbmaßO) - Convert.ToDouble(temp.AbmaßU)) / 2.0;

        // Wenn die Bohrungspassung benötigt wird
        if (Hole)
        {
            temp.Passung = tol.GetHoleFitValue();
            temp.VorbearbeitungAbmaßO = Convert.ToDouble(temp.AbmaßO) + SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = Convert.ToDouble(temp.AbmaßU) + SchichtStärke * 2;
        }
        else
        {
            temp.Passung = tol.GetShaftFitValue();
            temp.VorbearbeitungAbmaßO = Convert.ToDouble(temp.AbmaßO) - SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = Convert.ToDouble(temp.AbmaßU) - SchichtStärke * 2;
        }

        temp.VorbearbeitungAbmaßToleranzMitte = Convert.ToDouble(temp.VorbearbeitungAbmaßU) + (Convert.ToDouble(temp.VorbearbeitungAbmaßO) - Convert.ToDouble(temp.VorbearbeitungAbmaßU)) / 2.0;

        temp.MaßPassung = temp.Prefix + temp.Maß + " " + temp.Passung;
        temp.Name = dimen.FullName;

        tol.SetFitValues(HoleValue, Shaftvalue);
        return temp;
    }

    // setzt die Zeileneinträge für diese Maß/Passungskombination
    public Passungstabelle_Zeile SetColumnsFromCallOut(IDimension dimen, ICalloutVariable swCalloutVariable, ICalloutLengthVariable swCalloutLengthVariable, bool Hole, string zone)
    {
        Passungstabelle_Zeile temp = new Passungstabelle_Zeile();
        DimensionTolerance tol;
        bool flag;
        string Shaftvalue = "";
        string HoleValue = "";
        int tempfittyp = 0;
        int temptyp = 0;
        double maxtol = 0.0;
        double mintol = 0.0;
        string fittext = "";

        tol = dimen.Tolerance;
        flag = false;

        HoleValue = swCalloutVariable.HoleFit;
        Shaftvalue = swCalloutVariable.ShaftFit;

        if (HoleValue != "" & Shaftvalue != "")
        {
            tempfittyp = tol.FitType;
            temptyp = tol.Type;
            tol.FitType = (int)swFitType_e.swFitUSER;
            tol.Type = (int)swTolType_e.swTolFIT;
            if (Hole)
            {
                tol.SetFitValues(HoleValue, "");
                fittext = HoleValue;
            }
            else
            {
                tol.SetFitValues("", Shaftvalue);
                fittext = Shaftvalue;
            }
            maxtol = tol.GetMaxValue();
            mintol = tol.GetMinValue();
        }
        else
        {
            maxtol = swCalloutVariable.ToleranceMax;
            mintol = swCalloutVariable.ToleranceMin;
            if (Hole)
                fittext = swCalloutVariable.HoleFit;
            else
                fittext = swCalloutVariable.ShaftFit;
        }

        temp.Maß = Math.Round(swCalloutLengthVariable.Length * fac, RundenAuf);
        temp.Zone = zone;

        if (Attr_generell.PlusZeichen & maxtol > 0)
            temp.ToleranzO = maxtol * fac;
        else
            temp.ToleranzO = maxtol * fac;
        if (Attr_generell.PlusZeichen & mintol > 0)
            temp.ToleranzU = mintol * fac;
        else
            temp.ToleranzU = mintol * fac;

        temp.AbmaßO = Convert.ToDouble(temp.Maß) + Convert.ToDouble(temp.ToleranzO);
        temp.AbmaßU = Convert.ToDouble(temp.Maß) + Convert.ToDouble(temp.ToleranzU);
        temp.AbmaßToleranzMitte = Convert.ToDouble(temp.AbmaßU) + (Convert.ToDouble(temp.AbmaßO) - Convert.ToDouble(temp.AbmaßU)) / 2.0;

        // Wenn die Bohrungspassung benötigt wird
        if (Hole)
        {
            temp.Passung = fittext;
            temp.VorbearbeitungAbmaßO = Convert.ToDouble(temp.AbmaßO) + SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = Convert.ToDouble(temp.AbmaßU) + SchichtStärke * 2;
        }
        else
        {
            temp.Passung = fittext;
            temp.VorbearbeitungAbmaßO = Convert.ToDouble(temp.AbmaßO) - SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = Convert.ToDouble(temp.AbmaßU) - SchichtStärke * 2;
        }

        temp.VorbearbeitungAbmaßToleranzMitte = Convert.ToDouble(temp.VorbearbeitungAbmaßU) + (Convert.ToDouble(temp.VorbearbeitungAbmaßO) - Convert.ToDouble(temp.VorbearbeitungAbmaßU)) / 2.0;

        temp.MaßPassung = temp.Prefix + temp.Maß + " " + temp.Passung;
        temp.Name = swCalloutVariable.VariableName;

        if (HoleValue != "" & Shaftvalue != "")
        {
            tol.SetFitValues(HoleValue, Shaftvalue);
            tol.FitType = tempfittyp;
            tol.Type = temptyp;
        }


        return temp;
    }

    public bool GetHoleTableDimension(List<HoleTable> HoleTabs, View swview, List<Dictionary<string, List<string>>> Zonen)
    {
        Dimension dimen;
        string prefix = "";
        string zone = "";
        int z;

        int k = 0;
        foreach (var holetab in HoleTabs)
        {
            var tabs = holetab.GetTableAnnotations().AsArrayOfType<IHoleTableAnnotation>();
            // tabs = HoleTabs.GetTableAnnotations

            foreach (var holeTable in tabs)
            {
                var feat = holeTable.HoleTable.GetFeature();
                var dispdim = feat.GetFirstDisplayDimension().As<DisplayDimension>();
                z = 0;

                while (dispdim is not null)
                {
                    if (dispdim.Type2 == (int)swDimensionType_e.swDiameterDimension)
                    {
                        prefix = "Ø";
                    }

                    dimen = dispdim.GetDimension2(0);
                    // zone = GetZoneFromDisplayDimension(dispdim, swview)
                    zone = string.Join("/", [Zonen[k], HoleTabs[k].HoleTag[z + 1]]);
                    // zone = ""
                    Gettolfromdim(dimen, prefix, zone);

                    // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
                    var holeVariables = dispdim.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();
                    // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden
                    if (holeVariables.Any())
                    {
                        Gettolfromcalloutvar(prefix, holeVariables, dimen, zone);
                    }
                    dispdim = feat.GetNextDisplayDimension(dispdim).As<DisplayDimension>();
                    z = z + 1;
                }
            }
            k++;
        }

        return true;
    }
    private void SetColors()
    {
        RowColor = ConvertColorToSwxHex(Attr_Tabelle.FarbeZeile);
        HeadColor = ConvertColorToSwxHex(Attr_Tabelle.FarbeKopfZeile);
    }

    private string ConvertColorToSwxHex(long colorcode)
    {
        string temps1;
        string temps2;

        temps1 = Strings.Right("000000" + Conversion.Hex(colorcode), 6);

        temps2 = "";
        temps2 = Strings.Mid(temps1, Strings.Len(temps1) - 1, 1) + Strings.Right(temps1, 1);

        temps1 = Strings.Left(temps1, 4);
        temps2 = temps2 + Strings.Mid(temps1, Strings.Len(temps1) - 1, 1) + Strings.Right(temps1, 1);

        temps1 = Strings.Left(temps1, 2);
        temps2 = temps2 + Strings.Mid(temps1, Strings.Len(temps1) - 1, 1) + Strings.Right(temps1, 1);

        return "0x" + temps2;
    }

    public int GetLineWidth1(string widthName, ModelDoc2 modeldoc)
    {
        var lineWidth = widthName switch
        {
            "Dünn" => swLineWeights_e.swLW_THIN,
            "Normal" => swLineWeights_e.swLW_NORMAL,
            "Dick" => swLineWeights_e.swLW_THICK,
            "Dick(2)" => swLineWeights_e.swLW_THICK2,
            "Dick(3)" => swLineWeights_e.swLW_THICK3,
            "Dick(4)" => swLineWeights_e.swLW_THICK4,
            "Dick(5)" => swLineWeights_e.swLW_THICK5,
            "Dick(6)" => swLineWeights_e.swLW_THICK6,
            _ => swLineWeights_e.swLW_THIN,
        };
        return (int)lineWidth;
    }

    public double GetTableWidth(TableAnnotation swtable)
    {
        double temp = 0;

        for (var i = 0; i <= swtable.ColumnCount - 1; i++)
            temp = temp + swtable.GetColumnWidth(i);

        return temp;
    }

    public double GetTableHeigth(TableAnnotation swtable)
    {
        double temp = 0;

        for (var i = 0; i <= swtable.RowCount - 1; i++)
            temp = temp + swtable.GetRowHeight(i);

        return temp;
    }

    public void SetEinfügepunktSWX2019(TableAnnotation swtable)
    {
        double b = GetTableWidth(swtable);
        double h = GetTableHeigth(swtable);
        double[] temp = new double[2];

        if (Attr_generell.NeuPositionieren == true)
        {
            if (Einfügepunktposition == (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomLeft)
            {
                Einfügepunkt[0] = Einfügepunkt[0];
                Einfügepunkt[1] = Einfügepunkt[1] + h;
            }
            else if (Einfügepunktposition == (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight)
            {
                Einfügepunkt[0] = Einfügepunkt[0] - b;
                Einfügepunkt[1] = Einfügepunkt[1] + h;
            }
            else if (Einfügepunktposition == (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight)
            {
                Einfügepunkt[0] = Einfügepunkt[0] - b;
                Einfügepunkt[1] = Einfügepunkt[1];
            }
        }
    }

    public void InsertTable(DrawingDoc swdraw, Sheet swsheet)
    {
        TableAnnotation swTable;
        ModelDoc2 modeldoc = (ModelDoc2)swdraw;

        swdraw.ActivateSheet(swsheet.GetName());

        modeldoc.Extension.SelectByID2("PASSUNGSTABELLE@" + swsheet.GetName, "ANNOTATIONTABLES", 0, 0, 0, false, 0, null/* TODO Change to default(_) if this is not a reference type */, 0);
        modeldoc.EditDelete();

        // Funktioniert nicht mit SWX 2019 Rasterlinien werden nicht angezeigt
        // swTable = swdraw.InsertTableAnnotation2(False, Einfügepunkt(0), Einfügepunkt(1), Einfügepunktposition, "", Tabellenzeilencount * 2 + 1, TabellenSpaltenCount)

        // Rasterlinien funktionieren, Position stimmt nicht deshalb Verwendung von SetEinfügepunktSWX2019
        swTable = modeldoc.Extension.InsertGeneralTableAnnotation(false, Einfügepunkt[0], Einfügepunkt[1], Einfügepunktposition, "", Tabellenzeilencount * 2 + 1, TabellenSpaltenCount);

        // swTable.GetAnnotation.Visible = False
        swTable.GetAnnotation().Visible = (int)swAnnotationVisibilityState_e.swAnnotationHidden;

        for (var i = 0; i <= swTable.ColumnCount - 1; i++)
            swTable.SetColumnWidth(i, 1.0, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);

        swTable.GetAnnotation().SetName("PASSUNGSTABELLE");
        swTable.Title = "Passungstabelle";
        swTable.GeneralTableFeature.GetFeature().Name = "Passungstabelle-" + swsheet.GetName();

        swTable.BorderLineWeight = (int)Attr_Tabelle.RahmenStrichStärke;
        swTable.GridLineWeight = (int)Attr_Tabelle.RasterStrichStärke;


        SetColors();

        HeadStyle = GetTextStyle(true, swTable);
        RowStyle = GetTextStyle(false, swTable);

        SetTabelHeader(swTable);

        swTable.SetTextFormat(false, RowStyle);

        SetColumnHeader(swTable);

        InsertRowsText(swTable);


        if (Attr_Tabelle.SpaltenBreiteAutomatisch == true)
        {
            SetColumnWithAuto(swTable);
            SetColumnHeightAuto(swTable);
            MergeCells(swTable);
        }
        else
        {
            SetColumnWithValue(swTable);
            SetColumnHeightAuto(swTable);
            MergeCells(swTable);
        }

        SetEinfügepunktSWX2019(swTable);
        swTable.GetAnnotation().SetPosition2(Einfügepunkt[0], Einfügepunkt[1], 0);

        // swTable.GetAnnotation.Visible = True
        swTable.GetAnnotation().Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
    }

    private TextFormat GetTextStyle(bool Header, TableAnnotation swTable)
    {
        TextFormat temp;

        temp = swTable.GetTextFormat();

        if (Header == true)
        {
            temp.TypeFaceName = Attr_Tabelle.SchriftartKopfZeile;
            temp.CharHeight = Attr_Tabelle.TexthöheKopfZeile / 1000.0;
            temp.Bold = Attr_Tabelle.FettKopfZeile;
            temp.Underline = Attr_Tabelle.UnterstrichenKopfZeile;
            temp.Strikeout = Attr_Tabelle.DurchgestrichenKopfZeile;
            temp.Italic = Attr_Tabelle.KursivKopfZeile;
        }
        else
        {
            temp.TypeFaceName = Attr_Tabelle.SchriftartZeile;
            temp.CharHeight = Attr_Tabelle.TexthöheZeile / 1000.0;
            temp.Bold = Attr_Tabelle.FettZeile;
            temp.Underline = Attr_Tabelle.UnterstrichenZeile;
            temp.Strikeout = Attr_Tabelle.DurchgestrichenZeile;
            temp.Italic = Attr_Tabelle.KursivZeile;
        }

        return temp;
    }

    private void SetTabelHeader(TableAnnotation swTable)
    {
        int rows = 1;
        // Wenn zweisprachig dann auch zwei Zeilen
        // If Attr_Sprache.Contains("/") Then rows = 2 Else rows = 1

        if (Attr_Tabelle.HeaderOben == true)
            swTable.SetHeader((int)swTableHeaderPosition_e.swTableHeader_Top, rows);
        else
            swTable.SetHeader((int)swTableHeaderPosition_e.swTableHeader_Bottom, rows);
    }

    private void SetColumnHeader(TableAnnotation swTable)
    {
        string lang1 = "";
        string lang2 = "";

        Dictionary<string, string> lang1l = new Dictionary<string, string>();
        Dictionary<string, string> lang2l = new Dictionary<string, string>();


        if (Attr_Sprache.Contains("/"))
        {
            lang1 = Attr_Sprache.Substring(0, 2);
            lang2 = Attr_Sprache.Substring(3, 2);
        }
        else
            lang1 = Attr_Sprache.Substring(0, 2);

        if (Attr_Sprache.Contains("/"))
        {
            lang1l = Attr_Übersetzungen[lang1];
            lang2l = Attr_Übersetzungen[lang2];
        }
        else
            lang1l = Attr_Übersetzungen[lang1];

        InsertHeaderText(swTable, lang1l, lang2l);
    }

    private void InsertHeaderText(TableAnnotation swTable, Dictionary<string, string> lang1l, Dictionary<string, string> lang2l)
    {
        int pos = 0;
        Annotation ann;
        int rowpos = 0;

        if (swTable.GetHeaderStyle() == (int)swTableHeaderPosition_e.swTableHeader_Top)
            rowpos = 0;
        else
            rowpos = swTable.RowCount - 1;

        foreach (KeyValuePair<string, string> n in Definitionen.TABELLENATTR_Init)
        {
            if (n.Key.Length > 10)
            {
                if (n.Key.Substring(0, 9) == "TabSpalte")
                {
                    swTable.SetColumnTitle(pos, "<FONT color=" + HeadColor + ">" + lang1l[n.Key.Substring(9)]);
                    swTable.SetCellTextFormat(rowpos, pos, false, HeadStyle);
                    if (lang2l.Count > 0)
                    {
                        // swTable.SetColumnTitle(pos, swTable.GetColumnTitle2(pos, True) & Chr(13) & lang2l(n.Key.Substring(9)))
                        swTable.SetColumnTitle(pos, swTable.GetColumnTitle(pos) + Strings.Chr(13) + lang2l[n.Key.Substring(9)]);
                        swTable.SetCellTextFormat(rowpos, pos, false, HeadStyle);
                    }
                    pos = pos + 1;
                }
            }
        }
        ann = swTable.GetAnnotation();
    }

    private void InsertRowsText(TableAnnotation swTable)
    {
        int rowpos;
        int rowstep;

        if (swTable.GetHeaderStyle() == (int)swTableHeaderPosition_e.swTableHeader_Top)
        {
            rowpos = 1;
            rowstep = 2;
        }
        else
        {
            rowpos = swTable.RowCount - 2;
            rowstep = -2;
        }
        foreach (var row in TabellenZeilengefiltert)
        {
            InsertRowText(swTable, rowpos, rowstep, row);
            rowpos = rowpos + rowstep;
        }
    }

    private void MergeCells(TableAnnotation swTable)
    {
        int rowpos;
        int rowstep;

        if (swTable.GetHeaderStyle() == (int)swTableHeaderPosition_e.swTableHeader_Top)
        {
            rowpos = 1;
            rowstep = 2;
        }
        else
        {
            rowpos = swTable.RowCount - 2;
            rowstep = -2;
        }
        foreach (var row in TabellenZeilengefiltert)
        {
            MergeCell(swTable, rowpos, rowstep, row);
            rowpos = rowpos + rowstep;
        }
    }

    private void InsertRowText(TableAnnotation swTable, int rowpos, int rowstep, Passungstabelle_Zeile row)
    {
        int pos = 0;
        int rstep;

        if (rowstep < 0)
            rstep = -1;
        else
            rstep = 1;

        if (swTable.GetHeaderStyle() == (int)swTableHeaderPosition_e.swTableHeader_Bottom)
        {
            rowpos = rowpos - 1;
            rstep = 1;
        }
        else
        {
        }

        foreach (var spalte in Attr_Tabelle.Spalten)
        {
            if (!spalte.Visible)
            {
                continue;
            }


            switch (spalte.Name)
            {
                case "Maß":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.Prefix + row.Maß;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "Passung":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.Passung;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "MaßePassung":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.Prefix + row.Maß + " " + row.Passung;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "Toleranz":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.ToleranzO;
                    swTable.Text[rowpos + rstep, pos] = "<FONT color=" + RowColor + ">" + row.ToleranzU;
                    break;

                case "Abmaß":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.AbmaßO;
                    swTable.Text[rowpos + rstep, pos] = "<FONT color=" + RowColor + ">" + row.AbmaßU;
                    break;

                case "AbmaßToleranzMitte":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.AbmaßToleranzMitte;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "VorbearbeitungsAbmaße":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.VorbearbeitungAbmaßO;
                    swTable.Text[rowpos + rstep, pos] = "<FONT color=" + RowColor + ">" + row.VorbearbeitungAbmaßU;
                    break;

                case "VorbearbeitungsToleranzMitte":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.VorbearbeitungAbmaßToleranzMitte;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "Anzahl":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.Anzahl;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

                case "Zone":
                    swTable.Text[rowpos, pos] = "<FONT color=" + RowColor + ">" + row.Zone;
                    swTable.Text[rowpos + rstep, pos] = "-";
                    break;

            }
            pos = pos + 1;
        }
    }

    private void MergeCell(TableAnnotation swTable, int rowpos, int rowstep, Passungstabelle_Zeile row)
    {
        int pos = 0;
        int rstep = rowstep < 0 ? -1 : 1;

        foreach (var spalte in Attr_Tabelle.Spalten)
        {
            if (spalte.Visible != true)
            {
                continue;
            }

            if (spalte.MergeCells)
            {
                swTable.MergeCells(rowpos, pos, rowpos + rstep, pos);
            }

            pos = pos + 1;
        }
    }

    // Setzt die Spaltenbreiten an Hand der Setup Einstellungen
    // Achtung: Die Reihenfolge der Spaltennamen muss mit der Reihenfolge der Spaltenbreiten übereinstimmen
    public void SetColumnWithValue(TableAnnotation swTable)
    {
        int i = 0;

        foreach (var spalte in Attr_Tabelle.Spalten)
        {
            if (spalte.Visible != true)
            {
                continue;
            }

            swTable.SetColumnWidth(
                i,
                spalte.Breite / 1000,
                (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);

            i = i + 1;
        }
    }

    // Setzt die Spaltenbreiten automatisch an Hand des breitesten Texts der jeweiligen Spalte
    public void SetColumnWithAuto(TableAnnotation swTable)
    {
        int index = 0;
        Annotation swAnnotation;
        DisplayData swDisplayData;
        double TextWidth = 0.0;
        bool HeaderZweizeilig = false;
        double temp = 0.0;

        swAnnotation = swTable.GetAnnotation();

        swDisplayData = swAnnotation.GetDisplayData().As<DisplayData>()!;

        if (Attr_Sprache.Contains("/"))
            HeaderZweizeilig = true;

        // For i = 0 To swTable.ColumnCount - 1
        // swTable.SetColumnWidth(i, 1.0, swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange)
        // Next

        for (var i = 0; i <= swTable.ColumnCount - 1; i++)
        {
            for (var j = 0; j <= swTable.RowCount - 1; j++)
            {
                if (j == 0 & HeaderZweizeilig)
                {
                    if (swDisplayData.GetTextInBoxWidthAtIndex(index) > TextWidth)
                        temp = swDisplayData.GetTextInBoxWidthAtIndex(index);
                    if (swDisplayData.GetTextInBoxWidthAtIndex(index + 1) > temp)
                        temp = swDisplayData.GetTextInBoxWidthAtIndex(index + 1);
                    index = index + swTable.ColumnCount * 2 - i;
                }
                else
                {
                    if (swDisplayData.GetTextInBoxWidthAtIndex(index) > TextWidth)
                        temp = swDisplayData.GetTextInBoxWidthAtIndex(index);
                    index = index + swTable.ColumnCount;
                }
                if (temp > TextWidth)
                    TextWidth = temp;
            }
            swTable.SetColumnWidth(i, TextWidth + 0.001, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
            TextWidth = 0.0;
            temp = 0.0;
            if (HeaderZweizeilig)
                index = i * 2 + 2;
            else
                index = i + 1;
        }
        swAnnotation.Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
    }

    public void SetColumnHeightAuto(TableAnnotation swTable)
    {
        int index = 0;
        Annotation swAnnotation;
        DisplayData swDislplayData;
        double TextWidth = 0.0;
        bool HeaderZweizeilig = false;
        double temp = 0.0;
        double höheR = Attr_Tabelle.TexthöheZeile * 1.5 / 1000.0;
        double höheK = 0.0;

        if (HeaderZweizeilig)
            höheK = Attr_Tabelle.TexthöheKopfZeile * 1.25 / 1000.0;
        else
            höheK = Attr_Tabelle.TexthöheKopfZeile * 1.5 / 1000.0;

        swAnnotation = swTable.GetAnnotation();

        swDislplayData = swAnnotation.GetDisplayData().As<DisplayData>()!;

        if (Attr_Sprache.Contains("/"))
            HeaderZweizeilig = true;

        for (var i = 0; i <= swTable.RowCount - 1; i++)
        {
            // For j = 0 To swTable.ColumnCount - 1
            // temp = swDisplayData.GetTextInBoxHeightAtIndex(index)
            // index = index + 1
            // If temp > TextWidth Then TextWidth = temp
            // Next
            if (i == 0 & HeaderZweizeilig)
                swTable.SetRowHeight(i, höheK * 2, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
            else
                swTable.SetRowHeight(i, höheR, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
            swTable.SetRowVerticalGap(i, höheR / 10.0);
            TextWidth = 0.0;
            temp = 0.0;
        }
        swAnnotation.Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
    }

    // ** Umrechungsfaktor von SWX Einheiten zu mm bzw.Grad
    private double GetDimFactor(Dimension swDim)
    {
        const double PI = 3.14159265;
        const double LEN_FACTOR = 1000.0;
        const double ANG_FACTOR = 180.0 / PI;

        return (swDimensionParamType_e)swDim.GetType() switch
        {
            swDimensionParamType_e.swDimensionParamTypeDoubleLinear => LEN_FACTOR,
            swDimensionParamType_e.swDimensionParamTypeDoubleAngular => ANG_FACTOR,
            _ => 0,
        };
    }

    private int Count_passungen(DrawingDoc swdrw)
    {
        int zaehler = 0;
        return zaehler;
    }

    // Filtert die Tabellzeilen ohne Duplikate
    public void SetTabellenzeilenGefiltert()
    {
        // Sortiert die Einträge
        TabellenZeilen.Sort();
        // Anzahl der Passungen speichern
        SetTabellenzeilenCountDouble();
        // Entfernt doppelte Einträge
        TabellenZeilengefiltert = TabellenZeilen.Distinct();
        // Setzt den Zeilenzähler neu
        Tabellenzeilencount = TabellenZeilengefiltert.Count();
    }

    public void SetTabellenzeilenCountDouble()
    {
        int zähler = 1;
        Dictionary<int, int> anzahl = new Dictionary<int, int>();
        int j;
        int i = 0;

        while (i <= TabellenZeilen.Count - 1)
        {
            j = i;
            zähler = 1;
            if (j < TabellenZeilen.Count - 1)
            {
                while (TabellenZeilen[j].MaßPassung == TabellenZeilen[j + 1].MaßPassung)
                {
                    zähler = zähler + 1;
                    j = j + 1;
                    // *******
                    if (j >= TabellenZeilen.Count - 1)
                        break;
                }
            }
            anzahl.Add(i, zähler);
            i = j + 1;
        }
        foreach (var n in anzahl)
            TabellenZeilen[n.Key].Anzahl = n.Value;
    }
}
