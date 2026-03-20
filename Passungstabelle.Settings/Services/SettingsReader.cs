// <copyright file="GeneralSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </   copyright>

namespace Passungstabelle.Settings;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Liest die GeneralSettings aus einer XML-Datei.
/// </summary>
internal class SettingsReader
{
    public static bool TryGetExportDate(string inputFilename, out DateTime exportDate)
    {
        exportDate = default;
        if (!File.Exists(inputFilename))
        {
            return false;
        }

        using var reader = XmlReader.Create(inputFilename, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Settings")
            {
                if (reader.MoveToAttribute("ExportDate"))
                {
                    if (DateTime.TryParse(reader.Value, CultureInfo.CurrentCulture, out exportDate))
                    {
                        return true;
                    }

                    break;
                }
            }
        }

        return false;
    }

    public static void ReadGeneralSettings(string inputPath, ref GeneralSettings settings, ref List<FormatSettings> formats, ref List<TableSettings> tables, ref List<TemplateSettings> templates)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, DefaultLocations.SettingsFilename);
            if (!File.Exists(filePath))
            {
                GlobalLog.Default.LogWarning("{File} nicht gefunden.", filePath);
                return;
            }

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                if (reader.Name == "GeneralSettings")
                {
                    ReadAndUpdateGeneralSettings(reader, ref settings);
                }

                if (reader.Name == "Format")
                {
                    ReadAndAddOrUpdateFormats(reader, formats);
                }

                if (reader.Name == "Table")
                {
                    ReadAndAddOrUpdateTables(reader, tables);
                }

                if (reader.Name == "Template")
                {
                    ReadAndAddOrUpdateTemplates(reader, templates);
                }
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ReadAndUpdateGeneralSettings(XmlReader reader, ref GeneralSettings settings)
    {
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            if (reader.Name == nameof(GeneralSettings.Language))
            {
                settings.Language = reader.Value;
            }
            else if (reader.Name == nameof(GeneralSettings.UseCentralLocation))
            {
                settings.UseCentralLocation = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.CentralLocation))
            {
                settings.CentralLocation = reader.Value;
            }
            else if (reader.Name == nameof(GeneralSettings.UsePlusSign))
            {
                settings.UsePlusSign = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.OnlyAtFirstSheet))
            {
                settings.OnlyAtFirstSheet = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.RemoveAtAllPages))
            {
                settings.RemoveAtAllPages = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.CreateLogFile))
            {
                settings.CreateLogFile = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.LogFilePath))
            {
                settings.LogFilePath = reader.Value;
            }
            else if (reader.Name == nameof(GeneralSettings.UseEvents))
            {
                settings.UseEvents = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.RecalculateBeforeSave))
            {
                settings.RecalculateBeforeSave = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.RecalculateAfterRebuild))
            {
                settings.RecalculateAfterRebuild = reader.ReadBool();
            }
            else if (reader.Name == nameof(GeneralSettings.SuppressMessages))
            {
                settings.SuppressMessages = reader.ReadBool();
            }
            // AddInVersion, ExportDate und unbekannte Attribute werden ignoriert
        }
        reader.MoveToElement();
    }

    private static void ReadAndAddOrUpdateFormats(XmlReader reader, List<FormatSettings> formats)
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

    private static void ReadAndAddOrUpdateTables(XmlReader reader, List<TableSettings> tables)
    {
        string? name = null;
        LineWidth? raster = null;
        LineWidth? rahmen = null;
        HeaderPosition? headerPos = null;

        // Attribute lesen
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);

            if (reader.Name == nameof(TableSettings.Name))
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

        if (tables.FirstOrDefault(o => o.Name == name) is not TableSettings table)
        {
            table = new TableSettings()
            {
                Name = name,
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
                    ReadAndUpdateTableTextFormat(reader, table.HeaderFormat);
                }
                else if (reader.Name == nameof(TableSettings.TextFormat))
                {
                    ReadAndUpdateTableTextFormat(reader, table.TextFormat);
                }
                else if (reader.Name == "Columns")
                {
                    ReadAndUpdateTableSpalten(reader, table.Spalten);
                }
            }
        }
    }

    private static void ReadAndUpdateTableTextFormat(XmlReader reader, TextFormat format)
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

    private static void ReadAndUpdateTableSpalten(XmlReader reader, IEnumerable<ColumnSettings> spalten)
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


    private static void ReadAndAddOrUpdateTemplates(XmlReader reader, List<TemplateSettings> templates)
    {
        string pattern = "";
        string tableSchemaName = "";

        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            switch (reader.Name)
            {
                case nameof(TemplateSettings.TemplateNamePattern):
                    pattern = HttpUtility.HtmlDecode(reader.Value);
                    break;
                case nameof(TemplateSettings.TableSchemaName):
                    tableSchemaName = reader.Value;
                    break;
            }
        }
        reader.MoveToElement();

        if (templates.FirstOrDefault(o => o.TemplateNamePattern == pattern) is not TemplateSettings template)
        {
            template = new TemplateSettings()
            {
                TemplateNamePattern = pattern,
                TableSchemaName = tableSchemaName,
            };

            templates.Add(template);
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Table")
                break;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == nameof(TableSettings.HeaderFormat))
                {
                    ReadAndUpdateTableTextFormat(reader, table.HeaderFormat);
                }
                else if (reader.Name == nameof(TableSettings.TextFormat))
                {
                    ReadAndUpdateTableTextFormat(reader, table.TextFormat);
                }
                else if (reader.Name == "Columns")
                {
                    ReadAndUpdateTableSpalten(reader, table.Spalten);
                }
            }
        }

    }

}