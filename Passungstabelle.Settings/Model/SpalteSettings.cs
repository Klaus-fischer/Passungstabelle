// <copyright file="TableSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SpalteSettings : INotifyPropertyChanged
{
    private string name = string.Empty;
    private string title = string.Empty;
    private string subTitle = string.Empty;
    private bool visible;
    private double breite;
    private bool autoBreite;

    public string Name
    {
        get => this.name;
        set => this.Set(ref this.name, value);
    }

    public string Title
    {
        get => this.title;
        set => this.Set(ref this.title, value);
    }

    public string SubTitle
    {
        get => this.subTitle;
        set => this.Set(ref this.subTitle, value);
    }

    public bool Visible
    {
        get => this.visible;
        set => this.Set(ref this.visible, value);
    }

    public double Breite
    {
        get => this.breite;
        set => this.Set(ref this.breite, value);
    }

    public bool AutoBreite
    {
        get => this.autoBreite;
        set => this.Set(ref this.autoBreite, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string DisplayName => this.Visible ? this.Name : $"[{this.Name}]";

    protected void Set<T>(ref T origin, T newValue, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(origin, newValue))
        {
            return;
        }

        origin = newValue;
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == nameof(this.Visible))
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DisplayName)));
        }
    }
}
