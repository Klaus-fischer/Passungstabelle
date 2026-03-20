// <copyright file="HeaderPosition" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

internal class MoveSelectedCommand<T> : ICommand
{
    private readonly MoveDirection moveDirection;
    private readonly ISelectedItemHost<T> host;

    public MoveSelectedCommand(MoveDirection moveDirection, ISelectedItemHost<T> host)
    {
        this.moveDirection = moveDirection;
        this.host = host;
        this.host.PropertyChanged += InvokeCanExecuteChangedOnPropertyChanged;
        if (this.host.Collection is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged += this.InvokeCanExecuteChangedOnCollectionChanged;
        }
    }


    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        if (this.host.SelectedItem is not T current)
        {
            return false;
        }

        var index = this.host.Collection.IndexOf(current);
        return moveDirection == MoveDirection.Up
            ? index > 0
            : index < this.host.Collection.Count-1;
    }

    public void Execute(object? parameter)
    {
        if (this.host.SelectedItem is not T current)
        {
            return;
        }

        var index = this.host.Collection.IndexOf(current);

        if (moveDirection == MoveDirection.Up)
        {
            this.host.Collection.Insert(index - 1, current);
            this.host.Collection.RemoveAt(index + 1);
        }
        else
        {
            if (index == this.host.Collection.Count - 2)
            {
                this.host.Collection.Add(current);
            }
            else
            {
                this.host.Collection.Insert(index + 2, current);
            }

            this.host.Collection.RemoveAt(index);
        }

        this.host.SelectedItem = current;
    }

    private void InvokeCanExecuteChangedOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        => CommandManager.InvalidateRequerySuggested();

    private void InvokeCanExecuteChangedOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => CommandManager.InvalidateRequerySuggested();
}

internal enum MoveDirection { Up, Down, }
