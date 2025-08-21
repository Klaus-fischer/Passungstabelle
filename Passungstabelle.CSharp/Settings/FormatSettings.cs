// <copyright file="FormatSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

public class FormatSettings
{
    public double Breite { get; set; } = 210;
    public double Höhe { get; set; } = 297;
    public bool EinfügepunktLO { get; set; } = false;
    public bool EinfügepunktRO { get; set; } = true;
    public bool EinfügepunktLU { get; set; } = false;
    public bool EinfügepunktRU { get; set; } = false;
    public double Offset_X { get; set; } = 0;
    public double Offset_Y { get; set; } = 0;

}
