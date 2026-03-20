// <copyright file="TemplateViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

public class TemplateViewModel : BaseViewModel, ISelectedItemHost<TemplateSettingsViewModel>
{
    private readonly ObservableCollection<FormatSettings> formats;
    private string selectedTemplatePattern = "*";
    private TemplateSettingsViewModel selectedTemplate = new();
    private string _SelectedTableSchemaName = string.Empty;

    public TemplateViewModel(ObservableCollection<TableSettings> tables, ObservableCollection<FormatSettings> formats)
    {
        this.Tables = tables;
        this.formats = formats;
        this.formats.CollectionChanged += OnFormatCollectionChanged;
        this.AddCommand = new AddToSelectionCommand<TemplateSettingsViewModel>(this);
        this.UpdateCommand = new UpdateSelectionCommand<TemplateSettingsViewModel>(this);
        this.DeleteCommand = new DeleteSelectedCommand<TemplateSettingsViewModel>(this);

        this.MoveSelectedUpCommand = new MoveSelectedCommand<TemplateSettingsViewModel>(
            MoveDirection.Up, this);
        this.MoveSelectedDownCommand = new MoveSelectedCommand<TemplateSettingsViewModel>(
            MoveDirection.Down, this);
    }

    public string SelectedTableSchemaName
    {
        get => this._SelectedTableSchemaName;
        set => this.Set(ref _SelectedTableSchemaName, value);
    }

    public string SelectedTemplatePattern
    {
        get => this.selectedTemplatePattern;
        set => this.Set(ref selectedTemplatePattern, value);
    }

    public ObservableCollection<FormatSelector> Formats { get; } = new();

    public ObservableCollection<TableSettings> Tables { get; }

    public ObservableCollection<TemplateSettingsViewModel> Templates { get; } = new();

    public TemplateSettingsViewModel SelectedTemplate
    {
        get => this.selectedTemplate;
        set
        {
            this.selectedTemplate = value;

            this.UpdateFormatSelection();

            this.OnPropertyChanged();
            this.OnPropertyChanged(nameof(ISelectedItemHost<TemplateSettingsViewModel>.SelectedItem));
        }
    }

    public ICommand AddCommand { get; }

    public ICommand UpdateCommand { get; }

    public ICommand DeleteCommand { get; }

    public ICommand MoveSelectedUpCommand { get; }

    public ICommand MoveSelectedDownCommand { get; }

    TemplateSettingsViewModel ISelectedItemHost<TemplateSettingsViewModel>.SelectedItem
    {
        get => this.SelectedTemplate;
        set => this.SelectedTemplate = value;
    }

    public void InitializeTemplates(IEnumerable<TemplateSettings> templates)
    {
        this.Templates.Clear();
        foreach (var template in templates.Select(TemplateSettingsViewModel.FromModel))
        {
            this.Templates.Add(template);
        }

        this.SelectedTemplate = this.Templates.First();
    }

    IList<TemplateSettingsViewModel> ISelectedItemHost<TemplateSettingsViewModel>.Collection => this.Templates;

    TemplateSettingsViewModel ISelectedItemHost<TemplateSettingsViewModel>.CreateItem(out int? insertIndex)
    {
        insertIndex = this.Templates.LastOrDefault()?.TemplateNamePattern == "*" ? Templates.Count - 1 : null;

        return new TemplateSettingsViewModel()
        {
            TemplateNamePattern = this.SelectedTemplatePattern,
            TableSchemaName = this.SelectedTableSchemaName,
            FormatNames = [.. this.Formats.Where(o => o.IsSelected).Select(o => o.Format.Name)]
        };
    }

    bool ISelectedItemHost<TemplateSettingsViewModel>.PropertiesHasChanged(TemplateSettingsViewModel item)
    {
        var formatNamesEquals = item.FormatNames
            .ToHashSet()
            .SetEquals(this.Formats.Where(o => o.IsSelected).Select(o => o.Format.Name));

        return item.TemplateNamePattern != this.SelectedTemplatePattern
            || item.TableSchemaName != this.SelectedTableSchemaName
            || !formatNamesEquals;
    }

    private void UpdateFormatSelection()
    {
        this.SelectedTableSchemaName = this.SelectedTemplate?.TableSchemaName ?? string.Empty;
        this.SelectedTemplatePattern = this.SelectedTemplate?.TemplateNamePattern ?? string.Empty;

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