// <copyright file="FormstSettingWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.IO;
using System.Windows;
using System.Xml;

internal class FormstSettingWriter
{
    public void WriteFormatSettings(FormatSettings[] formats, string outputPath)
    {
        var xmlSettings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "  ",
            WriteEndDocumentOnClose = true,
        };

        using XmlWriter writer = XmlWriter.Create(Path.Combine(outputPath, "FormatSettings.xml"), xmlSettings);

        writer.WriteStartDocument();
        writer.WriteStartElement("FormatSettings");
        writer.WriteAttributeString("Version", "1.0");

        foreach (var item in formats)
        {
            this.Export(item, writer);
        }
    }

    private void Export(FormatSettings settings, XmlWriter writer)
    {
        writer.WriteStartElement("Format");
        {
            writer.WriteAttributeString(nameof(FormatSettings.Name), settings.Name);
            writer.WriteAttributeString(nameof(FormatSettings.SheetFormat), settings.SheetFormat.ToString());
            writer.WriteAttributeString(nameof(FormatSettings.InsertPoint), settings.InsertPoint.ToString());
            writer.WriteAttributeString(nameof(FormatSettings.MaxZone), settings.MaxZone);

            writer.WriteStartElement(nameof(FormatSettings.Margin));
            {
                writer.WriteAttributeString(nameof(Thickness.Left), $"{settings.Margin.Left}");
                writer.WriteAttributeString(nameof(Thickness.Top), $"{settings.Margin.Top}");
                writer.WriteAttributeString(nameof(Thickness.Right), $"{settings.Margin.Right}");
                writer.WriteAttributeString(nameof(Thickness.Bottom), $"{settings.Margin.Bottom}");
            }
            writer.WriteEndElement(); // of Margin

            writer.WriteStartElement(nameof(FormatSettings.Offset));
            {
                writer.WriteAttributeString(nameof(Vector.X), $"{settings.Offset.X}");
                writer.WriteAttributeString(nameof(Vector.Y), $"{settings.Offset.Y}");
            }
            writer.WriteEndElement(); // of Offset
        }
        writer.WriteEndElement(); // of Format
    }
}
