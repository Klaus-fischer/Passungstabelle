// <copyright file="RelayCommand" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Windows.Input;


namespace Passungstabelle.Settings;

public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    public static ICommand Empty { get; } = new RelayCommand(() => { }, () => false);

    private readonly Action execute = execute;
    private readonly Func<bool> canExecute = canExecute ?? CanAlwaysExecute;

    static bool CanAlwaysExecute() => true;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
        => this.canExecute();

    public void Execute(object? parameter)
    {
        if (this.CanExecute(parameter))
        {
            execute();
        }
    }
}