// <copyright file="ResourceLocater" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Passungstabelle.Settings;

internal class ResourceLocater : INotifyPropertyChanged
{
    public static ResourceLocater Current { get; } = new ResourceLocater();

    private ResourceLocater() { }

    public static ResourceManager ResourceManager { get; }
        = new ResourceManager("Passungstabelle.Settings.Resources.Strings", typeof(UiTextExtension).Assembly);

    public event PropertyChangedEventHandler? PropertyChanged;

    [IndexerName("Item")]
    public string this[string key]
    {
        get => ResourceManager.GetString(key) ?? string.Empty;
    }

    public void ChangeLanguage(string language)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(language);
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }
}

