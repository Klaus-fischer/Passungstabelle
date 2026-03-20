// <copyright file="TableViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

public class TableViewModel : BaseViewModel, ISelectedItemHost<TableSettings>
{
    private string schemaName = "Default";
    private LineWidth rasterStrichStärke = LineWidth.Dünn;
    private LineWidth rahmenStrichStärke = LineWidth.Dick;
    private TableSettings selectedTable = new();
    private HeaderPosition headerPosition = default;
    private ColumnSettings[] spalten = [];
    private ColumnSettings selectedSpalte = new();

    public TableViewModel()
    {
        this.AddCommand = new AddToSelectionCommand<TableSettings>(this);
        this.UpdateCommand = new UpdateSelectionCommand<TableSettings>(this);
        this.DeleteCommand = new DeleteSelectedCommand<TableSettings>(this);
        this.SelectedItem = new TableSettings();
        this.TableCollection.Add(this.SelectedItem);
    }

    public ICommand AddCommand { get; }

    public ICommand UpdateCommand { get; }

    public ICommand DeleteCommand { get; }

    public string Name { get => this.schemaName; set => this.Set(ref this.schemaName, value); }

    public LineWidth RasterStrichStärke { get => this.rasterStrichStärke; set => this.Set(ref this.rasterStrichStärke, value); }

    public LineWidth RahmenStrichStärke { get => this.rahmenStrichStärke; set => this.Set(ref this.rahmenStrichStärke, value); }

    public TextViewModel HeaderFormat { get; } = new();

    public TextViewModel TextFormat { get; } = new();

    public ColumnSettings[] Spalten { get => this.spalten; set => this.Set(ref this.spalten, value); }

    public ColumnSettings Spalte
    {
        get => this.selectedSpalte;
        set => this.Set(ref this.selectedSpalte, value);
    }

    public HeaderPosition HeaderPosition
    {
        get => this.headerPosition;
        set => this.Set(ref this.headerPosition, value);
    }

    public ObservableCollection<TableSettings> TableCollection { get; } = new ObservableCollection<TableSettings>();

    public TableSettings SelectedItem
    {
        get => this.selectedTable;
        set => this.SelectTable(value);
    }

    public void InitializeTableCollection(IEnumerable<TableSettings> tables)
    {
        this.TableCollection.Clear();
        foreach (var table in tables)
        {
            this.TableCollection.Add(table);
        }

        this.SelectedItem = this.TableCollection.First();
    }

    IList<TableSettings> ISelectedItemHost<TableSettings>.Collection => this.TableCollection;

    TableSettings ISelectedItemHost<TableSettings>.CreateItem(out int? insertIndex)
    {
        insertIndex = null;
        var result = new TableSettings()
        {
            Name = this.Name,
            RasterStrichStärke = this.RasterStrichStärke,
            RahmenStrichStärke = this.RahmenStrichStärke,
            HeaderPosition = this.HeaderPosition,
        };

        result.TextFormat = (TextFormat)this.TextFormat;
        result.HeaderFormat = (TextFormat)this.HeaderFormat;

        foreach (var spalte in this.Spalten)
        {
            var target = result.Spalten.FirstOrDefault(o => o.Name == spalte.Name);
            if (target is null)
            {
                continue;
            }

            target.Title = spalte.Title;
            target.SubTitle = spalte.SubTitle;
            target.Visible = spalte.Visible;
            target.Breite = spalte.Breite;
            target.AutoBreite = spalte.AutoBreite;
        }

        return result;
    }

    bool ISelectedItemHost<TableSettings>.PropertiesHasChanged(TableSettings item)
    {
        return this.Name != item.Name
           || this.RasterStrichStärke != item.RasterStrichStärke
           || this.RahmenStrichStärke != item.RahmenStrichStärke
           || this.HeaderPosition != item.HeaderPosition
           || !this.HeaderFormat.Equals(item.HeaderFormat)
           || !this.TextFormat.Equals(item.TextFormat)
           || !this.Spalten.SequenceEqual(item.Spalten);
    }

    private void SelectTable(TableSettings value)
    {
        this.selectedTable = value;
        this.OnPropertyChanged(nameof(this.SelectedItem));
        CommandManager.InvalidateRequerySuggested();

        if (value is null)
        {
            return;
        }

        this.Name = value.Name;
        this.RasterStrichStärke = value.RasterStrichStärke;
        this.RahmenStrichStärke = value.RahmenStrichStärke;
        this.HeaderPosition = value.HeaderPosition;
        this.HeaderFormat.Parse(value.HeaderFormat);
        this.TextFormat.Parse(value.TextFormat);

        this.Spalten = [.. this.CopySpalten(value.Spalten)];
        this.selectedSpalte = this.Spalten[0];
    }

    private IEnumerable<ColumnSettings> CopySpalten(IEnumerable<ColumnSettings> spalten)
    {
        foreach (var spalte in spalten)
        {
            yield return new ColumnSettings()
            {
                Name = spalte.Name,
                Title = spalte.Title,
                SubTitle = spalte.SubTitle,
                Visible = spalte.Visible,
                Breite = spalte.Breite,
                AutoBreite = spalte.AutoBreite,
            };
        }
    }
}
