namespace Passungstabelle.Settings;

using System;
using System.Linq;
using System.Windows.Input;

internal class DeleteSelectedCommand<T> : ICommand
    where T : new()
{
    private readonly ISelectedItemHost<T> host;

    public DeleteSelectedCommand(ISelectedItemHost<T> host)
    {
        this.host = host;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
        => this.host.Collection.Count > 1;

    public void Execute(object? parameter)
    {
        if (this.host.SelectedItem is not T)
        {
            var first = this.host.Collection.FirstOrDefault() ?? new();

            if (!this.host.Collection.Contains(first))
            {
                this.host.Collection.Add(first);
            }

            return;
        }

        var index = this.host.Collection.IndexOf(this.host.SelectedItem);
        this.host.Collection.RemoveAt(index);
        var next = this.host.Collection.Skip(index).FirstOrDefault()
            ?? this.host.Collection.LastOrDefault()
            ?? new();

        if (!this.host.Collection.Contains(next))
        {
            this.host.Collection.Add(next);
        }

        this.host.SelectedItem = next;
    }
}
