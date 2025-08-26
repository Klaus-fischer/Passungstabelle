// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

internal class SettingsLoader
{
    public GeneralSettings Settings { get; } = new GeneralSettings();

    public void ReloadSettings()
    {

    }

    internal TableSettings GetTableSettings(ISheet sheet)
    {
        return new TableSettings();
    }

    internal FormatSettings GetFormat(ISheet sheet, TableSettings tableSettings)
    {
        var format = GetSheetFormat(sheet);

        // ToDo: Load FromSettings
        return new FormatSettings()
        {
            Format = format,
            InsertPoint = Einfügepunkt.TopRight,
            MaxZone = GetMaxZone(format),
        };
    }

    private SheetFormat GetSheetFormat(ISheet sheet)
    {
        var properties = sheet.GetProperties2().AsArrayOfType<double>();

        var result = (swDwgPaperSizes_e)properties[0] switch
        {
            swDwgPaperSizes_e.swDwgPaperA0size => SheetFormat.A0,
            swDwgPaperSizes_e.swDwgPaperA1size => SheetFormat.A1,
            swDwgPaperSizes_e.swDwgPaperA2size => SheetFormat.A2,
            swDwgPaperSizes_e.swDwgPaperA3size => SheetFormat.A3,
            swDwgPaperSizes_e.swDwgPaperA4size => SheetFormat.A4,
            swDwgPaperSizes_e.swDwgPaperA4sizeVertical => SheetFormat.A4V,
            _ => SheetFormat.All,
        };

        if (result == SheetFormat.All)
        {
            result = properties[5] switch
            {
                < 0.210 + 0.005 => SheetFormat.A4V,
                < 0.297 + 0.005 => SheetFormat.A4,
                < 0.420 + 0.005 => SheetFormat.A3,
                < 0.594 + 0.005 => SheetFormat.A2,
                < 0.841 + 0.005 => SheetFormat.A1,
                _ => SheetFormat.A0,
            };
        }

        return result;
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
