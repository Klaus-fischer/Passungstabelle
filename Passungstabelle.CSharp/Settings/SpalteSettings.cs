// <copyright file="TableSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.Xml.Serialization;

public class SpalteSettings
{
    public string Name { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string SubTitle { get; set; } = string.Empty;

    public bool Visible { get; set; }

    public double Breite { get; set; }

    public bool AutoBreite { get; set; } = true;
}
