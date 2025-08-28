// <copyright file="GeneralSettingsWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.IO;
using System.Xml;

internal class GeneralSettingsWriter
{
    public void WriteGeneralSettings(GeneralSettings settings, string outputPath, bool userSettingsOnly)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = true,
                WriteEndDocumentOnClose = true,
            };

            using XmlWriter writer = XmlWriter.Create(Path.Combine(outputPath, "GeneralSettings.xml"), xmlSettings);

            this.Export(settings, userSettingsOnly, writer);
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private void Export(GeneralSettings settings, bool userSettingsOnly, XmlWriter writer)
    {
        writer.WriteStartDocument();
        if (userSettingsOnly)
        {
            writer.WriteComment("User specific settings. Will override the common settings.");
        }

        writer.WriteStartElement("Settings");
        writer.WriteAttributeString("AddInVersion", "9.0");
        writer.WriteAttributeString("ExportDate", $"{DateTime.Now:s}");

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

        writer.WriteEndElement(); // GeneralSettings
        writer.WriteEndDocument();
    }
}
