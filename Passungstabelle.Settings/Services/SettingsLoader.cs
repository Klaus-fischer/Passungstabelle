// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;

public class SettingsLoader
{
    public GeneralSettings Settings { get; } = new GeneralSettings();

    public void ReloadSettings()
    {

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
            _=> "R24",
        };
}
