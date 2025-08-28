// <copyright file="TableSettingsReader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Xml;

/// <summary>
/// Liest TableSettings aus einer XML-Datei.
/// </summary>
internal class TableSettingsReader
{
    public TableSettings[] ReadTableSettings(string inputPath, TableSettings[] tables)
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            var filePath = Path.Combine(inputPath, "TableSettings.xml");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("TableSettings.xml nicht gefunden.", filePath);

            var result = new List<TableSettings>();

            using var reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Table")
                {
                    var table = ReadTable(reader, tables);
                    if (table is not null)
                    {
                        result.Add(table);
                    }
                }
            }

            return [.. result];
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    private TableSettings? ReadTable(XmlReader reader, TableSettings[] tables)
    {
        string? name = null;
        LineWidth? raster = null;
        LineWidth? rahmen = null;
        HeaderPosition? headerPos = null;

        // Attribute lesen
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            switch (reader.Name)
            {
                case nameof(TableSettings.SchemaName):
                    name = reader.Value;
                    break;
                case nameof(TableSettings.RasterStrichStärke):
                    raster = Enum.TryParse<LineWidth>(reader.Value, out var rasterVal) ? rasterVal : null;
                    break;
                case nameof(TableSettings.RahmenStrichStärke):
                    rahmen = Enum.TryParse<LineWidth>(reader.Value, out var rahmenVal) ? rahmenVal : null;
                    break;
                case nameof(TableSettings.HeaderPosition):
                    headerPos = Enum.TryParse<HeaderPosition>(reader.Value, out var pos) ? pos : null;
                    break;
            }
        }
        reader.MoveToElement();

        if (name is null)
        {
            return null;
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
        }

        // Sub-Elemente lesen
        if (reader.IsEmptyElement)
            return table;

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Table")
                break;

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == nameof(TableSettings.HeaderFormat))
                {
                    ReadTextFormat(reader, table.HeaderFormat);
                }
                else if (reader.Name == nameof(TableSettings.TextFormat))
                {
                    ReadTextFormat(reader, table.TextFormat);
                }
                else if (reader.Name == "Spalten")
                {
                    this.ReadSpalten(reader, table.Spalten);
                }
            }
        }

        return table;
    }

    private void ReadTextFormat(XmlReader reader, TextFormat format)
    {
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            switch (reader.Name)
            {
                case nameof(TextFormat.Schriftart):
                    format.Schriftart = reader.Value;
                    break;
                case nameof(TextFormat.Schriftstil):
                    format.Schriftstil = reader.Value;
                    break;
                case nameof(TextFormat.Texthöhe):
                    format.Texthöhe = double.TryParse(reader.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var th) ? th : 0.0;
                    break;
                case nameof(TextFormat.Fett):
                    format.Fett = ParseBool(reader.Value);
                    break;
                case nameof(TextFormat.Unterstrichen):
                    format.Unterstrichen = ParseBool(reader.Value);
                    break;
                case nameof(TextFormat.Durchgestrichen):
                    format.Durchgestrichen = ParseBool(reader.Value);
                    break;
                case nameof(TextFormat.Kursiv):
                    format.Kursiv = ParseBool(reader.Value);
                    break;
                case nameof(TextFormat.RgbFarbe):
                    if (int.TryParse(reader.Value, out var rgb))
                    {
                        format.RgbFarbe = rgb;
                    }
                    break;
            }
        }

        reader.MoveToElement();
        // Leeres Element überspringen
        if (!reader.IsEmptyElement)
            reader.Read();
    }

    private void ReadSpalten(XmlReader reader, IEnumerable<SpalteSettings> spalten)
    {
        if (reader.IsEmptyElement)
            return;

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Spalten")
                break;

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Spalte")
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
                        case nameof(SpalteSettings.Name):
                            name = reader.Value;
                            break;
                        case nameof(SpalteSettings.Title):
                            title = reader.Value;
                            break;
                        case nameof(SpalteSettings.SubTitle):
                            subTitle = reader.Value;
                            break;
                        case nameof(SpalteSettings.Visible):
                            visible = ParseBool(reader.Value);
                            break;
                        case nameof(SpalteSettings.AutoBreite):
                            autoBreite = ParseBool(reader.Value);
                            break;
                        case nameof(SpalteSettings.Breite):
                            breite = double.TryParse(reader.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var breiteVal) ? breiteVal : null;
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }
                if (spalten.FirstOrDefault(o => o.Name == name) is SpalteSettings spalte)
                {
                    spalte.Title = title ?? spalte.Title;
                    spalte.SubTitle = subTitle ?? spalte.SubTitle;
                    spalte.Visible= visible ?? spalte.Visible;
                    spalte.AutoBreite = autoBreite ?? spalte.AutoBreite;
                    spalte.Breite = breite ?? spalte.Breite;
                }

                reader.MoveToElement();
            }
        }
    }

    private static bool ParseBool(string value)
        => value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1";
}