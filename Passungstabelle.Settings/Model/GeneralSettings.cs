// <copyright file="GeneralSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;


public class GeneralSettings
{
    public string Language { get; set; } = string.Empty;

    public bool UseCentralLocation { get; set; } = false;

    public string CentralLocation { get; set; } = string.Empty;

    public bool CreateLogFile { get; internal set; } = false;
    
    public string LogFilePath { get; set; } = string.Empty;

    public bool OnlyAtFirstSheet { get; set; } = true;

    public bool RemoveAtAllPages { get; internal set; } = true;

    public bool UseEvents { get; set; } = false;

    public bool RecalculateBeforeSave { get; set; } = false;

    public bool RecalculateAfterRebuild { get; set; } = false;

    public bool UsePlusSign { get; internal set; } = true;

    public bool SuppressMessages { get; internal set; } = false;

}
