// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            this.OverrideLocalSettings(centralLocation, DefaultLocations.GeneralSettingsFilename);
            this.OverrideLocalSettings(centralLocation, DefaultLocations.TableSettingsFilename);
            this.OverrideLocalSettings(centralLocation, DefaultLocations.FormatSettingsFilename);
        }

        this.LoadLocalSettings();
        RegistryService.UpdateRegistry(this.Settings); // update central location in registry
    }

    private void OverrideLocalSettings(string centralLocation, string filename)
    {
        var centralFilename = Path.Combine(centralLocation, filename);
        var localFilename = Path.Combine(DefaultLocations.CommonLocalSettingsPath, filename);

        if (!GeneralSettingsReader.TryGetExportDate(centralFilename, out var centralExportDate))
        {
            // no settings date --> no central settings found
            return;
        }

        if (GeneralSettingsReader.TryGetExportDate(localFilename, out var localExportDate)
            && centralExportDate <= localExportDate)
        {
            // central settings are older or equal to local settings
            return;
        }

        File.Copy(centralFilename, localFilename, true);
    }

    private void LoadLocalSettings()
    {
        this.LoadGeneralSettings();
        this.LoadTableSettings();
        this.LoadFormatSettings();
        this.LoadTemplateSettings();
    }

    private void LoadGeneralSettings()
    {
        var settings = new GeneralSettings();
        GeneralSettingsReader.ReadGeneralSettings(DefaultLocations.CommonLocalSettingsPath, ref settings);

        // user settings overrides common settings
        GeneralSettingsReader.ReadGeneralSettings(DefaultLocations.UserLocalSettingsPath, ref settings);
        this.Settings = settings;
    }

    private void LoadTableSettings()
    {
        List<TableSettings> tableSettings = [];
        TableSettingsReader.ReadTableSettings(DefaultLocations.CommonLocalSettingsPath, tableSettings);
        if (tableSettings.Count == 0)
        {
            tableSettings.Add(new TableSettings());
        }

        tableSettings.ForEach(t => this.tableSettingsCache[t.SchemaName] = t);
    }

    private void LoadFormatSettings()
    {
        List<FormatSettings> formatSettings = [];
        FormatSettingsReader.ReadFormatSettings(DefaultLocations.CommonLocalSettingsPath, formatSettings);
        if (formatSettings.Count == 0)
        {
            formatSettings.Add(new FormatSettings() { SheetFormat = SheetFormat.All, MaxZone = "" });
        }

        formatSettings.ForEach(f => this.formatSettingsCache[f.Name] = f);
    }

    private void LoadTemplateSettings()
    {
        List<TemplateSettings> templateSettings = [];
        TemplateSettingsReader.ReadTemplateSettings(DefaultLocations.CommonLocalSettingsPath, templateSettings);
        if (templateSettings.Count == 0)
        {
            templateSettings.Add(new TemplateSettings()
            {
                TemplateNamePattern = "*",
                FormatNames = this.formatSettingsCache.Values.Select(o => o.Name).ToArray(),
                TableSchemaName = this.tableSettingsCache.First().Key,
            });
        }

        templateSettings.ForEach(this.templateSettingsCache.Add);
    }

    public bool TryGetTableSettings(string templateName, SheetFormat sheetFormat, out TableSettings table, out FormatSettings format)
    {
        format = null!;
        if (!this.TryFindTemplate(templateName, out var template))
        {
            table = new();
            format = new();
            return false;
        }

        return TryFindTable(template.TableSchemaName, out table)
            && TryFindFormat(template.FormatNames, sheetFormat, out format);
    }

    private bool TryFindTemplate(string templateName, out TemplateSettings templateSettings)
    {
        templateSettings = null!;
        foreach (var template in templateSettingsCache)
        {
            if (!TemplateNameMatches(templateName, template.TemplateNamePattern))
            {
                continue;
            }

            templateSettings = template;
            return true;
        }

        return false;
    }

    private bool TemplateNameMatches(string templateName, string templatePattern)
    {
        var pattern = Regex.Escape(templatePattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".");

        return Regex.IsMatch(templateName, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private bool TryFindTable(string schemaName, out TableSettings tableSettings)
    {
        if (!this.tableSettingsCache.TryGetValue(schemaName, out tableSettings))
        {
            tableSettings = new TableSettings { SchemaName = schemaName };
            return false;
        }

        return true;
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
