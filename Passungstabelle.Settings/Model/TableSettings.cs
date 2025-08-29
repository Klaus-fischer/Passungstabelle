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

    public TextFormat HeaderFormat { get; internal set; } = new TextFormat();

    public TextFormat TextFormat { get; internal set; } = new TextFormat();

    public HeaderPosition HeaderPosition { get; internal set; } = HeaderPosition.Oben;

    public bool HasMultiLineHeader => this.Spalten.Any(o => o.Visible && !string.IsNullOrWhiteSpace(o.SubTitle));

    public ColumnSettings Maß { get; } = new ColumnSettings() { Name = "Maß", Title = "Maß", Visible = true, Breite = 15, };

    public ColumnSettings Passung { get; } = new ColumnSettings() { Name = "Passung", Title = "Passung", Visible = true, Breite = 15, };

    public ColumnSettings MaßePassung { get; } = new ColumnSettings() { Name = "MaßePassung", Title = "MaßePassung", Visible = false, Breite = 20, };

    public ColumnSettings Toleranz { get; } = new ColumnSettings() { Name = "Toleranz", Title = "Toleranz", Visible = true, Breite = 20, };

    public ColumnSettings Abmaß { get; } = new ColumnSettings() { Name = "Abmaß", Title = "Abmaß", Visible = true, Breite = 20, };

    public ColumnSettings AbmaßToleranzMitte { get; } = new ColumnSettings() { Name = "AbmaßToleranzMitte", Title = "AbmaßToleranzMitte", Visible = false, Breite = 20, };

    public ColumnSettings VorbearbeitungsAbmaße { get; } = new ColumnSettings() { Name = "VorbearbeitungsAbmaße", Title = "VorbearbeitungsAbmaße", Visible = false, Breite = 20, };

    public ColumnSettings VorbearbeitungsToleranzMitte { get; } = new ColumnSettings() { Name = "VorbearbeitungsToleranzMitte", Title = "VorbearbeitungsToleranzMitte", Visible = false, Breite = 20, };

    public ColumnSettings Anzahl { get; } = new ColumnSettings() { Name = "Anzahl", Title = "Anzahl", Visible = false, Breite = 20, };

    public ColumnSettings Zone { get; } = new ColumnSettings() { Name = "Zone", Title = "Zone", Visible = false, Breite = 20, };

    public IEnumerable<ColumnSettings> Spalten
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