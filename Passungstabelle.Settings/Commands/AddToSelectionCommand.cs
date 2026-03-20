// <copyright file="HeaderPosition" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Windows.Input;

internal class AddToSelectionCommand<T> : ICommand
{
    private readonly ISelectedItemHost<T> host;

    public AddToSelectionCommand(ISelectedItemHost<T> host)
    {
        this.host = host;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter) => !host.Collection.IsReadOnly;

    public void Execute(object? parameter)
    {
        var item = this.host.CreateItem(out var index);

        if (index.HasValue)
        {
            this.host.Collection.Insert(index.Value, item);
        }
        else
        {
            this.host.Collection.Add(item);
        }

        this.host.SelectedItem = item;
    }
}
