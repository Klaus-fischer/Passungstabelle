// <copyright file="SpaltenViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public class SpaltenViewModel : BaseViewModel
{
    private string name = string.Empty;
    private string title = string.Empty;
    private string subTitle = string.Empty;
    private bool visible;
    private double breite;
    private bool autoBreite;

    public string Name
    {
        get => this.name;
        set => this.Set(ref this.name, value);
    }

    public string Title
    {
        get => this.title;
        set => this.Set(ref this.title, value);
    }

    public string SubTitle
    {
        get => this.subTitle;
        set => this.Set(ref this.subTitle, value);
    }

    public bool Visible
    {
        get => this.visible;
        set => this.Set(ref this.visible, value);
    }

    public double Breite
    {
        get => this.breite;
        set => this.Set(ref this.breite, value);
    }

    public bool AutoBreite
    {
        get => this.autoBreite;
        set => this.Set(ref this.autoBreite, value);
    }

    internal void Parse(SpalteSettings spalte)
    {
        this.Name = spalte.Name;
        this.Title = spalte.Title;
        this.SubTitle = spalte.SubTitle;
        this.Visible = spalte.Visible;
        this.Breite = spalte.Breite;
        this.AutoBreite = spalte.AutoBreite;
    }

    internal void Update(SpalteSettings settings)
    {
        settings.Name = this.Name;
        settings.Title = this.Title;
        settings.SubTitle = this.SubTitle;
        settings.Visible = this.Visible;
        settings.Breite = this.Breite;
        settings.AutoBreite = this.AutoBreite;
    }
}
