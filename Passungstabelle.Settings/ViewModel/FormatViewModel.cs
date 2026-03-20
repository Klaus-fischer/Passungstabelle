// <copyright file="FormatViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Passungstabelle.Settings;

public class FormatViewModel : BaseViewModel, ISelectedItemHost<FormatSettings>
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
    private FormatSettings selectedFormat = new();

    public FormatViewModel()
    {
        this.AddCommand = new AddToSelectionCommand<FormatSettings>(this);
        this.UpdateCommand = new UpdateSelectionCommand<FormatSettings>(this);
        this.DeleteCommand = new DeleteSelectedCommand<FormatSettings>(this);
    }

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

    public ICommand AddCommand { get; }

    public ICommand UpdateCommand { get; }

    public ICommand DeleteCommand { get; }

    public ObservableCollection<FormatSettings> FormatCollection { get; } = new ObservableCollection<FormatSettings>();

    public FormatSettings SelectedItem
    {
        get => this.selectedFormat;
        set => this.SelectFormat(value);
    }

    IList<FormatSettings> ISelectedItemHost<FormatSettings>.Collection => this.FormatCollection;

    FormatSettings ISelectedItemHost<FormatSettings>.CreateItem(out int? index)
    {
        index = null;
        return new()
        {
            InsertPoint = this.InsertPoint,
            Name = this.Name,
            MaxZone = this.MaxZone,
            SheetFormat = this.SheetFormat,
            Offset = new Vector(this.OffsetX, this.OffsetY),
            Margin = new Thickness(this.MarginBottom, this.MarginLeft, this.MarginRight, this.MarginTop),
        };
    }

    bool ISelectedItemHost<FormatSettings>.PropertiesHasChanged(FormatSettings item)
    {
        return this.InsertPoint != item.InsertPoint
            || this.Name != item.Name
            || this.OffsetX != item.Offset.X
            || this.OffsetY != item.Offset.Y
            || this.MarginBottom != item.Margin.Bottom
            || this.MarginLeft != item.Margin.Left
            || this.MarginRight != item.Margin.Right
            || this.MarginTop != item.Margin.Top
            || this.MaxZone != item.MaxZone
            || this.SheetFormat != item.SheetFormat;
    }

    public void InitializeFormats(IEnumerable<FormatSettings> formats)
    {
        this.FormatCollection.Clear();
        foreach (var format in formats)
        {
            this.FormatCollection.Add(format);
        }

        this.SelectedItem = this.FormatCollection.First();
    }

    private void SelectFormat(FormatSettings value)
    {
        this.selectedFormat = value;
        OnPropertyChanged(nameof(SelectedItem));

        if (value is null)
        {
            return;
        }

        this.InsertPoint = value.InsertPoint;
        this.Name = value.Name;
        this.OffsetX = value.Offset.X;
        this.OffsetY = value.Offset.Y;
        this.MarginBottom = value.Margin.Bottom;
        this.MarginLeft = value.Margin.Left;
        this.MarginRight = value.Margin.Right;
        this.MarginTop = value.Margin.Top;
        this.MaxZone = value.MaxZone;
        this.SheetFormat = value.SheetFormat;
    }
}