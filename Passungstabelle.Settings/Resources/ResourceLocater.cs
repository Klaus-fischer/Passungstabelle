// <copyright file="ResourceLocater" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Resources;
using System.Windows;


namespace Passungstabelle.Settings;

internal class ResourceLocater
{
    public static ResourceLocater Current { get; } = new ResourceLocater();

    private ResourceLocater() { }

    public static ResourceManager ResourceManager { get; }
        = new ResourceManager("Passungstabelle.Settings.Resources.Strings", typeof(UiTextExtension).Assembly);

    public static void AddHandler(EventHandler<LanguageEventArgs> handler)
    {
        WeakEventManager<ResourceLocater, LanguageEventArgs>.AddHandler(Current, nameof(OnLanguageChange), handler);
    }

    public static void RemoveHandler(EventHandler<LanguageEventArgs> handler)
    {
        WeakEventManager<ResourceLocater, LanguageEventArgs>.RemoveHandler(Current, nameof(OnLanguageChange), handler);
    }

    public event EventHandler<LanguageEventArgs>? OnLanguageChange;

    public string this[string key]
    {
        get => ResourceManager.GetString(key) ?? string.Empty;
    }

    public void ChangeLanguage(string language)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(language);
        var args = new LanguageEventArgs(language);
        this.OnLanguageChange?.Invoke(this, args);
    }
}

public class LanguageEventArgs(string language) : EventArgs
{
    public string Language { get; } = language;
}

