// <copyright file="GeneralSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public class GeneralSettings
{
    public bool Fehlermeldung { get; internal set; } = false;

    public bool LogDatei { get; internal set; } = false;

    public bool LöschenAufRestlichenBlättern { get; internal set; } = true;

    public bool Eventgesteuert { get; set; } = false;

    public bool Event_BevorSave { get; set; } = false;

    public bool Event_AfterRegen { get; set; } = false;

    public bool NurAufErstemBlatt { get; set; } = true;

    public bool PlusZeichen { get; internal set; } = true;

    public double SchichtStärke { get; internal set; } = 0.015;

    public bool SchichtStärkeAbfragen { get; set; } = false;

}
