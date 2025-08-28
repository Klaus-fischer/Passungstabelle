// <copyright file="GeneralViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Globalization;

namespace Passungstabelle.Settings;

public class GeneralViewModel : BaseViewModel
{
    private string language = CultureInfo.CurrentUICulture.Name;
    private bool useCentralLocation = default;
    private string centralLocation = @"C:\PDM\Einstellungen\Passungstabelle";
    private bool usePlusSign = default;
    private bool onlyAtFirstSheet = default;
    private bool removeAtAllPages = default;
    private bool createLogFile = default;
    private string logFilePath = "%temp%/Passungstabelle.log";
    private bool useEvents = default;
    private bool recalculateAfterRebuild = default;
    private bool recalculateBeforeSave = default;
    private bool _SuppressMessages = default;

    public ObservableCollection<string> AvailableLanguages { get; } = new(["de-DE", "en-US"]);

    public string Language
    {
        get => this.language;
        set
        {
            this.Set(ref language, value);
            ResourceLocater.Current.ChangeLanguage(value);
        }
    }

    public bool UseCentralLocation
    {
        get => this.useCentralLocation;
        set => this.Set(ref useCentralLocation, value);
    }

    public string CentralLocation
    {
        get => this.centralLocation;
        set => this.Set(ref centralLocation, value);
    }

    public bool UsePlusSign
    {
        get => this.usePlusSign;
        set => this.Set(ref usePlusSign, value);
    }

    public bool OnlyAtFirstSheet
    {
        get => this.onlyAtFirstSheet;
        set => this.Set(ref onlyAtFirstSheet, value);
    }

    public bool RemoveAtAllPages
    {
        get => this.removeAtAllPages;
        set => this.Set(ref removeAtAllPages, value);
    }

    public bool CreateLogFile
    {
        get => this.createLogFile;
        set => this.Set(ref createLogFile, value);
    }

    public string LogFilePath
    {
        get => this.logFilePath;
        set => this.Set(ref logFilePath, value);
    }

    public bool UseEvents
    {
        get => this.useEvents;
        set => this.Set(ref useEvents, value, alsoNotify: [nameof(RecalculateAfterRebuild), nameof(RecalculateBeforeSave)]);
    }

    public bool RecalculateAfterRebuild
    {
        get => this.useEvents && this.recalculateAfterRebuild;
        set
        {
            if (value)
            {
                RecalculateBeforeSave = false;
            }
            Set(ref recalculateAfterRebuild, value);
        }
    }

    public bool RecalculateBeforeSave
    {
        get => this.useEvents && this.recalculateBeforeSave;
        set
        {
            if (value)
            {
                RecalculateAfterRebuild = false;
            }

            this.Set(ref recalculateBeforeSave, value);
        }
    }

    public bool SuppressMessages
    {
        get => this._SuppressMessages;
        set => this.Set(ref _SuppressMessages, value);
    }

    internal void ParseValues(GeneralSettings settings)
    {
        this.Language = settings.Language;
        this.UseCentralLocation = settings.UseCentralLocation;
        this.CentralLocation = settings.CentralLocation;
        this.CreateLogFile = settings.CreateLogFile;
        this.LogFilePath = settings.LogFilePath;
        this.OnlyAtFirstSheet = settings.OnlyAtFirstSheet;
        this.RemoveAtAllPages = settings.RemoveAtAllPages;
        this.UseEvents = settings.UseEvents;
        this.RecalculateBeforeSave = settings.RecalculateBeforeSave;
        this.RecalculateAfterRebuild = settings.RecalculateAfterRebuild;
        this.UsePlusSign = settings.UsePlusSign;
        this.SuppressMessages = settings.SuppressMessages;
    }

    public static explicit operator GeneralSettings(GeneralViewModel viewModel)
        => new GeneralSettings()
        {
            Language = viewModel.Language,
            UseCentralLocation = viewModel.UseCentralLocation,
            CentralLocation = viewModel.CentralLocation,
            CreateLogFile = viewModel.CreateLogFile,
            LogFilePath = viewModel.LogFilePath,
            OnlyAtFirstSheet = viewModel.OnlyAtFirstSheet,
            RemoveAtAllPages = viewModel.RemoveAtAllPages,
            UseEvents = viewModel.UseEvents,
            RecalculateBeforeSave = viewModel.RecalculateBeforeSave,
            RecalculateAfterRebuild = viewModel.RecalculateAfterRebuild,
            UsePlusSign = viewModel.UsePlusSign,
            SuppressMessages = viewModel.SuppressMessages,
        };
}
