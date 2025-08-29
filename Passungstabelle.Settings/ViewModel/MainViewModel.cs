// <copyright file="MainViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using Passungstabelle.CSharp;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        this.SaveAllCommand = new SaveAllCommand(this);
        this.Template = new TemplateViewModel(Table.TableCollection, Format.FormatCollection);
    }

    public GeneralViewModel General { get; } = new();

    public FormatViewModel Format { get; } = new();

    public TableViewModel Table { get; } = new();

    public TemplateViewModel Template { get; }

    public void Initialize()
    {
        var loader = new SettingsLoader();
        loader.ReloadSettings();
        this.General.ParseValues(loader.Settings);
        this.Format.InitializeFormats(loader.formatSettingsCache.Values);
        this.Table.InitializeTableCollection(loader.tableSettingsCache.Values);
        this.Template.InitializeTemplates(loader.templateSettingsCache);
    }

    public ICommand SaveAllCommand { get; }
}

