// <copyright file="GeneralSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

public class GeneralSettings
{
    public bool Fehlermeldung { get; internal set; } = false;

    public bool LogDatei { get; internal set; } = false;

    public bool NeuPositionieren { get; internal set; } = true;

    public bool LöschenAufRestlichenBlättern { get; internal set; } = true;

    public bool Eventgesteuert { get; set; } = false;

    public bool Event_BevorSave { get; set; } = false;

    public bool Event_AfterRegen { get; set; } = false;

    public bool NurAufErstemBlatt { get; set; } = true;

    public bool PlusZeichen { get; internal set; } = true;

    public bool ReaktionAufLeerePassung { get; internal set; } = false;

    public int RundenAuf { get; internal set; } = 8;

    public double SchichtStärke { get; internal set; } = 0.015;

    public bool SchichtStärkeAbfragen { get; set; } = false;

    public bool SchichtStärkeKeine { get; set; } = true;

    public bool SchichtStärkeFix { get; set; } = false;

    public bool AnsichtsTypSkizzen { get; set; } = true;

    public bool AnsichtsTypTeile { get; set; } = true;

    public bool AnsichtsTypBaugruppen { get; set; } = true;
}
