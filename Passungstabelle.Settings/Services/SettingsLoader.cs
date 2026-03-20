// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class SettingsLoader
{
    internal readonly Dictionary<string, TableSettings> tableSettingsCache = new(StringComparer.Ordinal);

    internal readonly Dictionary<string, FormatSettings> formatSettingsCache = new(StringComparer.Ordinal);

    internal readonly List<TemplateSettings> templateSettingsCache = new();

    public GeneralSettings Settings { get; private set; } = new GeneralSettings();

    public void ReloadSettings()
    {
        if (RegistryService.TryGetCentralLocation(out var centralLocation))
        {
            this.OverrideLocalSettings(centralLocation, DefaultLocations.SettingsFilename);
        }

        this.LoadLocalSettings();
        RegistryService.UpdateRegistry(this.Settings); // update central location in registry
    }

    private void OverrideLocalSettings(string centralLocation, string filename)
    {
        var centralFilename = Path.Combine(centralLocation, filename);
        var localFilename = Path.Combine(DefaultLocations.CommonLocalSettingsPath, filename);

        if (!SettingsReader.TryGetExportDate(centralFilename, out var centralExportDate))
        {
            // no settings date --> no central settings found
            return;
        }

        if (SettingsReader.TryGetExportDate(localFilename, out var localExportDate)
            && centralExportDate <= localExportDate)
        {
            // central settings are older or equal to local settings
            return;
        }

        File.Copy(centralFilename, localFilename, true);
    }

    private void LoadLocalSettings()
    {
        var settings = new GeneralSettings();
        List<TableSettings> tableSettings = [];
        List<FormatSettings> formatSettings = [];
        List<TemplateSettings> templateSettings = [];

        SettingsReader.ReadGeneralSettings(DefaultLocations.CommonLocalSettingsPath, ref settings, ref formatSettings, ref tableSettings);

        // user settings overrides common settings
        SettingsReader.ReadGeneralSettings(DefaultLocations.UserLocalSettingsPath, ref settings, ref formatSettings, ref tableSettings);
        this.Settings = settings;

        if (tableSettings.Count == 0)
        {
            tableSettings.Add(new TableSettings());
        }

        tableSettings.ForEach(t => this.tableSettingsCache[t.Name] = t);

        if (formatSettings.Count == 0)
        {
            formatSettings.Add(new FormatSettings() { SheetFormat = SheetFormat.All, MaxZone = "" });
        }

        formatSettings.ForEach(f => this.formatSettingsCache[f.Name] = f);

    }

    public bool TryGetTableSettings(string templateName, SheetFormat sheetFormat, out TableSettings table, out FormatSettings format)
    {
        format = new FormatSettings() { SheetFormat = SheetFormat.All };

        return TryFindTable(templateName, out table)
            && TryFindFormat(table.FormatNames, sheetFormat, out format);
    }

    private bool TryFindTable(string templateName, out TableSettings tableSettings)
    {
        tableSettings = null!;
        foreach (var table in tableSettingsCache.Values)
        {
            if (!TemplateNameMatches(templateName, table.TemplateNamePattern))
            {
                continue;
            }

            tableSettings = table;
            break;
        }

        if (tableSettings is null)
        {
            tableSettings = new TableSettings();
            return false;
        }

        return true;
    }

    private bool TemplateNameMatches(string templateName, string templatePattern)
    {
        var pattern = Regex.Escape(templateName)
            .Replace("\\*", ".*")
            .Replace("\\?", ".");

        return Regex.IsMatch(templatePattern, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private bool TryFindFormat(string[] formatNames, SheetFormat sheetFormat, out FormatSettings formatSettings)
    {
        foreach (var formatName in formatNames)
        {
            if (this.formatSettingsCache.TryGetValue(formatName, out var format))
            {
                if (format.SheetFormat == SheetFormat.All || format.SheetFormat == sheetFormat)
                {
                    formatSettings = format;
                    return true;
                }
            }
        }

        formatSettings = new FormatSettings
        {
            SheetFormat = sheetFormat,
            MaxZone = string.Empty,
        };

        return false;
    }
}
