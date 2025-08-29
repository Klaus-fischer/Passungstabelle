// <copyright file="TemplateViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

public class TemplateViewModel : BaseViewModel
{
    private readonly ObservableCollection<FormatSettings> formats;
    private string templatePattern = "*";
    private TemplateSettings selectedTemplate;

    public TemplateViewModel(ObservableCollection<TableSettings> tables, ObservableCollection<FormatSettings> formats)
    {
        this.Tables = tables;
        this.formats = formats;
        formats.CollectionChanged += OnFormatCollectionChanged;
    }

    public string TemplatePattern
    {
        get => this.templatePattern;
        set => this.Set(ref templatePattern, value);
    }

    public ObservableCollection<FormatSelector> Formats { get; } = new();

    public ObservableCollection<TableSettings> Tables { get; }

    public ObservableCollection<TemplateSettings> Templates { get; } = new();

    public TemplateSettings SelectedTemplate
    {
        get => this.selectedTemplate;
        set
        {
            this.selectedTemplate = value;

            this.UpdateFormatSelection();

            OnPropertyChanged();
        }
    }

    public void InitializeTemplates(IEnumerable<TemplateSettings> templates)
    {
        this.Templates.Clear();
        foreach (var template in templates)
        {
            this.Templates.Add(template);
        }

        this.SelectedTemplate = this.Templates.First();
    }

    private void UpdateFormatSelection()
    {
        foreach (var formatSelector in this.Formats)
        {
            formatSelector.IsSelected = this.selectedTemplate?.FormatNames.Contains(formatSelector.Format.Name) ?? false;
        }
    }

    private void OnFormatCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.Formats.Clear();
        foreach (var format in this.formats)
        {
            this.Formats.Add(new FormatSelector(format));
        }

        this.UpdateFormatSelection();
    }
}

public class FormatSelector(FormatSettings format) : BaseViewModel
{
    private FormatSettings format = format;
    private bool _IsSelected = default;

    public FormatSettings Format
    {
        get => this.format;
        set => this.Set(ref format, value);
    }

    public bool IsSelected
    {
        get => this._IsSelected;
        set => this.Set(ref _IsSelected, value);
    }
}
