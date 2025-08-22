// <copyright file="ISheetExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

internal static class ISheetExtensions
{
    private const double factor = 1000;

    public static IEnumerable<ISheet> GetSheets(this IDrawingDoc drawing)
    {
        var sheetNames = drawing.GetSheetNames().AsArrayOfType<string>();
        foreach (var sheetName in sheetNames)
        {
            yield return drawing.Sheet[sheetName];
        }
    }

    public static IEnumerable<IView> GetAllViews(this IDrawingDoc drawing)
    {
        object? view = drawing.GetFirstView();

        while (view is IView swView)
        {
            yield return swView;

            view = swView.GetNextView();
        }
    }

    public static bool CheckForDiameter(this IDisplayDimension displayDimension)
    {
        if (displayDimension.Type2 == (int)swDimensionType_e.swDiameterDimension)
        {
            return true;
        }

        var temp = displayDimension.GetText((int)swDimensionTextParts_e.swDimensionTextPrefix);

        if (temp.Contains('Ø') ||
            temp.Contains("<MOD-DIAM>", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public static string GetZoneFromDisplayDimension(this IDisplayDimension displayDimension, IView swView, ISheet sheet)
    {
        double[] dimPosition;
        Annotation swAnnotation;

        swAnnotation = (Annotation)displayDimension.GetAnnotation();
        dimPosition = swAnnotation.GetPosition().AsArrayOfType<double>();

        sheet = swView.Sheet ?? sheet;

        return sheet.GetDrawingZone(dimPosition[0], dimPosition[1]);
    }

    public static PassungEntity GetPassung(this IDimension dimension, bool isHole, string prefix, string? zone = null)
    {
        var tolerance = dimension.Tolerance;
        var holeFit = tolerance.GetHoleFitValue();
        var shaftFit = tolerance.GetShaftFitValue();

        if (isHole)
        {
            tolerance.SetFitValues(holeFit, "");
        }
        else
        {
            tolerance.SetFitValues("", shaftFit);
        }

        PassungEntity result = new()
        {
            Maß = dimension.GetSystemValue2("") * factor,
            Passung = isHole ? holeFit : shaftFit,
            Prefix = prefix,
            PassungsType = isHole ? PassungsType.Hole : PassungsType.Shaft,
            ToleranzO = tolerance.GetMaxValue() * factor,
            ToleranzU = tolerance.GetMinValue() * factor
        };

        if (!string.IsNullOrEmpty(zone))
        {
            result.Zone.Add(zone);
        }

        tolerance.SetFitValues(holeFit, shaftFit);
        return result;
    }

    public static IEnumerable<PassungEntity> GetTolerances(this ICalloutVariable[] calloutVariables, string prefix, string? zone = null)
    {
        foreach (ICalloutVariable swCalloutVariable in calloutVariables)
        {
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

            var maß = swCalloutLengthVariable.Length * factor;
            var holeFit = swCalloutVariable.HoleFit;
            var shaftFit = swCalloutVariable.ShaftFit;

            if (!string.IsNullOrWhiteSpace(holeFit))
            {
                var passung = swCalloutVariable.GetPassung(maß, true, prefix, zone);
                if (passung != null)
                {
                    yield return passung;
                }
            }

            if (!string.IsNullOrWhiteSpace(shaftFit))
            {
                var passung = swCalloutVariable.GetPassung(maß, false, prefix, zone);
                if (passung != null)
                {
                    yield return passung;
                }
            }
        }
    }

    public static PassungEntity GetPassung(this ICalloutVariable swCalloutVariable, double maß, bool isHole, string prefix, string? zone = null)
    {
        var holeFit = swCalloutVariable.HoleFit;
        var shaftFit = swCalloutVariable.ShaftFit;

        var passung = isHole ? holeFit : shaftFit;

        swCalloutVariable.ShaftFit = isHole ? string.Empty : shaftFit;
        swCalloutVariable.HoleFit = isHole ? holeFit : string.Empty;

        var result = new PassungEntity()
        {

            Maß = maß,
            Passung = passung,
            ToleranzO = swCalloutVariable.ToleranceMax,
            ToleranzU = swCalloutVariable.ToleranceMin,
            PassungsType = isHole ? PassungsType.Hole : PassungsType.Shaft,
            Prefix = prefix,
        };

        swCalloutVariable.HoleFit = holeFit;
        swCalloutVariable.ShaftFit = shaftFit;

        if (!string.IsNullOrEmpty(zone))
        {
            result.Zone.Add(zone);
        }

        return result;
    }

}
