// <copyright file="TableSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.Collections.Generic;
using System.Linq;

public class TableSettings
{
    public string SchemaName { get; internal set; } = "Default";

    public LineWidth RasterStrichStärke { get; internal set; } = LineWidth.Dünn;

    public LineWidth RahmenStrichStärke { get; internal set; } = LineWidth.Dick;

    public bool SpaltenBreiteAutomatisch { get; internal set; } = false;

    public TextFormat HeaderFormat { get; set; } = new TextFormat();

    public TextFormat TextFormat { get; set; } = new TextFormat();

    public HeaderPosition HeaderPosition { get; set; } = HeaderPosition.Oben;

    public bool HasMultiLineHeader => this.Spalten.Any(o => o.Visible && !string.IsNullOrWhiteSpace(o.SubTitle));

    public SpalteSettings Maß { get; } = new SpalteSettings() { Name = "Maß", Title = "Maß", Visible = true, Breite = 15, };

    public SpalteSettings Passung { get; } = new SpalteSettings() { Name = "Passung", Title = "Passung", Visible = true, Breite = 15, };

    public SpalteSettings MaßePassung { get; } = new SpalteSettings() { Name = "MaßePassung", Title = "MaßePassung", Visible = false, Breite = 20, };

    public SpalteSettings Toleranz { get; } = new SpalteSettings() { Name = "Toleranz", Title = "Toleranz", Visible = true, Breite = 20, };

    public SpalteSettings Abmaß { get; } = new SpalteSettings() { Name = "Abmaß", Title = "Abmaß", Visible = true, Breite = 20, };

    public SpalteSettings AbmaßToleranzMitte { get; } = new SpalteSettings() { Name = "AbmaßToleranzMitte", Title = "AbmaßToleranzMitte", Visible = false, Breite = 20, };

    public SpalteSettings VorbearbeitungsAbmaße { get; } = new SpalteSettings() { Name = "VorbearbeitungsAbmaße", Title = "VorbearbeitungsAbmaße", Visible = false, Breite = 20, };

    public SpalteSettings VorbearbeitungsToleranzMitte { get; } = new SpalteSettings() { Name = "VorbearbeitungsToleranzMitte", Title = "VorbearbeitungsToleranzMitte", Visible = false, Breite = 20, };

    public SpalteSettings Anzahl { get; } = new SpalteSettings() { Name = "Anzahl", Title = "Anzahl", Visible = false, Breite = 20, };

    public SpalteSettings Zone { get; } = new SpalteSettings() { Name = "Zone", Title = "Zone", Visible = false, Breite = 20, };

    public IEnumerable<SpalteSettings> Spalten
    {
        get
        {
            yield return this.Maß;
            yield return this.Passung;
            yield return this.MaßePassung;
            yield return this.Toleranz;
            yield return this.Abmaß;
            yield return this.AbmaßToleranzMitte;
            yield return this.VorbearbeitungsAbmaße;
            yield return this.VorbearbeitungsToleranzMitte;
            yield return this.Anzahl;
            yield return this.Zone;
        }
    }
}