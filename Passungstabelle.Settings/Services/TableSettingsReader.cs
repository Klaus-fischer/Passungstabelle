// <copyright file="TableSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

/// <summary>
/// Liest TableSettings aus einer XML-Datei.
/// </summary>
internal static class TableSettingsReader
{
    public static void ReadTableSettings(string inputPath, List<TableSettings> tables)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, DefaultLocations.TableSettingsFilename);
            if (!File.Exists(filePath))
            {
                GlobalLog.Default.LogWarning("{File} nicht gefunden.", filePath);
                return;
            }

            var result = new List<TableSettings>();

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                {
                    ReadAndAddOrUpdateTable(reader, tables);
                }
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ReadAndAddOrUpdateTable(XmlReader reader, List<TableSettings> tables)
    {
        string? name = null;
        LineWidth? raster = null;
        LineWidth? rahmen = null;
        HeaderPosition? headerPos = null;

        // Attribute lesen
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);

            if (reader.Name == nameof(TableSettings.SchemaName))
            {
                name = reader.Value;
            }
            else if (reader.Name == nameof(TableSettings.RasterStrichStärke))
            {
                raster = reader.ReadEnum<LineWidth>();
            }
            else if (reader.Name == nameof(TableSettings.RahmenStrichStärke))
            {
                rahmen = reader.ReadEnum<LineWidth>();
            }
            else if (reader.Name == nameof(TableSettings.HeaderPosition))
            {
                headerPos = reader.ReadEnum<HeaderPosition>();
            }
        }
        reader.MoveToElement();

        if (name is null)
        {
            return;
        }

        if (tables.FirstOrDefault(o => o.SchemaName == name) is not TableSettings table)
        {
            table = new TableSettings()
            {
                SchemaName = name,
                RasterStrichStärke = raster ?? LineWidth.Dünn,
                RahmenStrichStärke = rahmen ?? LineWidth.Dick,
                HeaderPosition = headerPos ?? HeaderPosition.Oben,
            };

            tables.Add(table);
        }

        // Sub-Elemente lesen
        if (reader.IsEmptyElement)
        {
            return;
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Table")
                break;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == nameof(TableSettings.HeaderFormat))
                {
                    ReadAndUpdateTextFormat(reader, table.HeaderFormat);
                }
                else if (reader.Name == nameof(TableSettings.TextFormat))
                {
                    ReadAndUpdateTextFormat(reader, table.TextFormat);
                }
                else if (reader.Name == "Columns")
                {
                    ReadAndUpdateSpalten(reader, table.Spalten);
                }
            }
        }
    }

    private static void ReadAndUpdateTextFormat(XmlReader reader, TextFormat format)
    {
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);

            if (reader.Name == nameof(TextFormat.Schriftart))
            {
                format.Schriftart = reader.Value;
            }
            else if (reader.Name == nameof(TextFormat.Schriftstil))
            {
                format.Schriftstil = reader.Value;
            }
            else if (reader.Name == nameof(TextFormat.Texthöhe))
            {
                format.Texthöhe = reader.ReadDouble() ?? format.Texthöhe;
            }
            else if (reader.Name == nameof(TextFormat.Fett))
            {
                format.Fett = reader.ReadBool();
            }
            else if (reader.Name == nameof(TextFormat.Unterstrichen))
            {
                format.Unterstrichen = reader.ReadBool();
            }
            else if (reader.Name == nameof(TextFormat.Durchgestrichen))
            {
                format.Durchgestrichen = reader.ReadBool();
            }
            else if (reader.Name == nameof(TextFormat.Kursiv))
            {
                format.Kursiv = reader.ReadBool();
            }
            else if (reader.Name == nameof(TextFormat.RgbFarbe))
            {
                format.RgbFarbe = reader.ReadInt() ?? format.RgbFarbe;
            }
        }

        reader.MoveToElement();
        // Leeres Element überspringen
        if (!reader.IsEmptyElement)
            reader.Read();
    }

    private static void ReadAndUpdateSpalten(XmlReader reader, IEnumerable<ColumnSettings> spalten)
    {
        if (reader.IsEmptyElement)
            return;

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Columns")
                break;

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Column")
            {
                string? name = null;
                string? title = null;
                string? subTitle = null;
                bool? visible = null;
                bool? autoBreite = null;
                double? breite = null;

                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    switch (reader.Name)
                    {
                        case nameof(ColumnSettings.Name):
                            name = reader.Value;
                            break;
                        case nameof(ColumnSettings.Title):
                            title = reader.Value;
                            break;
                        case nameof(ColumnSettings.SubTitle):
                            subTitle = reader.Value;
                            break;
                        case nameof(ColumnSettings.Visible):
                            visible = reader.ReadBool();
                            break;
                        case nameof(ColumnSettings.AutoBreite):
                            autoBreite = reader.ReadBool();
                            break;
                        case nameof(ColumnSettings.Breite):
                            breite = reader.ReadDouble();
                            break;
                    }
                }

                if (spalten.FirstOrDefault(o => o.Name == name) is ColumnSettings spalte)
                {
                    spalte.Title = title ?? spalte.Title;
                    spalte.SubTitle = subTitle ?? spalte.SubTitle;
                    spalte.Visible = visible ?? spalte.Visible;
                    spalte.AutoBreite = autoBreite ?? spalte.AutoBreite;
                    spalte.Breite = breite ?? spalte.Breite;
                }

                reader.MoveToElement();
            }
        }
    }
}