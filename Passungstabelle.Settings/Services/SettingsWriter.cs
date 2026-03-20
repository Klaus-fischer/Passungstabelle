// <copyright file="GeneralSettingsWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Web;
using System.Windows;
using System.Xml;

internal static class SettingsWriter
{
    public static void WriteSettings(
        GeneralSettings settings,
        FormatSettings[] formats,
        TableSettings[] tables,
        TemplateSettings[] templates,
        string outputPath,
        bool userSettingsOnly)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "  ",
                WriteEndDocumentOnClose = true,
            };

            using XmlWriter writer = XmlWriter.Create(Path.Combine(outputPath, DefaultLocations.SettingsFilename), xmlSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");
            writer.WriteAttributeString("AddInVersion", "9.0");
            writer.WriteAttributeString("ExportDate", $"{DateTime.Now:s}");

            ExportGeneralSettings(settings, userSettingsOnly, writer);
            if (!userSettingsOnly)
            {
                ExportFormats(formats, writer);
                ExportTables(tables, writer);
                ExportTemplates(templates, writer);
            }

            writer.WriteEndElement(); // Settings
            writer.WriteEndDocument();
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ExportGeneralSettings(GeneralSettings settings, bool userSettingsOnly, XmlWriter writer)
    {
        if (userSettingsOnly)
        {
            writer.WriteComment("User specific settings. Will override the common settings.");
        }

        writer.WriteStartElement("GeneralSettings");
        if (!userSettingsOnly)
        {
            writer.WriteAttributeString(nameof(GeneralSettings.UseCentralLocation), $"{settings.UseCentralLocation}");
            writer.WriteAttributeString(nameof(GeneralSettings.CentralLocation), settings.CentralLocation);
            writer.WriteAttributeString(nameof(GeneralSettings.UsePlusSign), $"{settings.UsePlusSign}");
            writer.WriteAttributeString(nameof(GeneralSettings.OnlyAtFirstSheet), $"{settings.OnlyAtFirstSheet}");
            writer.WriteAttributeString(nameof(GeneralSettings.RemoveAtAllPages), $"{settings.RemoveAtAllPages}");
        }

        writer.WriteAttributeString(nameof(GeneralSettings.Language), settings.Language);
        writer.WriteAttributeString(nameof(GeneralSettings.CreateLogFile), $"{settings.CreateLogFile}");
        writer.WriteAttributeString(nameof(GeneralSettings.LogFilePath), settings.LogFilePath);
        writer.WriteAttributeString(nameof(GeneralSettings.UseEvents), $"{settings.UseEvents}");
        writer.WriteAttributeString(nameof(GeneralSettings.RecalculateBeforeSave), $"{settings.RecalculateBeforeSave}");
        writer.WriteAttributeString(nameof(GeneralSettings.RecalculateAfterRebuild), $"{settings.RecalculateAfterRebuild}");
        writer.WriteAttributeString(nameof(GeneralSettings.SuppressMessages), $"{settings.SuppressMessages}");

        writer.WriteEndElement();
    }

    private static void ExportFormats(FormatSettings[] formats, XmlWriter writer)
    {
        writer.WriteStartElement("FormatSettings");

        foreach (var format in formats)
        {
            ExportFormat(format, writer);
        }

        writer.WriteEndElement(); // FormatSettings
    }

    private static void ExportFormat(FormatSettings format, XmlWriter writer)
    {
        writer.WriteStartElement("Format");
        {
            writer.WriteAttributeString(nameof(FormatSettings.Name), format.Name);
            writer.WriteAttributeString(nameof(FormatSettings.SheetFormat), format.SheetFormat.ToString());
            writer.WriteAttributeString(nameof(FormatSettings.InsertPoint), format.InsertPoint.ToString());
            writer.WriteAttributeString(nameof(FormatSettings.MaxZone), format.MaxZone);

            writer.WriteStartElement(nameof(FormatSettings.Margin));
            {
                writer.WriteAttributeString(nameof(Thickness.Left), $"{format.Margin.Left}");
                writer.WriteAttributeString(nameof(Thickness.Top), $"{format.Margin.Top}");
                writer.WriteAttributeString(nameof(Thickness.Right), $"{format.Margin.Right}");
                writer.WriteAttributeString(nameof(Thickness.Bottom), $"{format.Margin.Bottom}");
            }
            writer.WriteEndElement(); // of Margin

            writer.WriteStartElement(nameof(FormatSettings.Offset));
            {
                writer.WriteAttributeString(nameof(Vector.X), $"{format.Offset.X}");
                writer.WriteAttributeString(nameof(Vector.Y), $"{format.Offset.Y}");
            }
            writer.WriteEndElement(); // of Offset
        }
        writer.WriteEndElement(); // of Format
    }

    public static void ExportTables(TableSettings[] tables, XmlWriter writer)
    {
        writer.WriteStartElement("TableSettings");

        foreach (var table in tables)
        {
            ExportTable(table, writer);
        }

        writer.WriteEndElement(); // TableSettings
    }

    private static void ExportTable(TableSettings settings, XmlWriter writer)
    {
        writer.WriteStartElement("Table");
        writer.WriteAttributeString(nameof(TableSettings.Name), settings.Name);
        writer.WriteAttributeString(nameof(TableSettings.RasterStrichStärke), settings.RasterStrichStärke.ToString());
        writer.WriteAttributeString(nameof(TableSettings.RahmenStrichStärke), settings.RahmenStrichStärke.ToString());
        writer.WriteAttributeString(nameof(TableSettings.HeaderPosition), settings.HeaderPosition.ToString());

        WriteTextFormat(writer, nameof(TableSettings.HeaderFormat), settings.HeaderFormat);
        WriteTextFormat(writer, nameof(TableSettings.TextFormat), settings.TextFormat);

        writer.WriteStartElement("Columns");
        foreach (var spalte in settings.Spalten)
        {
            WriteSpalteSettings(writer, spalte);
        }
        writer.WriteEndElement(); // Spalten
        writer.WriteEndElement(); // Table
    }

    private static void ExportTemplates(TemplateSettings[] templates, XmlWriter writer)
    {
        writer.WriteStartElement("TemplateSettings");

        foreach (var template in templates)
        {
            ExportTemplate(template, writer);
        }

        writer.WriteEndElement(); // TableSettings
    }

    private static void ExportTemplate(TemplateSettings settings, XmlWriter writer)
    {
        writer.WriteStartElement("Template");
        writer.WriteAttributeString(nameof(TemplateSettings.TemplateNamePattern), HttpUtility.HtmlAttributeEncode(settings.TemplateNamePattern));
        writer.WriteAttributeString(nameof(TemplateSettings.TableSchemaName), settings.TableSchemaName);

        writer.WriteStartElement("AllowedFormats");
        foreach (var formatName in settings.FormatNames)
        {
            writer.WriteStartElement("Format");
            writer.WriteAttributeString("Name", formatName);
            writer.WriteEndElement();
        }
        writer.WriteEndElement(); // AllowedFormats
        writer.WriteEndElement(); // Template
    }

    private static void WriteTextFormat(XmlWriter writer, string elementName, TextFormat format)
    {
        writer.WriteStartElement(elementName);
        writer.WriteAttributeString(nameof(TextFormat.Schriftart), format.Schriftart);
        writer.WriteAttributeString(nameof(TextFormat.Schriftstil), format.Schriftstil);
        writer.WriteAttributeString(nameof(TextFormat.Texthöhe), $"{format.Texthöhe:0.###}");
        writer.WriteAttributeString(nameof(TextFormat.Fett), $"{format.Fett}");
        writer.WriteAttributeString(nameof(TextFormat.Unterstrichen), $"{format.Unterstrichen}");
        writer.WriteAttributeString(nameof(TextFormat.Durchgestrichen), $"{format.Durchgestrichen}");
        writer.WriteAttributeString(nameof(TextFormat.Kursiv), $"{format.Kursiv:0.###}");
        writer.WriteAttributeString(nameof(TextFormat.RgbFarbe), $"{format.RgbFarbe}");
        writer.WriteEndElement();
    }

    private static void WriteSpalteSettings(XmlWriter writer, ColumnSettings spalte)
    {
        writer.WriteStartElement("Column");
        writer.WriteAttributeString(nameof(ColumnSettings.Name), spalte.Name);
        writer.WriteAttributeString(nameof(ColumnSettings.Title), spalte.Title);
        writer.WriteAttributeString(nameof(ColumnSettings.SubTitle), spalte.SubTitle ?? "");
        writer.WriteAttributeString(nameof(ColumnSettings.Visible), $"{spalte.Visible}");
        writer.WriteAttributeString(nameof(ColumnSettings.AutoBreite), $"{spalte.AutoBreite}");
        writer.WriteAttributeString(nameof(ColumnSettings.Breite), $"{spalte.Breite:0.###}");
        writer.WriteEndElement();
    }

}
