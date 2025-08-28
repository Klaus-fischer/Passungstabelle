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
internal class GeneralSettingsReader
{
    public void ReadGeneralSettings(string inputPath, ref GeneralSettings settings)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, "GeneralSettings.xml");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("GeneralSettings.xml nicht gefunden.", filePath);
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
            switch (reader.Name)
            {
                case nameof(GeneralSettings.Language):
                    settings.Language = reader.Value;
                    break;
                case nameof(GeneralSettings.UseCentralLocation):
                    settings.UseCentralLocation = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.CentralLocation):
                    settings.CentralLocation = reader.Value;
                    break;
                case nameof(GeneralSettings.UsePlusSign):
                    settings.UsePlusSign = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.OnlyAtFirstSheet):
                    settings.OnlyAtFirstSheet = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.RemoveAtAllPages):
                    settings.RemoveAtAllPages = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.CreateLogFile):
                    settings.CreateLogFile = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.LogFilePath):
                    settings.LogFilePath = reader.Value;
                    break;
                case nameof(GeneralSettings.UseEvents):
                    settings.UseEvents = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.RecalculateBeforeSave):
                    settings.RecalculateBeforeSave = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.RecalculateAfterRebuild):
                    settings.RecalculateAfterRebuild = ParseBool(reader.Value);
                    break;
                case nameof(GeneralSettings.SuppressMessages):
                    settings.SuppressMessages = ParseBool(reader.Value);
                    break;
                // AddInVersion, ExportDate und unbekannte Attribute werden ignoriert
            }
        }
        reader.MoveToElement();
    }

    private static bool ParseBool(string value)
        => value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1";
}