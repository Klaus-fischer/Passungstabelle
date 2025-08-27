// <copyright file="MainViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.Windows.Input;

public class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        this.SaveAllCommand = new SaveAllCommand(this);
    }

    public GeneralViewModel General { get; } = new();

    public FormatViewModel Format { get; } = new();

    public void Initialize()
    {

    }

    public ICommand SaveAllCommand { get; }
}

