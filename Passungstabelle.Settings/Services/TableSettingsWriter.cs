// <copyright file="TableSettingsWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.IO;
using System.Xml;

internal class TableSettingsWriter
{
    public void WriteTableSettings(TableSettings[] tables, string outputPath)
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

            using XmlWriter writer = XmlWriter.Create(Path.Combine(outputPath, "TableSettings.xml"), xmlSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("TableSettings");
            writer.WriteAttributeString("AddInVersion", "9.0");
            writer.WriteAttributeString("ExportDate", $"{DateTime.Now:s}");

            foreach (var table in tables)
            {
                WriteTableSettings(writer, table);
            }

            writer.WriteEndElement(); // TableSettings
            writer.WriteEndDocument();
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private void WriteTableSettings(XmlWriter writer, TableSettings settings)
    {
        writer.WriteStartElement("Table");
        writer.WriteAttributeString(nameof(TableSettings.SchemaName), settings.SchemaName);
        writer.WriteAttributeString(nameof(TableSettings.RasterStrichStärke), settings.RasterStrichStärke.ToString());
        writer.WriteAttributeString(nameof(TableSettings.RahmenStrichStärke), settings.RahmenStrichStärke.ToString());
        writer.WriteAttributeString(nameof(TableSettings.HeaderPosition), settings.HeaderPosition.ToString());

        WriteTextFormat(writer, nameof(TableSettings.HeaderFormat), settings.HeaderFormat);
        WriteTextFormat(writer, nameof(TableSettings.TextFormat), settings.TextFormat);

        writer.WriteStartElement("Spalten");
        foreach (var spalte in settings.Spalten)
        {
            WriteSpalteSettings(writer, spalte);
        }
        writer.WriteEndElement(); // Spalten
        writer.WriteEndElement(); // Table
    }

    private void WriteTextFormat(XmlWriter writer, string elementName, TextFormat format)
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

    private void WriteSpalteSettings(XmlWriter writer, SpalteSettings spalte)
    {
        writer.WriteStartElement("Spalte");
        writer.WriteAttributeString(nameof(SpalteSettings.Name), spalte.Name);
        writer.WriteAttributeString(nameof(SpalteSettings.Title), spalte.Title);
        writer.WriteAttributeString(nameof(SpalteSettings.SubTitle), spalte.SubTitle ?? "");
        writer.WriteAttributeString(nameof(SpalteSettings.Visible), $"{spalte.Visible}");
        writer.WriteAttributeString(nameof(SpalteSettings.AutoBreite), $"{spalte.AutoBreite}");
        writer.WriteAttributeString(nameof(SpalteSettings.Breite), $"{spalte.Breite:0.###}");
        writer.WriteEndElement();
    }
}