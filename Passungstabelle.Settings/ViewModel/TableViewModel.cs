// <copyright file="TableViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

public class TableViewModel : BaseViewModel
{
    private string schemaName = "Default";
    private LineWidth rasterStrichStärke = LineWidth.Dünn;
    private LineWidth rahmenStrichStärke = LineWidth.Dick;
    private TableSettings? selectedTable = new();
    private HeaderPosition headerPosition = default;
    private SpalteSettings[] spalten;
    private SpalteSettings selectedSpalte = new();

    public TableViewModel()
    {
        this.AddCommand = new RelayCommand(this.OnAddTable);
        this.UpdateCommand = new RelayCommand(this.OnUpdateTable, this.CanUpdateTable);
        this.DeleteCommand = new RelayCommand(this.OnDeleteTable);
        this.SelectedTable = new TableSettings();
        this.TableCollection.Add(this.SelectedTable);
    }

    public ICommand AddCommand { get; }

    public ICommand UpdateCommand { get; }

    public ICommand DeleteCommand { get; }

    public string SchemaName { get => this.schemaName; set => this.Set(ref this.schemaName, value); }

    public LineWidth RasterStrichStärke { get => this.rasterStrichStärke; set => this.Set(ref this.rasterStrichStärke, value); }

    public LineWidth RahmenStrichStärke { get => this.rahmenStrichStärke; set => this.Set(ref this.rahmenStrichStärke, value); }

    public TextViewModel HeaderFormat { get; } = new();

    public TextViewModel TextFormat { get; } = new();

    public SpalteSettings[] Spalten { get => this.spalten; set => this.Set(ref this.spalten, value); }

    public SpalteSettings Spalte { get => this.selectedSpalte; set => this.Set(ref this.selectedSpalte, value); }

    public HeaderPosition HeaderPosition
    {
        get => this.headerPosition;
        set => this.Set(ref this.headerPosition, value);
    }

    public ObservableCollection<TableSettings> TableCollection { get; } = new ObservableCollection<TableSettings>();

    public TableSettings? SelectedTable
    {
        get => this.selectedTable;
        set => this.SelectTable(value);
    }

    private void SelectTable(TableSettings? value)
    {
        this.selectedTable = value;
        this.OnPropertyChanged(nameof(this.SelectedTable));
        CommandManager.InvalidateRequerySuggested();

        if (value is null)
        {
            return;
        }

        this.SchemaName = value.SchemaName;
        this.RasterStrichStärke = value.RasterStrichStärke;
        this.RahmenStrichStärke = value.RahmenStrichStärke;
        this.HeaderPosition = value.HeaderPosition;
        this.HeaderFormat.Parse(value.HeaderFormat);
        this.TextFormat.Parse(value.TextFormat);

        this.Spalten = [.. this.CopySpalten(value.Spalten)];
        this.Spalte = this.Spalten[0];
    }

    private void OnAddTable()
    {
        var Table = this.CreateTable();

        this.TableCollection.Add(Table);
        this.SelectedTable = Table;
    }

    public bool CanUpdateTable() => this.selectedTable is TableSettings table && this.TableCollection.Contains(table);

    private void OnUpdateTable()
    {
        if (this.selectedTable is null)
        {
            return;
        }

        var index = this.TableCollection.IndexOf(this.selectedTable);
        if (index < 0)
        {
            return;
        }

        var Table = this.CreateTable();
        this.TableCollection.Insert(index, Table);
        this.TableCollection.Remove(this.selectedTable);
        this.SelectedTable = Table;
    }

    private void OnDeleteTable()
    {
        if (this.selectedTable is null)
        {
            return;
        }

        var index = this.TableCollection.IndexOf(this.selectedTable);
        this.TableCollection.Remove(this.selectedTable);

        if (this.TableCollection.Count == 0)
        {
            this.SelectedTable = null;
        }
        else if (index < this.TableCollection.Count)
        {
            this.SelectedTable = this.TableCollection[index];
        }
        else
        {
            this.SelectedTable = this.TableCollection[^1];
        }
    }

    private IEnumerable<SpalteSettings> CopySpalten(IEnumerable<SpalteSettings> spalten)
    {
        foreach (var spalte in spalten)
        {
            yield return new SpalteSettings()
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

    private TableSettings CreateTable()
    {
        var result = new TableSettings()
        {
            SchemaName = this.SchemaName,
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
}
