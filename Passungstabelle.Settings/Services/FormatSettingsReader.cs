// <copyright file="FormatSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

/// <summary>
/// Liest FormatSettings aus einer XML-Datei.
/// </summary>
internal static class FormatSettingsReader
{
    public static void ReadFormatSettings(string inputPath, List<FormatSettings> formats)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, DefaultLocations.FormatSettingsFilename);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"{DefaultLocations.FormatSettingsFilename} nicht gefunden.", filePath);

            var result = new List<FormatSettings>();

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Format")
                {
                    ReadFormat(reader, formats);
                }
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ReadFormat(XmlReader reader, List<FormatSettings> formats)
    {
        string? name = null;
        TableInsertPoint? insertPoint = null;
        SheetFormat? sheetFormat = null;
        string? maxZone = null;

        // Attribute lesen
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            switch (reader.Name)
            {
                case nameof(FormatSettings.Name):
                    name = reader.Value;
                    break;
                case nameof(FormatSettings.InsertPoint):
                    insertPoint = reader.ReadEnum<TableInsertPoint>();
                    break;
                case nameof(FormatSettings.SheetFormat):
                    sheetFormat = reader.ReadEnum<SheetFormat>();
                    break;
                case nameof(FormatSettings.MaxZone):
                    maxZone = reader.Value;
                    break;
            }
        }
        reader.MoveToElement();

        if (name is null)
        {
            return;
        }

        if (formats.FirstOrDefault(o => o.Name == name) is not FormatSettings format)
        {
            format = new FormatSettings()
            {
                Name = name,
            };

            formats.Add(format);
        }

        format.InsertPoint = insertPoint ?? format.InsertPoint;
        format.SheetFormat = sheetFormat ?? format.SheetFormat;
        format.MaxZone = maxZone ?? format.MaxZone;

        // Sub-Elemente lesen
        if (reader.IsEmptyElement)
        {
            return;
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Format")
                break;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == nameof(FormatSettings.Margin))
                {
                    format.Margin = reader.ReadThickness(format.Margin);
                }
                else if (reader.Name == nameof(FormatSettings.Offset))
                {
                    reader.ReadVector(format.Offset);
                }
            }
        }
    }
}