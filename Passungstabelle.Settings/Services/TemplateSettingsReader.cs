// <copyright file="FormatSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Liest FormatSettings aus einer XML-Datei.
/// </summary>
internal static class TemplateSettingsReader
{
    public static void ReadTemplateSettings(string inputPath, List<TemplateSettings> templates)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, DefaultLocations.TemplateSettingsFilename);
            if (!File.Exists(filePath))
            {
                GlobalLog.Default.LogWarning("{File} nicht gefunden.", filePath);
                return;
            }

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Template")
                {
                    ReadFormat(reader, templates);
                }
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ReadFormat(XmlReader reader, List<TemplateSettings> templates)
    {
        string? schemaName = null;
        string? namePattern = null;

        // Attribute lesen
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            if (reader.Name == nameof(TemplateSettings.TableSchemaName))
            {
                schemaName = reader.Value;
            }
            else if (reader.Name == nameof(TemplateSettings.TemplateNamePattern))
            {
                namePattern = HttpUtility.HtmlDecode(reader.Value);
            }
        }
        reader.MoveToElement();

        if (schemaName is null || namePattern is null)
        {
            return;
        }

        if (templates.FirstOrDefault(o => o.TableSchemaName == schemaName && o.TemplateNamePattern == namePattern) is not TemplateSettings template)
        {
            template = new TemplateSettings()
            {
                TableSchemaName = schemaName,
                TemplateNamePattern = namePattern,
            };

            templates.Add(template);
        }

        // Sub-Elemente lesen
        if (reader.IsEmptyElement)
        {
            return;
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "FormatNames")
                break;

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "FormatName")
            {
                var name = reader.ReadElementContentAsString();
                if (!template.FormatNames.Contains(name))
                {
                    template.FormatNames = [.. template.FormatNames, name];
                }
            }
        }
    }
}