// <copyright file="SheetAnalyser" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;


using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

internal class SheetAnalyser(IDrawingDoc drawing, ISheet sheet)
{
    private readonly IDrawingDoc drawing = drawing;
    private readonly ISheet sheet = sheet;

    private readonly PassungEntityCollection collection = new();

    public TabellenZeile[] GetPassungsEntities()
    {
        this.collection.Clear();
        this.drawing.ActivateSheet(sheet.GetName());

        foreach (var view in drawing.GetAllViews())
        {
            this.GetPassungsEntities(view);
        }

        return this.collection.BuildTable();
    }

    private void GetPassungsEntities(IView view)
    {
        this.GetPassungenFromDimensions(view);
        this.GetPassungenFromHoleTables(view);
    }

    private void GetPassungenFromDimensions(IView view)
    {
        IDisplayDimension[] dimensions = view.GetDisplayDimensions().AsArrayOfType<IDisplayDimension>();

        foreach (var displayDimension in dimensions)
        {
            if (!IsPassung(displayDimension, out var annotation, out var dimension))
            {
                continue;
            }

            string prefix = displayDimension.CheckForDiameter() ? "Ø" : "";
            string zone = displayDimension.GetZoneFromDisplayDimension(view, this.sheet);

            // Passung und Toleranzen ermitteln
            collection.AddPassungFromDimension(dimension, true, prefix, zone);
            collection.AddPassungFromDimension(dimension, false, prefix, zone);

            // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
            var calloutVariables = displayDimension.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

            // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden
            collection.AddPassungFromCallOut(calloutVariables, prefix, zone);
        }
    }

    private bool IsPassung(IDisplayDimension displayDimension, out IAnnotation annotation, out IDimension dimension)
    {
        annotation = null!;
        dimension = null!;
        // Keine Freistehenden Bemaßungen und Bemaßungen bei denen der Bemaßungswert 0 ist
        // Bemaßungswert 0 kommt bei abgelösten Zeichnungen vor
        if (displayDimension.GetDimension2(0) == null)
        {
            return false;
        }

        annotation = displayDimension.GetAnnotation().As<IAnnotation>()!;
        dimension = displayDimension.GetDimension2(0);

        if (annotation is null || dimension is null)
        {
            // Reading error.
            return false;
        }

        if (annotation.IsDangling() == true)
        {
            //Log.WriteInfo(My.Resources._ist_eine_freistehende_Bemaßung, " " + My.Resources.Bemaßung + ": " + displayDimension.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (displayDimension.GetDimension2(0).SystemValue * factor).ToString() + Strings.Chr(9), true);
            return false;
        }

        if (displayDimension.GetDimension2(0).Value == 0)
        {
            //Log.WriteInfo(My.Resources._hat_den_Wert_0, " " + My.Resources.Bemaßung + ": " + displayDimension.GetDimension2(0).FullName + " " + My.Resources.Maß + ": " + (displayDimension.GetDimension2(0).SystemValue * factor).ToString() + Strings.Chr(9), true);
            return false;
        }

        var tolerance = dimension.Tolerance;
        // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
        if (tolerance.Type != (int)swTolType_e.swTolFIT &&
            tolerance.Type != (int)swTolType_e.swTolFITTOLONLY &&
            tolerance.Type != (int)swTolType_e.swTolFITWITHTOL)
        {
            return false;
        }

        if (annotation.Visible != (int)swAnnotationVisibilityState_e.swAnnotationHalfHidden &&
            annotation.Visible != (int)swAnnotationVisibilityState_e.swAnnotationVisible)
        {
            return false;
        }

        return true;
    }

    private void GetPassungenFromHoleTables(IView view)
    {
        ITableAnnotation[] tables = view.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();

        foreach (var table in tables)
        {
            this.GetPassungenFromTable(view, table);
        }
    }

    private void GetPassungenFromTable(IView view, ITableAnnotation table)
    {
        if (table.Type != (int)swTableAnnotationType_e.swTableAnnotation_HoleChart ||
            table is not IHoleTableAnnotation annotation ||
            annotation.HoleTable is not IHoleTable holeTable)
        {
            return;
        }

        this.GetPassungenFromHoleTable(view, holeTable);
    }

    private void GetPassungenFromHoleTable(IView view, IHoleTable holeTable)
    {
        var annotations = holeTable.GetTableAnnotations().AsArrayOfType<IHoleTableAnnotation>();

        var feat = holeTable.GetFeature();
        var displayDimension = feat.GetFirstDisplayDimension().As<DisplayDimension>();
        int index = 0;

        var zonenContainer = this.GetZonen(view, holeTable);

        int rowIndex = 1;
        while (displayDimension is not null)
        {
            if (this.IsPassung(displayDimension, out _, out var dimension))
            {
                displayDimension = feat.GetNextDisplayDimension(displayDimension).As<DisplayDimension>();
                continue;
            }

            string prefix = displayDimension.Type2 == (int)swDimensionType_e.swDiameterDimension ? "Ø" : "";

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
        var tables = holeTable.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();

        var result = new Dictionary<string, List<string>>();
        for (int rowIndex = 0; rowIndex < tables.Sum(t => t.RowCount) - 1; rowIndex++)
        {
            var tag = holeTable.HoleTag[rowIndex];
            if (tag is null)
            {
                continue;
            }

            result.Add(tag, []);
        }

        var notes = view.GetNotes().AsArrayOfType<INote>();

        foreach (var note in notes)
        {
            if (result.TryGetValue(note.PropertyLinkedText, out var zones))
            {
                var point = note.GetTextPoint2().AsArrayOfType<double>();
                var zone = view.Sheet.GetZone(point);
                if (zone is not null)
                {
                    zones.Add(zone);
                }
            }
        }

        return result;
    }
}
