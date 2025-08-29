// <copyright file="FormatSettingWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Web;
using System.Xml;

internal static class TemplateSettingWriter
{
    public static void WriteTemplateSettings(TemplateSettings[] templates, string outputPath)
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

            using XmlWriter writer = XmlWriter.Create(Path.Combine(outputPath, DefaultLocations.TemplateSettingsFilename), xmlSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("TemplateSettings");
            writer.WriteAttributeString("AddInVersion", "9.0");
            writer.WriteAttributeString("ExportDate", $"{DateTime.Now:s}");

            foreach (var item in templates)
            {
                Export(item, writer);
            }

            writer.WriteEndElement(); // FormatSettings
            writer.WriteEndDocument();
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void Export(TemplateSettings settings, XmlWriter writer)
    {
        writer.WriteStartElement("Template");
        {
            writer.WriteAttributeString(nameof(TemplateSettings.TableSchemaName),
                settings.TableSchemaName.ToString());
            writer.WriteAttributeString(nameof(TemplateSettings.TemplateNamePattern),
                 HttpUtility.HtmlAttributeEncode(settings.TemplateNamePattern));

            writer.WriteStartElement(nameof(TemplateSettings.FormatNames));

            foreach (var formatName in settings.FormatNames)
            {
                writer.WriteElementString("FormatName", value: formatName);
            }

            writer.WriteEndElement(); // of FormatNames

        }
        writer.WriteEndElement(); // of Template
    }
}
