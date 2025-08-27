// <copyright file="ResourceLocater" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Passungstabelle.Settings;

public class ResourceLocater : INotifyPropertyChanged
{
    public static ResourceLocater Current { get; } = new ResourceLocater();

    private ResourceLocater() { }

    public static ResourceManager ResourceManager { get; }
        = new ResourceManager("Passungstabelle.Settings.Resources.Strings", typeof(UiTextExtension).Assembly);

    public event PropertyChangedEventHandler? PropertyChanged;
    
    [IndexerName("Item")]
    public string this[UiText key]
    {
        get => ResourceManager.GetString(key.ToString()) ?? string.Empty;
    }

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

    public string SwUiCommandText => this[UiText.SwUiCommandText ];

    public string SwUiHelpCommandText => this[UiText.SwUiHelpCommandText ];

    public string SwUiSettingsCommandText => this[UiText.SwUiSettingsCommandText ];

    public string SwAddinTitle => this[UiText.SwAddinTitle ];

    public string SwAddinDescription => this[UiText.SwAddinDescription];
}

