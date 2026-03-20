// <copyright file="HeaderPosition" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Windows.Input;

internal class UpdateSelectionCommand<T> : ICommand
{
    private readonly ISelectedItemHost<T> host;

    public UpdateSelectionCommand(ISelectedItemHost<T> host)
    {
        this.host = host;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
        => !host.Collection.IsReadOnly
        && this.host.PropertiesHasChanged(this.host.SelectedItem);

    public void Execute(object? parameter)
    {
        var index = this.host.Collection.IndexOf(this.host.SelectedItem);
        if (index < 0)
        {
            return;
        }

        var item = this.host.CreateItem(out _);

        this.host.Collection.Insert(index, item);
        this.host.Collection.Remove(this.host.SelectedItem);
        this.host.SelectedItem = item;
    }
}
