// <copyright file="TextViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public class TextViewModel : BaseViewModel
{
    private string schriftart = string.Empty;
    public string Schriftart
    {
        get => this.schriftart;
        set => this.Set(ref this.schriftart, value);
    }

    private string schriftstil = string.Empty;
    public string Schriftstil
    {
        get => this.schriftstil;
        set => this.Set(ref this.schriftstil, value);
    }

    private double texthöhe;
    public double Texthöhe
    {
        get => this.texthöhe;
        set => this.Set(ref this.texthöhe, value);
    }

    private bool fett;
    public bool Fett
    {
        get => this.fett;
        set => this.Set(ref this.fett, value);
    }

    private bool unterstrichen;
    public bool Unterstrichen
    {
        get => this.unterstrichen;
        set => this.Set(ref this.unterstrichen, value);
    }

    private bool durchgestrichen;
    public bool Durchgestrichen
    {
        get => this.durchgestrichen;
        set => this.Set(ref this.durchgestrichen, value);
    }

    private bool kursiv;
    public bool Kursiv
    {
        get => this.kursiv;
        set => this.Set(ref this.kursiv, value);
    }

    private int rgbFarbe;
    public int RgbFarbe
    {
        get => this.rgbFarbe;
        set => this.Set(ref this.rgbFarbe, value);
    }

    public void Parse(TextFormat format)
    {
        this.Schriftart = format.Schriftart;
        this.Schriftstil = format.Schriftstil;
        this.Texthöhe = format.Texthöhe;
        this.Fett = format.Fett;
        this.Unterstrichen = format.Unterstrichen;
        this.Durchgestrichen = format.Durchgestrichen;
        this.Kursiv = format.Kursiv;
        this.RgbFarbe = format.RgbFarbe;
    }

    public static explicit operator TextFormat(TextViewModel viewModel) => new TextFormat
    {
        Schriftart = viewModel.Schriftart,
        Schriftstil = viewModel.Schriftstil,
        Texthöhe = viewModel.Texthöhe,
        Fett = viewModel.Fett,
        Unterstrichen = viewModel.Unterstrichen,
        Durchgestrichen = viewModel.Durchgestrichen,
        Kursiv = viewModel.Kursiv,
        RgbFarbe = viewModel.RgbFarbe,
    };
}
