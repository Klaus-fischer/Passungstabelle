// <copyright file="MainViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

internal class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
    }

    public GeneralViewModel General { get; } = new();

    public FormatViewModel Format { get; } = new();
}

