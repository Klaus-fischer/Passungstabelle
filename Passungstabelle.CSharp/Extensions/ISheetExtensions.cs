// <copyright file="ISheetExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal static class ISheetExtensions
{

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

    public static string? GetZone(this ISheet sheet, double[] array)
    {
        return sheet.GetDrawingZone(array[0], array[1]);
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

    public static Version GetVersion(this ISldWorks sldWorks)
    {
        if (Version.TryParse(sldWorks.RevisionNumber(), out var version))
        {
            return version;
        }

        return new Version();
    }
}
