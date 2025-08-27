// <copyright file="BoolToEnumConverter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Windows.Data;


namespace Passungstabelle.Settings;

public class BoolToEnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool)
        {
            return parameter;
        }

        return value?.Equals(parameter) ?? false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => this.Convert(value, targetType, parameter, culture);
}

