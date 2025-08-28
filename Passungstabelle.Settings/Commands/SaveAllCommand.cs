// <copyright file="SaveAllCommand" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Linq;
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
        var formats = viewModel.Format.FormatCollection.ToArray();
        var formatWriter = new FormatSettingWriter();
        formatWriter.WriteFormatSettings(formats, DefaultLocations.CommonLocalSettingsPath);
    }

    private void ExportSettingsTo(string outputPath) 
    {
        var generalWriter = new GeneralSettingsWriter();
        var general = (GeneralSettings)this.viewModel.General;
        generalWriter.WriteGeneralSettings(general, outputPath, userSettingsOnly: false);

        var formats = viewModel.Format.FormatCollection.ToArray();
        var formatWriter = new FormatSettingWriter();
        formatWriter.WriteFormatSettings(formats, outputPath);
    }
}

