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
        GeneralSettingsWriter.WriteGeneralSettings(general, outputPath, userSettingsOnly: false);

        var formats = viewModel.Format.FormatCollection.ToArray();
        FormatSettingWriter.WriteFormatSettings(formats, outputPath);

        var tables = viewModel.Table.TableCollection.ToArray();
        TableSettingsWriter.WriteTableSettings(tables, outputPath);

        var templates = viewModel.Template.Templates.ToArray();
        TemplateSettingWriter.WriteTemplateSettings(templates, outputPath);
    }

    private void ExportUserSettingsTo(string outputPath, GeneralSettings general)
    {
        GeneralSettingsWriter.WriteGeneralSettings(general, outputPath, userSettingsOnly: true);
    }
}

