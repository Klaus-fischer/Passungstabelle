// <copyright file="BaseViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passungstabelle.Settings;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void Set<T>(ref T origin, T newValue, [CallerMemberName] string propertyName = "", params string[] alsoNotify)
    {
        if (EqualityComparer<T>.Default.Equals(origin, newValue))
        {
            return;
        }

        origin = newValue;
        this.OnPropertyChanged(propertyName);
        foreach (var name in alsoNotify)
        {
            this.OnPropertyChanged(name);
        }
    }
}