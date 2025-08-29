// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using System;
using System.Collections.Generic;
using System.IO;

public class SettingsLoader
{
    private Dictionary<string, TableSettings> tableSettingsCache = new(StringComparer.Ordinal);

    private Dictionary<string, FormatSettings> formatSettingsCache = new(StringComparer.Ordinal);

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

    internal TableSettings GetTableSettings(string templateName, SheetFormat format)
    {
        return new TableSettings();
    }

    internal FormatSettings GetFormat(SheetFormat format, TableSettings tableSettings)
    {
        // ToDo: Load FromSettings
        return new FormatSettings()
        {
            SheetFormat = format,
            InsertPoint = TableInsertPoint.TopRight,
            MaxZone = GetMaxZone(format),
        };
    }

    private string GetMaxZone(SheetFormat sf)
        => sf switch
        {
            SheetFormat.A4V => "F4",
            SheetFormat.A4 => "D6",
            SheetFormat.A3 => "F8",
            SheetFormat.A2 => "H12",
            SheetFormat.A1 => "M16",
            _ => "R24",
        };
}
