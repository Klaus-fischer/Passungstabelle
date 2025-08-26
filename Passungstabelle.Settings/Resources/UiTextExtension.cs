// <copyright file="UiTextExtension" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using System.Windows.Markup;

public class UiTextExtension : MarkupExtension, INotifyPropertyChanged
{
    private readonly static PropertyChangedEventArgs ValuePropertyChangedArgs = new(nameof(Value));

    public UiTextExtension(string key)
    {
        this.Key = key;
        ResourceLocater.AddHandler(this.OnLanguageChanged);
    }

    private void OnLanguageChanged(object? sender, LanguageEventArgs e)
    {
        this.PropertyChanged?.Invoke(this, ValuePropertyChangedArgs);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Key { get; set; }

    public string Value => ResourceLocater.Current[Key];

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding(nameof(Value))
        {
            Source = this,
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}
