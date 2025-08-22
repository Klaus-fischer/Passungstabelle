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
    public TableSettings Settings { get; } = new();

    public double[] EinfügePosition { get; set; }

    public Einfügepunkt Einfügepunkt { get; set; }

    public List<TabellenZeile> TabellenZeilen { get; set; } = new List<TabellenZeile>();
    public IEnumerable<TabellenZeile> TabellenZeilengefiltert { get; set; }
    public int Tabellenzeilencount { get; set; }
    public int TabellenSpaltenCount { get; set; }

    public TextFormat HeadStyle { get; set; }
    public TextFormat RowStyle { get; set; }

    public LogFile Log { get; set; }

    public List<List<string>> HoletableTags { get; set; } = new List<List<string>>();

    public Sheet Blatt { get; set; }
    public bool ReaktionAufLeerePassung { get; private set; }
    public bool PlusZeichen { get; private set; }

    private int RundenAuf { get; }
    private double SchichtStärke { get; }
    private DrawingDoc swdraw { get; }
    private double fac { get; }

    private string HeadColor { get; }
    private string RowColor { get; }

    // Dim Log As LogFile = Nothing

    public Passungstabelle_Tabelle()
    {
        Tabellenzeilencount = 0;
        TabellenSpaltenCount = 0;
    }

    /// <summary>
    /// Tabelle initialisieren
    /// </summary>
    /// <param name="iAttr_generell"></param>
    /// <param name="iAttr_Tabelle"></param>
    /// <param name="iAttr_Übersetzungen"></param>
    /// <param name="swSheet"></param>
    public Passungstabelle_Tabelle(GeneralSettings iAttr_generell, TableSettings iAttr_Tabelle, Dictionary<string, Dictionary<string, string>> iAttr_Übersetzungen, Sheet swSheet)
        : this()
    {
        this.Settings = iAttr_Tabelle;
        this.RundenAuf = iAttr_generell.RundenAuf;
        this.SchichtStärke = iAttr_generell.SchichtStärke;
        this.ReaktionAufLeerePassung = iAttr_generell.ReaktionAufLeerePassung;
        this.PlusZeichen = iAttr_generell.PlusZeichen;
        this.fac = 1000.0;
        this.Log = new LogFile(iAttr_generell);
        this.Blatt = swSheet;
    }

    private string GetZoneFromDisplayDimension(IDisplayDimension dispdim, View swView, Sheet swsheet)
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

    private bool CheckForDiameter(IDisplayDimension dispdim)
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
    private bool Gettolfromdim(IDimension dimension, string prefix, string zone)
    {
        TabellenZeile temp;
        TabellenZeile temp1;
        List<TabellenZeile> tempz = new List<TabellenZeile>();
        DimensionTolerance tol;
        bool flag; // Marker um zu erkennen ob Passung manuell eingetragen wurde

        // Toleranz holen
        tol = dimension.Tolerance;
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
        if (!CheckForFitValues(tol.GetHoleFitValue(), tol.GetShaftFitValue(), dimension.FullName, dimension.SystemValue * fac))
        {
            return false;
        }

        // Toleranzen von Bohrungspassung
        if (tol.GetHoleFitValue() != "" & tol.GetShaftFitValue() == "")
        {
            temp = SetColumnsFromDim(dimension, true, zone);
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
            temp = SetColumnsFromDim(dimension, false, zone);
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
            temp = SetColumnsFromDim(dimension, true, zone);
            temp.Prefix = prefix;

            // * Bohungswerte ermitteln
            // tol.SetFitValues(holePassung.Passung, ""]
            if (temp.ToleranzO == 0.0 & temp.ToleranzU == 0.0)
                flag = true;

            // * wellenwerte ermitteln
            temp1 = SetColumnsFromDim(dimension, false, zone);
            temp1.Prefix = prefix;
            // tol.SetFitValues("", shaftPassung.Passung)
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
                TabellenZeilen.Add(temp2);
                Log.WriteInfo(My.Resources.Bemaßung + ": " + temp2.Name + " " + My.Resources.Maß + ": " + temp2.Maß + Strings.Chr(9) + temp2.Passung, "", false);
                temp = null/* TODO Change to default(_) if this is not a reference type */;
                Tabellenzeilencount = Tabellenzeilencount + 1;
            }
        }
        return true;
    }

    /// <summary>
    /// ermittelt die Toleranzen, wenn die Passungswerte nicht gewählt wurden 
    /// sondern Manuel eingetragen wurden.
    /// </summary>
    /// <param varName="swCalloutVariable"></param>
    /// <param varName="swCalloutLengthVariable"></param>
    /// <param varName="dimension"></param>
    /// <param varName="zone"></param>
    /// <returns></returns>
    private List<TabellenZeile>? GettolfromfitCallOut(ICalloutVariable swCalloutVariable, ICalloutLengthVariable swCalloutLengthVariable, string zone)
    {
        var holeFit = swCalloutVariable.HoleFit;
        var shaftFit = swCalloutVariable.ShaftFit;
        var value = swCalloutLengthVariable.Length;
        var varName = swCalloutVariable.VariableName;

        // Prüfung ob auch Passungswerte eingetragen sind
        // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
        if (!CheckForFitValues(holeFit, shaftFit, varName, value))
        {
            return [];
        }

        TabellenZeile? holePassung = this.GetPassung(swCalloutVariable, value, true);
        TabellenZeile? shaftPassung = this.GetPassung(swCalloutVariable, value, false);

        List<TabellenZeile> tempz = new List<TabellenZeile>();

        if (holePassung is not null && CheckForFitToleranceValues(holePassung))
        {
            holePassung.Name = varName;
            holePassung.Zone = zone;
            tempz.Add(holePassung);
        }
        if (shaftPassung is not null && CheckForFitToleranceValues(shaftPassung))
        {
            shaftPassung.Name = varName;
            shaftPassung.Zone = zone;
            tempz.Add(shaftPassung);
        }

        return tempz;
    }

    private TabellenZeile? GetPassung(ICalloutVariable swCalloutVariable, double maß, bool isHole)
    {
        var holeFit = swCalloutVariable.HoleFit;
        var shaftFit = swCalloutVariable.ShaftFit;

        var passung = isHole ? holeFit : shaftFit;

        if (string.IsNullOrEmpty(passung))
        {
            return null;
        }

        swCalloutVariable.ShaftFit = isHole ? string.Empty : shaftFit;
        swCalloutVariable.HoleFit = isHole ? holeFit : string.Empty;

        var result = new TabellenZeile(
            maß,
            passung,
            swCalloutVariable.ToleranceMax,
            swCalloutVariable.ToleranceMin,
            isHole,
            this.SchichtStärke);

        swCalloutVariable.HoleFit = holeFit;
        swCalloutVariable.ShaftFit = shaftFit;

        return result;
    }

    /// <summary>
    /// ermittelt die Passung und Toleranzen aus einer Bohrungsbeschreibung
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="calloutvar"></param>
    /// <param name="dimen"></param>
    /// <param name="zone"></param>
    /// <returns></returns>
    private bool Gettolfromcalloutvar(string prefix, ICalloutVariable[] calloutvar, IDimension dimen, string zone)
    {
        List<TabellenZeile> tempz = new List<TabellenZeile>();


        foreach (ICalloutVariable swCalloutVariable in calloutvar)
        {
            var flag = false;
            if (swCalloutVariable.Type != (int)swCalloutVariableType_e.swCalloutVariableType_Length ||
                swCalloutVariable is not CalloutLengthVariable swCalloutLengthVariable)
            {
                continue;
            }

            // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
            if (swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFIT &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITTOLONLY &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITWITHTOL)
            {
                continue;
            }

            var value = swCalloutLengthVariable.Length * fac;

            // Prüfung ob auch Passungswerte eingetragen sind
            // Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde
            // Wenn kein Passungswert gefunden wird, dann Abbruch der Funktion
            if (!CheckForFitValues(swCalloutVariable.HoleFit, swCalloutVariable.ShaftFit, swCalloutVariable.VariableName, value))
            {
                continue;
            }

            TabellenZeile? temp;
            TabellenZeile? temp1;

            // Toleranzen von Bohrungspassung
            if (swCalloutVariable.HoleFit != "" & swCalloutVariable.ShaftFit == "")
            {
                temp = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, true, zone);
                temp.Prefix = prefix;
                temp.Type = PassungsType.Hole;
                // wenn die Passungswerte nicht gewählt wurden sondern manuel eingetragen wurden
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, zone);
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
                temp.Type = PassungsType.Shaft;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, zone);
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
                temp.Type = PassungsType.Hole;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    flag = true;

                // * wellenwerte ermitteln
                temp1 = SetColumnsFromCallOut(dimen, swCalloutVariable, swCalloutLengthVariable, false, zone);
                temp1.Prefix = prefix;
                temp.Type = PassungsType.Shaft;
                if (swCalloutVariable.ToleranceMin == 0.0 & swCalloutVariable.ToleranceMax == 0.0)
                    flag = true;

                // * Alten Wert wieder setzen
                swCalloutVariable.HoleFit = temp.Passung;
                swCalloutVariable.ShaftFit = temp1.Passung;

                if (flag == true)
                    tempz = GettolfromfitCallOut(swCalloutVariable, swCalloutLengthVariable, zone);
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

    /// <summary>
    /// Prüfung ob auch Passungswerte eingetragen sind
    /// Könnte ja auch sein, dass als Toleranzttyp Passung eingestellt ist und keine Passung gewählt wurde.
    /// </summary>
    /// <param varName="holeFit"></param>
    /// <param varName="shaftFit"></param>
    /// <param varName="MaszStr"></param>
    /// <returns></returns>
    private bool CheckForFitValues(string holeFit, string shaftFit, string varName, double value)
    {
        if (shaftFit == "" & holeFit == "")
        {
            // If (Attr_generell("ReaktionAufLeerePassung") = True) And (Attr_generell("Fehlermeldung") = True) Then
            if (this.ReaktionAufLeerePassung == false)
                Log.WriteInfo(My.Resources.KeinePassungTemplate, varName, value);
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Prüfung ob auch Toleranzwertwerte für die Passung gefunden wurden
    /// z.B.: Passung M3 ist nur bis zu einer Größe von max. 50mm definiert
    /// </summary>
    /// <param varName="temp"></param>
    /// <returns></returns>
    private bool CheckForFitToleranceValues(TabellenZeile temp)
    {
        if (temp.Passung != "" & temp.ToleranzO == 0.0 & temp.ToleranzU == 0.0)
        {
            Log.WriteInfo(My.Resources.LeerePassungsWerte, temp.Maß, temp.Passung);

            if (temp.Passung[0] >= 'A' & temp.Passung[0] <= 'Z' & temp.Type == PassungsType.Shaft)
            {
                Log.WriteInfo(My.Resources.UngültigeWellenpassung, temp.Passung);
            }
            else if (temp.Passung[0] >= 'a' & temp.Passung[0] <= 'Z' & temp.Type == PassungsType.Hole)
            {
                Log.WriteInfo(My.Resources.UngültigeBohrungspassung, temp.Passung);
            }

            return false;
        }
        else if (temp.Passung[0] >= 'A' & temp.Passung[0] <= 'Z' & temp.Type == PassungsType.Shaft)
        {
            Log.WriteInfo(My.Resources.UngültigeWellenpassung, temp.Passung);
            return false;
        }
        else if (temp.Passung[0] >= 'a' & temp.Passung[0] <= 'z' & temp.Type == PassungsType.Hole)
        {
            Log.WriteInfo(My.Resources.UngültigeBohrungspassung, temp.Passung);
            return false;
        }

        return true;
    }

    /// <summary>
    /// setzt die Zeileneinträge für diese Maß/Passungskombination
    /// </summary>
    /// <param varName="dimension"></param>
    /// <param varName="isHole"></param>
    /// <param varName="zone"></param>
    /// <returns></returns>
    private TabellenZeile SetColumnsFromDim(IDimension dimen, bool Hole, string zone)
    {
        TabellenZeile temp = new Passungstabelle_Zeile();

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
            temp.Type = PassungsType.Hole;
        }
        else
        {
            tol.SetFitValues("", tol.GetShaftFitValue());
            temp.Type = PassungsType.Shaft;
        }
        temp.Zone = zone;
        temp.Maß = Math.Round((dimen.GetSystemValue2("") * fac), RundenAuf);
        if (this.PlusZeichen & tol.GetMaxValue() > 0)
            temp.ToleranzO = tol.GetMaxValue() * fac;
        else
            temp.ToleranzO = tol.GetMaxValue() * fac;
        if (this.PlusZeichen & tol.GetMinValue() > 0)
            temp.ToleranzU = tol.GetMinValue() * fac;
        else
            temp.ToleranzU = tol.GetMinValue() * fac;

        temp.AbmaßO = temp.Maß + temp.ToleranzO;
        temp.AbmaßU = temp.Maß + temp.ToleranzU;
        temp.AbmaßToleranzMitte = temp.AbmaßU + (temp.AbmaßO - temp.AbmaßU) / 2.0;

        // Wenn die Bohrungspassung benötigt wird
        if (Hole)
        {
            temp.Passung = tol.GetHoleFitValue();
            temp.VorbearbeitungAbmaßO = temp.AbmaßO + SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = temp.AbmaßU + SchichtStärke * 2;
        }
        else
        {
            temp.Passung = tol.GetShaftFitValue();
            temp.VorbearbeitungAbmaßO = temp.AbmaßO - SchichtStärke * 2;
            temp.VorbearbeitungAbmaßU = temp.AbmaßU - SchichtStärke * 2;
        }

        temp.VorbearbeitungAbmaßToleranzMitte = temp.VorbearbeitungAbmaßU + (temp.VorbearbeitungAbmaßO - temp.VorbearbeitungAbmaßU) / 2.0;

        temp.MaßPassung = temp.Prefix + temp.Maß + " " + temp.Passung;
        temp.Name = dimen.FullName;

        tol.SetFitValues(HoleValue, Shaftvalue);
        return temp;
    }

    // setzt die Zeileneinträge für diese Maß/Passungskombination
    public TabellenZeile SetColumnsFromCallOut(IDimension dimension, ICalloutVariable swCalloutVariable, ICalloutLengthVariable swCalloutLengthVariable, bool isHole, string zone)
    {
        var maß = swCalloutLengthVariable.Length * fac;
        double maxTolerance;
        double minTolerance;

        var holeFit = swCalloutVariable.HoleFit;
        var shaftValue = swCalloutVariable.ShaftFit;

        string passung = isHole ? holeFit : shaftValue;

        if (holeFit != "" & shaftValue != "")
        {
            var tol = dimension.Tolerance;
            var tempFitTyp = tol.FitType;
            var tempTyp = tol.Type;
            tol.FitType = (int)swFitType_e.swFitUSER;
            tol.Type = (int)swTolType_e.swTolFIT;
            if (isHole)
            {
                tol.SetFitValues(holeFit, "");
            }
            else
            {
                tol.SetFitValues("", shaftValue);
            }

            maxTolerance = tol.GetMaxValue() * fac;
            minTolerance = tol.GetMinValue() * fac;

            tol.SetFitValues(holeFit, shaftValue);
            tol.FitType = tempFitTyp;
            tol.Type = tempTyp;
        }
        else
        {
            maxTolerance = swCalloutVariable.ToleranceMax * fac;
            minTolerance = swCalloutVariable.ToleranceMin * fac;
        }

        return new TabellenZeile(maß, passung, maxTolerance, minTolerance, isHole, this.SchichtStärke);
    }

    private bool GetHoleTableDimension(List<HoleTable> HoleTabs, View swview, List<Dictionary<string, List<string>>> Zonen)
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
        RowColor = ConvertColorToSwxHex(Settings.FarbeZeile);
        HeadColor = ConvertColorToSwxHex(Settings.FarbeKopfZeile);
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

    private int GetLineWidth1(string widthName, ModelDoc2 modeldoc)
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

    private double GetTableWidth(TableAnnotation swtable)
    {
        double temp = 0;

        for (var i = 0; i <= swtable.ColumnCount - 1; i++)
            temp = temp + swtable.GetColumnWidth(i);

        return temp;
    }

    private double GetTableHeigth(TableAnnotation swtable)
    {
        double temp = 0;

        for (var i = 0; i <= swtable.RowCount - 1; i++)
            temp = temp + swtable.GetRowHeight(i);

        return temp;
    }

    private void SetEinfügepunktSWX2019(TableAnnotation swtable)
    {
        double b = GetTableWidth(swtable);
        double h = GetTableHeigth(swtable);
        double[] temp = new double[2];

        if (Attr_generell.NeuPositionieren == true)
        {
            if (Einfügepunkt == Einfügepunkt.BottomLeft)
            {
                EinfügePosition[0] = EinfügePosition[0];
                EinfügePosition[1] = EinfügePosition[1] + h;
            }
            else if (Einfügepunkt == Einfügepunkt.BottomRight)
            {
                EinfügePosition[0] = EinfügePosition[0] - b;
                EinfügePosition[1] = EinfügePosition[1] + h;
            }
            else if (Einfügepunkt == Einfügepunkt.TopRight)
            {
                EinfügePosition[0] = EinfügePosition[0] - b;
                EinfügePosition[1] = EinfügePosition[1];
            }
        }
    }

    private void InsertTable(DrawingDoc swdraw, Sheet swsheet)
    {
        TableAnnotation swTable;
        ModelDoc2 modeldoc = (ModelDoc2)swdraw;

        swdraw.ActivateSheet(swsheet.GetName());

        if (modeldoc.Extension.SelectByID2("PASSUNGSTABELLE@" + swsheet.GetName(), "ANNOTATIONTABLES", 0, 0, 0, false, 0, null/* TODO Change to default(_) if this is not a reference type */, 0))
        {
            modeldoc.EditDelete();
        }

        // Funktioniert nicht mit SWX 2019 Rasterlinien werden nicht angezeigt
        // swTable = swdraw.InsertTableAnnotation2(False, Einfügeposition(0), Einfügeposition(1), Einfügepunkt, "", Tabellenzeilencount * 2 + 1, TabellenSpaltenCount)

        // Rasterlinien funktionieren, Position stimmt nicht deshalb Verwendung von SetEinfügepunktSWX2019
        swTable = modeldoc.Extension.InsertGeneralTableAnnotation(false, EinfügePosition[0], EinfügePosition[1], (int)Einfügepunkt, "", Tabellenzeilencount * 2 + 1, TabellenSpaltenCount);

        // swTable.GetAnnotation.Visible = False
        swTable.GetAnnotation().Visible = (int)swAnnotationVisibilityState_e.swAnnotationHidden;

        for (var i = 0; i <= swTable.ColumnCount - 1; i++)
            swTable.SetColumnWidth(i, 1.0, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);

        swTable.GetAnnotation().SetName("PASSUNGSTABELLE");
        swTable.Title = "Passungstabelle";
        swTable.GeneralTableFeature.GetFeature().Name = "Passungstabelle-" + swsheet.GetName();

        swTable.BorderLineWeight = (int)Settings.RahmenStrichStärke;
        swTable.GridLineWeight = (int)Settings.RasterStrichStärke;


        SetColors();

        HeadStyle = GetTextStyle(true, swTable);
        RowStyle = GetTextStyle(false, swTable);

        SetTabelHeader(swTable);

        swTable.SetTextFormat(false, RowStyle);

        InsertHeaderText(swTable);

        InsertRowsText(swTable);


        if (Settings.SpaltenBreiteAutomatisch == true)
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
        swTable.GetAnnotation().SetPosition2(EinfügePosition[0], EinfügePosition[1], 0);

        // swTable.GetAnnotation.Visible = True
        swTable.GetAnnotation().Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
    }

    private TextFormat GetTextStyle(bool Header, TableAnnotation swTable)
    {
        TextFormat temp;

        temp = swTable.GetTextFormat();

        if (Header == true)
        {
            temp.TypeFaceName = Settings.SchriftartKopfZeile;
            temp.CharHeight = Settings.TexthöheKopfZeile / 1000.0;
            temp.Bold = Settings.FettKopfZeile;
            temp.Underline = Settings.UnterstrichenKopfZeile;
            temp.Strikeout = Settings.DurchgestrichenKopfZeile;
            temp.Italic = Settings.KursivKopfZeile;
        }
        else
        {
            temp.TypeFaceName = Settings.SchriftartZeile;
            temp.CharHeight = Settings.TexthöheZeile / 1000.0;
            temp.Bold = Settings.FettZeile;
            temp.Underline = Settings.UnterstrichenZeile;
            temp.Strikeout = Settings.DurchgestrichenZeile;
            temp.Italic = Settings.KursivZeile;
        }

        return temp;
    }

    private void SetTabelHeader(TableAnnotation swTable)
    {
        int rows = this.Settings.HasMultiLineHeader ? 2 : 1;

        if (Settings.HeaderOben == true)
        {
            swTable.SetHeader((int)swTableHeaderPosition_e.swTableHeader_Top, rows);
        }
        else
        {
            swTable.SetHeader((int)swTableHeaderPosition_e.swTableHeader_Bottom, rows);
        }
    }

    private void InsertHeaderText(TableAnnotation swTable)
    {
        int pos = 0;
        Annotation ann;
        int rowpos = 0;

        if (Settings.HeaderOben)
            rowpos = 0;
        else
            rowpos = swTable.RowCount - 1;

        foreach (var spalte in Settings.Spalten)
        {
            var title = string.IsNullOrWhiteSpace(spalte.SubTitle) ? spalte.Title : $"{spalte.Title}\n{spalte.SubTitle}";

            swTable.SetColumnTitle(pos, $"<FONT color={HeadColor}>{title}");
            swTable.SetCellTextFormat(rowpos, pos, false, HeadStyle);
            pos++;
        }
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

    private void InsertRowText(TableAnnotation swTable, int rowpos, int rowstep, TabellenZeile row)
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

        foreach (var spalte in Settings.Spalten)
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

    private void MergeCell(TableAnnotation swTable, int rowpos, int rowstep, TabellenZeile row)
    {
        int pos = 0;
        int rstep = rowstep < 0 ? -1 : 1;

        foreach (var spalte in Settings.Spalten)
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
    private void SetColumnWithValue(TableAnnotation swTable)
    {
        int i = 0;

        foreach (var spalte in Settings.Spalten)
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
    private void SetColumnWithAuto(TableAnnotation swTable)
    {
        int index = 0;
        Annotation swAnnotation;
        DisplayData swDisplayData;
        double TextWidth = 0.0;
        bool HeaderZweizeilig = this.Settings.Spalten.Any(o => o.Visible && !string.IsNullOrWhiteSpace(o.SubTitle));
        double temp = 0.0;

        swAnnotation = swTable.GetAnnotation();

        swDisplayData = swAnnotation.GetDisplayData().As<DisplayData>()!;

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

    private void SetColumnHeightAuto(TableAnnotation swTable)
    {
        int index = 0;
        Annotation swAnnotation;
        DisplayData swDislplayData;
        double TextWidth = 0.0;
        bool HeaderZweizeilig = false;
        double temp = 0.0;
        double höheR = Settings.TexthöheZeile * 1.5 / 1000.0;
        double höheK = 0.0;

        if (HeaderZweizeilig)
            höheK = Settings.TexthöheKopfZeile * 1.25 / 1000.0;
        else
            höheK = Settings.TexthöheKopfZeile * 1.5 / 1000.0;

        swAnnotation = swTable.GetAnnotation();

        swDislplayData = swAnnotation.GetDisplayData().As<DisplayData>()!;

        if (Attr_Sprache.Contains("/"))
            HeaderZweizeilig = true;

        for (var i = 0; i <= swTable.RowCount - 1; i++)
        {
            // For j = 0 To swTable.ColumnCount - 1
            // holePassung = swDisplayData.GetTextInBoxHeightAtIndex(index)
            // index = index + 1
            // If holePassung > TextWidth Then TextWidth = holePassung
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

    // Filtert die Tabellzeilen ohne Duplikate
    private void SetTabellenzeilenGefiltert()
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

    private void SetTabellenzeilenCountDouble()
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
