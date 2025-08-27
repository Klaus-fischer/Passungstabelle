// <copyright file="FormatViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;

namespace Passungstabelle.Settings;

public class FormatViewModel : BaseViewModel
{
    private string name = string.Empty;
    private SheetFormat sheetFormat = default;
    private TableInsertPoint insertPoint = TableInsertPoint.TopRight;
    private double offsetX;
    private double offsetY;
    private double marginTop = default;
    private double marginLeft = default;
    private double marginRight = default;
    private double marginBottom = default;

    public string Name
    {
        get => this.name;
        set => this.Set(ref name, value);
    }

    public SheetFormat SheetFormat
    {
        get => this.sheetFormat;
        set => this.Set(ref sheetFormat, value);
    }

    public TableInsertPoint InsertPoint
    {
        get => this.insertPoint;
        set => this.Set(ref insertPoint, value);
    }

    public double OffsetX { get => offsetX; set => Set(ref offsetX, value); }

    public double OffsetY { get => offsetY; set => Set(ref offsetY, value); }

    public string MaxZone
    {
        get => this._MaxZone;
        set => this.Set(ref _MaxZone, value, alsoNotify: [nameof(TopRightZone), nameof(BottomLeftZone)]);
    }

    private string _MaxZone = "H6";

    public string TopRightZone => _MaxZone.Length > 0 ? string.Concat("A", _MaxZone.AsSpan(1)) : "";
    public string BottomLeftZone => _MaxZone.Length > 0 ? string.Concat(_MaxZone[0], "1") : "";

    public double MarginTop
    {
        get => this.marginTop;
        set => this.Set(ref marginTop, value);
    }

    public double MarginLeft
    {
        get => this.marginLeft;
        set => this.Set(ref marginLeft, value);
    }

    public double MarginRight
    {
        get => this.marginRight;
        set => this.Set(ref marginRight, value);
    }

    public double MarginBottom
    {
        get => this.marginBottom;
        set => this.Set(ref marginBottom, value);
    }
}

