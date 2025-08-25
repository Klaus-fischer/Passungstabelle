// <copyright file="ModelDocExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

internal static class ModelDocExtensions
{
    public static void SetStringProperty(this IModelDoc2 modelDoc, string propertyName, string? value, string configuration = "")
    {
        var config = modelDoc.Extension.CustomPropertyManager[configuration];

        if (value is null)
        {
            config.Delete2(propertyName);
        }
        else
        {
            config.Add3(
                FieldName: propertyName,
                FieldType: (int)swCustomInfoType_e.swCustomInfoText,
                FieldValue: value,
                OverwriteExisting: (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
        }

        modelDoc.SetSaveFlag();
    }

    /// <inheritdoc/>
    public static string? GetStringProperty(this IModelDoc2 modelDoc, string propertyName, string configuration = "")
    {
        var config = modelDoc.Extension.CustomPropertyManager[configuration];

        swCustomInfoGetResult_e result =
            (swCustomInfoGetResult_e)config.Get5(
                FieldName: propertyName,
                UseCached: false,
                ValOut: out string value,
                ResolvedValOut: out string resolvedValOut,
                WasResolved: out _);

        if (result == swCustomInfoGetResult_e.swCustomInfoGetResult_NotPresent)
        {
            return null;
        }

        return result == swCustomInfoGetResult_e.swCustomInfoGetResult_ResolvedValue
            ? resolvedValOut
            : value;
    }
}
