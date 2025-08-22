// <copyright file="MathPointExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;

internal static class MathPointExtensions
{
    public static string? GetZone(this MathPoint mathPoint, ISheet sheet)
    {
        if (mathPoint.ArrayData.AsArrayOfType<double>() is  double[] array && array.Length > 2)
        {
            return sheet.GetDrawingZone(array[0], array[1]);
        }

        return null;
    }
}
