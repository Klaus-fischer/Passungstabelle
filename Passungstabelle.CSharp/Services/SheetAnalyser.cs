// <copyright file="SheetAnalyser" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;


using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Windows.Automation.Peers;

internal class SheetAnalyser(IDrawingDoc drawing, ISheet sheet)
{
    private readonly IDrawingDoc drawing = drawing;
    private readonly ISheet sheet = sheet;

    public void GetPassungsEntities(ref PassungEntityCollection collection)
    {
        this.drawing.ActivateSheet(sheet.GetName());

        foreach (var view in drawing.GetAllViews())
        {
            this.GetPassungsEntities(view, ref collection);
        }
    }

    private void GetPassungsEntities(IView view, ref PassungEntityCollection collection)
    {
        this.GetPassungenFromDimensions(view, ref collection);
        this.GetPassungenFromHoleTables(view, ref collection);
    }

    private void GetPassungenFromDimensions(IView view, ref PassungEntityCollection collection)
    {
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
                collection.AddPassungFromDimension(dimension, true, prefix, zone);
                collection.AddPassungFromDimension(dimension, false, prefix, zone);

                // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
                var calloutVariables = displayDimension.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

                // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden
                collection.AddPassungFromCallOut(calloutVariables, prefix, zone);
            }
        }
    }

    private void GetPassungenFromHoleTables(IView view, ref PassungEntityCollection collection)
    {
        ITableAnnotation[] tables = view.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();

        foreach (var table in tables)
        {
            this.GetPassungenFromTable(view, table, ref collection);
        }
    }

    private void GetPassungenFromTable(IView view, ITableAnnotation table, ref PassungEntityCollection collection)
    {
        if (table.Type != (int)swTableAnnotationType_e.swTableAnnotation_HoleChart ||
            table is not HoleTable holeTable)
        {
            return;
        }

        this.GetPassungenFromHoleTable(view, holeTable, ref collection);
    }

    private void GetPassungenFromHoleTable(IView view, IHoleTable holeTable, ref PassungEntityCollection collection)
    {
        var annotations = holeTable.GetTableAnnotations().AsArrayOfType<IHoleTableAnnotation>();

        var feat = holeTable.GetFeature();
        var displayDimension = feat.GetFirstDisplayDimension().As<DisplayDimension>();
        int index = 0;

        var zonenContainer = this.GetZonen(view, holeTable);

        int rowIndex = 1;
        while (displayDimension is not null)
        {
            string prefix = "";
            if (displayDimension.Type2 == (int)swDimensionType_e.swDiameterDimension)
            {
                prefix = "Ø";
            }

            Dimension dimension = displayDimension.GetDimension2(0);

            var zoneKey = holeTable.HoleTag[rowIndex];
            var zoneArray = zonenContainer.ContainsKey(zoneKey) ? zonenContainer[zoneKey].ToArray() : [];
            collection.AddPassungFromDimension(dimension, true, prefix, zoneArray);

            // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
            var holeVariables = displayDimension.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

            collection.AddPassungFromCallOut(holeVariables, prefix, zoneArray);

            displayDimension = feat.GetNextDisplayDimension(displayDimension).As<DisplayDimension>();
            index++;
        }
    }

    private Dictionary<string, List<string>> GetZonen(IView view, IHoleTable holeTable)
    {
        //var view = holeTable.GetFeature().GetOwnerFeature().GetSpecificFeature2().As<IView>()
        if (holeTable.As<ITableAnnotation>() is not ITableAnnotation table)
        {
            return [];
        }

        var result = new Dictionary<string, List<string>>();

        for (int rowIndex = 1; rowIndex < table.RowCount - 1; rowIndex++)
        {
            result.Add(holeTable.HoleTag[rowIndex], []);
        }

        var notes = view.GetNotes().AsArrayOfType<INote>();

        foreach (var note in notes)
        {
            if (result.TryGetValue(note.PropertyLinkedText, out var zones))
            {
                var point = note.GetTextPoint2().As<MathPoint>();
                var zone = point?.GetZone(view.Sheet);
                if (zone is not null)
                {
                    zones.Add(zone);
                }
            }
        }

        return result;
    }
}
