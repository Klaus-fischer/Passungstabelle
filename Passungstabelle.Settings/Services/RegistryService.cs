// <copyright file="RegistryService" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using Microsoft.Win32;
using System;

internal static class RegistryService
{
    public static void UpdateRegistry(GeneralSettings settings)
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Passungstabelle", true)
            ?? Registry.CurrentUser.CreateSubKey(@"Software\Passungstabelle", true);

        key.SetValue(nameof(GeneralSettings.UseCentralLocation), settings.UseCentralLocation ? 1 : 0, RegistryValueKind.DWord);
        key.SetValue(nameof(GeneralSettings.CentralLocation), settings.CentralLocation, RegistryValueKind.String);
    }


    public static bool TryGetCentralLocation(out string centralLocation)
    {
        centralLocation = string.Empty;
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Passungstabelle");
        if (key == null)
        {
            return false;

        }

        // Check if the UseCentralLocation value exists and is set to 1 (true)
        if (key.GetValue(nameof(GeneralSettings.UseCentralLocation)) is not object value || Convert.ToInt32(value) != 1)
        {
            return false;
        }

        centralLocation = key.GetValue(nameof(GeneralSettings.CentralLocation)) as string ?? string.Empty;
        return !string.IsNullOrEmpty(centralLocation);
    }
}
