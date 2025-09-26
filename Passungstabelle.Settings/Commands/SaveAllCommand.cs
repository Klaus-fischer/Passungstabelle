// <copyright file="SaveAllCommand" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Passungstabelle.Settings;

internal class SaveAllCommand(MainViewModel viewModel) : ICommand
{
    private readonly MainViewModel viewModel = viewModel;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        this.ExportSettings();
    }

    private void ExportSettings()
    {
        var general = (GeneralSettings)this.viewModel.General;
        if (general.UseCentralLocation)
        {
            try
            {
                ExportSettingsTo(general.CentralLocation, general);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Einstellungen konnten nicht nach {viewModel.General.CentralLocation} exportiert werden.\n" + ex.Message, "Fehler bei ExportSettings");
            }
        }

        RegistryService.UpdateRegistry(general);
        ExportSettingsTo(DefaultLocations.CommonLocalSettingsPath, general);
        ExportUserSettingsTo(DefaultLocations.UserLocalSettingsPath, general);
    }

    private void ExportSettingsTo(string outputPath, GeneralSettings general)
    {
        var formats = viewModel.Format.FormatCollection.ToArray();
        var tables = viewModel.Table.TableCollection.ToArray();

        SettingsWriter.WriteSettings(general, formats, tables, outputPath, userSettingsOnly: false);
    }

    private void ExportUserSettingsTo(string outputPath, GeneralSettings general)
    {
        SettingsWriter.WriteSettings(general, [], [], outputPath, userSettingsOnly: true);
    }
}

