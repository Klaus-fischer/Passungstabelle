// <copyright file="SheetAnalyser" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;


using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

internal class SheetAnalyser(IDrawingDoc drawing, ISheet sheet)
{
    private readonly IDrawingDoc drawing = drawing;
    private readonly ISheet sheet = sheet;

    public IEnumerable<PassungEntity> GetPassungsEntities()
    {
        this.drawing.ActivateSheet(sheet.GetName());

        return drawing.GetAllViews().SelectMany(this.GetPassungsEntities);
    }

    private IEnumerable<PassungEntity> GetPassungsEntities(IView view)
    {
        return this.GetPassungenFromDimensions(view)
            .Union(this.GetPassungenFromHoleTables(view));
    }


    private IEnumerable<PassungEntity> GetPassungenFromDimensions(IView view)
    {
        List<PassungEntity> passungen = new List<PassungEntity>();
        IDisplayDimension[] dimensions = view.GetDisplayDimensions().AsArrayOfType<IDisplayDimension>();

        foreach (var displayDimension in dimensions)
        {
            // Keine Freistehenden Bemaßungen und Bemaßungen bei denen der Bemaßungswert 0 ist
            // Bemaßungswert 0 kommt bei abgelösten Zeichnungen vor
            if (displayDimension.GetDimension2(0) == null)
            {
                continue;
            }

            IAnnotation? annotation = displayDimension.GetAnnotation().As<IAnnotation>();
            IDimension? dimension = displayDimension.GetDimension2(0);
            string prefix = string.Empty;
            string zone = string.Empty;

            if (annotation is null || dimension is null)
            {
                // Reading error.
                continue;
            }

            if (annotation.IsDangling() == true)
            {
                //Log.WriteInfo(My.Resources._ist_eine_freistehende_Bemaßung, " " + My.Resources.Bemaßung + ": " + displayDimension.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (displayDimension.GetDimension2(0).SystemValue * factor).ToString() + Strings.Chr(9), true);
                continue;
            }

            if (displayDimension.GetDimension2(0).Value == 0)
            {
                //Log.WriteInfo(My.Resources._hat_den_Wert_0, " " + My.Resources.Bemaßung + ": " + displayDimension.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (displayDimension.GetDimension2(0).SystemValue * factor).ToString() + Strings.Chr(9), true);
                continue;
            }

            if (annotation.Visible == (int)swAnnotationVisibilityState_e.swAnnotationHalfHidden ||
                annotation.Visible == (int)swAnnotationVisibilityState_e.swAnnotationVisible)
            {
                // Wenn es sich um einen Durchmsser handelt dann wird dem Maß ein Ø Symbol vorangestellt
                // If displayDimension.Type2 = swDimensionType_e.swDiameterDimension Or displayDimension.GetText(swDimensionTextParts_e.swDimensionTextPrefix) = "<MOD-DIAM>" Or InStr(displayDimension.GetText(swDimensionTextParts_e.swDimensionTextPrefix), "<MOD-DIAM>") <> 0 Then
                if (displayDimension.CheckForDiameter())
                    prefix = "Ø";
                else
                    prefix = "";

                zone = displayDimension.GetZoneFromDisplayDimension(view, this.sheet);

                // Passung und Toleranzen ermitteln
                passungen.AddRange(GetTolerancesFromDimension(dimension, prefix, zone));

                // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
                var calloutVariables = displayDimension.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

                // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden
                passungen.AddRange(calloutVariables.GetTolerances(prefix, zone));
            }
        }

        return passungen;
    }

    private IEnumerable<PassungEntity> GetTolerancesFromDimension(IDimension dimension, string prefix, string zone)
    {
        // Toleranz holen
        var tolerance = dimension.Tolerance;

        // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
        if (tolerance.Type != (int)swTolType_e.swTolFIT &&
            tolerance.Type != (int)swTolType_e.swTolFITTOLONLY &&
            tolerance.Type != (int)swTolType_e.swTolFITWITHTOL)
        {
            yield break;
        }

        var holeFit = tolerance.GetHoleFitValue();
        var shaftFit = tolerance.GetShaftFitValue();

        // Toleranzen von Bohrungspassung
        if (!string.IsNullOrEmpty(holeFit))
        {
            yield return dimension.GetPassung(true, prefix, zone);
        }

        if (!string.IsNullOrEmpty(shaftFit))
        {
            yield return dimension.GetPassung(false, prefix, zone);
        }

    }

    private IEnumerable<PassungEntity> GetPassungenFromHoleTables(IView view)
    {
        var analyzer = new HoleTableAnalyzer(view);
        return analyzer.GetPassungen();
    }

}
