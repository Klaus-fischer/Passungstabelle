// <copyright file="HoleTableAnalyzer" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

internal class HoleTableAnalyzer(IView view)
{
    private readonly IView view = view;


    public IEnumerable<PassungEntity> GetPassungen()
    {
        ITableAnnotation[] tables = view.GetTableAnnotations().AsArrayOfType<ITableAnnotation>();
        return tables.SelectMany(this.GetPassungenFromTable);
    }

    private IEnumerable<PassungEntity> GetPassungenFromTable(ITableAnnotation table)
    {
        if (table.Type != (int)swTableAnnotationType_e.swTableAnnotation_HoleChart ||
            table is not HoleTable holeTable)
        {
            return [];
        }

        return this.GetPassungenFromHoleTable(holeTable);
    }

    private IEnumerable<PassungEntity> GetPassungenFromHoleTable(IHoleTable holeTable)
    {
        var annotations = holeTable.GetTableAnnotations().AsArrayOfType<IHoleTableAnnotation>();

        return annotations.SelectMany(annotation => this.GetPassungenFromHoleTableAnnotation(annotation, holeTable));
    }

    private IEnumerable<PassungEntity> GetPassungenFromHoleTableAnnotation(IHoleTableAnnotation annotation, IHoleTable holeTable)
    {
        var feat = holeTable.GetFeature();
        var displayDimension = feat.GetFirstDisplayDimension().As<DisplayDimension>();
        int index = 0;

        var zonenContainer = this.GetZonen(holeTable);

        int rowIndex = 1;
        while (displayDimension is not null)
        {
            string prefix = "";
            if (displayDimension.Type2 == (int)swDimensionType_e.swDiameterDimension)
            {
                prefix = "Ø";
            }

            Dimension dimension = displayDimension.GetDimension2(0);
            var passung = dimension.GetPassung(true, prefix, string.Empty);

            var zoneKey = holeTable.HoleTag[rowIndex];
            var zonen = zonenContainer.ContainsKey(zoneKey) ? zonenContainer[zoneKey] : [];
            passung.Zone.AddRange(zonen);
            yield return passung;

            // Prüfung ob es sich um eine Bohrungsbeschreibung handelt
            var holeVariables = displayDimension.GetHoleCalloutVariables().AsArrayOfType<ICalloutVariable>();

            // Wenn Bohrungs-Beschreibungs-Variablen gefunden wurden
            foreach (var callOutPassung in holeVariables.GetTolerances(prefix))
            {
                callOutPassung.Zone.AddRange(zonen);
                yield return callOutPassung;
            }

            displayDimension = feat.GetNextDisplayDimension(displayDimension).As<DisplayDimension>();
            index++;
        }
    }

    private Dictionary<string, List<string>> GetZonen(IHoleTable holeTable)
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
