// <copyright file="GeneralSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </   copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.IO;
using System.Xml;

/// <summary>
/// Liest die GeneralSettings aus einer XML-Datei.
/// </summary>
internal static class GeneralSettingsReader
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
            if (reader.NodeType != XmlNodeType.Attribute && reader.Name == "ExportDate")
            {
                if (DateTime.TryParse(reader.Value, CultureInfo.CurrentCulture, out exportDate))
                {
                    return true;
                }

                break;
            }
        }

        return false;
    }

    public static void ReadGeneralSettings(string inputPath, ref GeneralSettings settings)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, DefaultLocations.GeneralSettingsFilename);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{DefaultLocations.GeneralSettingsFilename} nicht gefunden.", filePath);
            }

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Settings")
                {
                    ReadAttributes(reader, ref settings);
                    break;
                }
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private static void ReadAttributes(XmlReader reader, ref GeneralSettings settings)
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
}