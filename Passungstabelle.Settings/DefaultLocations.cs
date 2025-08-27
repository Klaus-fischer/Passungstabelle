// <copyright file="DefaultLocations" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.IO;


namespace Passungstabelle.Settings;

internal static class DefaultLocations
{
    static DefaultLocations()
    {
        var common = new DirectoryInfo(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Passungstabelle"));

        if (!common.Exists)
        {
            common.Create();
        }

        CommonLocalSettingsPath = common.FullName;

        var user = new DirectoryInfo(Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
           "Passungstabelle"));

        if (!user.Exists)
        {
            user.Create();
        }

        UserLocalSettingsPath = user.FullName;
    }

    public static string CommonLocalSettingsPath { get; }

    public static string UserLocalSettingsPath { get; }
}

