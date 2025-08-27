// <copyright file="MainViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.ObjectModel;

internal class MainViewModel : BaseViewModel
{
    private FormatViewModel? format = new();
    private FormatSettings? selectedFormat = new();

    /// <summary>
    /// Gets or sets the ...
    /// </summary>
    public FormatViewModel? Format
    {
        get => this.format;
        set => this.Set(ref format, value);
    }


    public ObservableCollection<FormatSettings> FormatSettings { get; } = new ObservableCollection<FormatSettings>();

    /// <summary>
    /// Gets or sets the ...
    /// </summary>
    public FormatSettings? SelectedFormat
    {
        get => this.selectedFormat;
        set => this.SelectFormat(value);
    }

    private void SelectFormat(FormatSettings? value)
    {
        this.selectedFormat = value;
        OnPropertyChanged(nameof(SelectedFormat));

        if (value is null)
        {
            this.Format = null;
            return;
        }

        this.Format = new FormatViewModel
        {
            InsertPoint = value.InsertPoint,
            Name = value.Name,
            OffsetX = value.Offset.X,
            OffsetY = value.Offset.Y,
            MarginBottom = value.Margin.Bottom,
            MarginLeft = value.Margin.Left,
            MarginRight = value.Margin.Right,
            MarginTop = value.Margin.Top,
            MaxZone = value.MaxZone,
        };
    }
}

