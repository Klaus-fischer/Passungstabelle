// <copyright file="SettingsLoader" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

internal class SettingsLoader
{
    public GeneralSettings Settings { get; } = new GeneralSettings();
    public TableSettings TableSettings { get; } = new TableSettings();

    public void ReloadSettings()
    {

    }
}
