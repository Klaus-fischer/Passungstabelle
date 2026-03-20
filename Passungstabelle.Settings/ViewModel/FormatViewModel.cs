// <copyright file="FormatViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        this.AddCommand = new RelayCommand(OnAddFormat);
        this.UpdateCommand = new RelayCommand(OnUpdateFormat, CanUpdateFormat);
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

    private void OnAddFormat()
    {
        var format = this.CreateFormat();

        this.FormatCollection.Add(format);
        this.SelectedItem = format;
    }

    private bool CanUpdateFormat()
    {
        var selected = this.SelectedItem;

        return this.InsertPoint != selected.InsertPoint
            || this.Name != selected.Name
            || this.OffsetX != selected.Offset.X
            || this.OffsetY != selected.Offset.Y
            || this.MarginBottom != selected.Margin.Bottom
            || this.MarginLeft != selected.Margin.Left
            || this.MarginRight != selected.Margin.Right
            || this.MarginTop != selected.Margin.Top
            || this.MaxZone != selected.MaxZone
            || this.SheetFormat != selected.SheetFormat;
    }

    private void OnUpdateFormat()
    {
        if (this.selectedFormat is null)
        {
            return;
        }

        var index = this.FormatCollection.IndexOf(this.selectedFormat);
        var format = this.CreateFormat();

        this.FormatCollection.Insert(index, format);
        this.FormatCollection.Remove(this.selectedFormat);
        this.SelectedItem = format;
    }

    private void OnDeleteFormat()
    {
        if (this.SelectedItem is null)
        {
            var first = this.FormatCollection.FirstOrDefault() ?? new();

            if (!this.FormatCollection.Contains(first))
            {
                this.FormatCollection.Add(first);
            }
            return;
        }

        var index = this.FormatCollection.IndexOf(this.selectedFormat);
        this.FormatCollection.RemoveAt(index);

        var next = this.FormatCollection.Skip(index).FirstOrDefault() ?? new();

        if (!this.FormatCollection.Contains(next))
        {
            this.FormatCollection.Add(next);
        }

        this.SelectedItem = next;
    }

    private FormatSettings CreateFormat() =>
        new()
        {
            InsertPoint = this.InsertPoint,
            Name = this.Name,
            MaxZone = this.MaxZone,
            SheetFormat = this.SheetFormat,
            Offset = new Vector(this.OffsetX, this.OffsetY),
            Margin = new Thickness(this.MarginBottom, this.MarginLeft, this.MarginRight, this.MarginTop),
        };
}
