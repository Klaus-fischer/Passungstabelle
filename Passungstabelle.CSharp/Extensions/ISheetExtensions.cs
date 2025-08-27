// <copyright file="ISheetExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

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

    /// <summary>
    /// Gets the insert point in mm based on <see cref="ISheet.GetSize(ref double, ref double)"/>,
    /// <see cref="FormatSettings.Margin"/> and <see cref="FormatSettings.Offset_X"/> / <see cref="FormatSettings.Offset_Y."/>
    /// </summary>
    /// <param name="sheet">The current sheet.</param>
    /// <param name="settings">The format settings.</param>
    /// <returns></returns>
    public static Point GetInsertPoint(this ISheet sheet, FormatSettings settings)
    {
        var properties = sheet.GetProperties2().AsArrayOfType<double>();
        var paperSize = new Size(properties[5] * 1000, properties[6] * 1000);
        var rectangle = new Rect(paperSize).Shrink(settings.Margin);
        var offset = settings.Offset;

        return settings.InsertPoint switch
        {
            TableInsertPoint.BottomLeft => rectangle.TopLeft + offset,
            TableInsertPoint.TopLeft => rectangle.BottomLeft + offset,
            TableInsertPoint.BottomRight => rectangle.TopRight + offset,
            TableInsertPoint.TopRight => rectangle.BottomRight + offset,
            _ => rectangle.TopRight + offset,
        };
    }

    private static Rect Shrink(this Rect rect, Thickness margin)
    {
        return new Rect(
            rect.Left + margin.Left,
            rect.Top + margin.Top,
            Math.Max(0, rect.Width - margin.Left - margin.Right),
            Math.Max(0, rect.Height - margin.Top - margin.Bottom)
        );
    }
}
