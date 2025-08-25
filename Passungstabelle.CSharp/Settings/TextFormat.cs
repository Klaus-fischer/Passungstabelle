// <copyright file="TextFormat" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

public class TextFormat
{
    public string Schriftart { get; set; } = "Arial";

    public string Schriftstil { get; set; } = "Standard";

    public double Texthöhe { get; set; } = 2.5;

    public bool Fett { get; set; } = false;

    public bool Unterstrichen { get; set; } = false;

    public bool Durchgestrichen { get; set; } = false;

    public bool Kursiv { get; set; } = false;

    public int RgbFarbe { get; set; } = 0;

    public string SwFarbe
    {
        get
        {
            var red = (RgbFarbe >> 16) & 0xff;
            var green = (RgbFarbe >> 8) & 0xff;
            var blue = (RgbFarbe >> 0) & 0xFF;

            return $"0x{blue:X2}{green:X2}{red:X2}";
        }
    }
}
