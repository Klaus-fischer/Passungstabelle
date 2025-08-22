// <copyright file="TableSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using SolidWorks.Interop.swconst;
using System.Xml.Serialization;

namespace Passungstabelle.CSharp
{
    using System.Collections.Generic;
    using System.Linq;

    public class TableSettings
    {
        public string SchemaName { get; internal set; } = "DE";

        public long FarbeZeile { get; internal set; } = 0;

        public long FarbeKopfZeile { get; internal set; } = 0;

        public LineWidth RasterStrichStärke { get; internal set; } = LineWidth.Dünn;

        public LineWidth RahmenStrichStärke { get; internal set; } = LineWidth.Dick;

        public bool SpaltenBreiteAutomatisch { get; internal set; } = false;

        public string SchriftartKopfZeile { get; internal set; } = "Arial";

        public string SchriftstilKopfZeile { get; set; } = "Standard";

        public double TexthöheKopfZeile { get; internal set; } = 2.5;

        public bool FettKopfZeile { get; internal set; } = false;

        public bool UnterstrichenKopfZeile { get; internal set; } = false;

        public bool DurchgestrichenKopfZeile { get; internal set; } = false;

        public bool KursivKopfZeile { get; internal set; } = false;

        public string SchriftartZeile { get; internal set; } = "Arial";

        public double TexthöheZeile { get; internal set; } = 2.5;

        public bool FettZeile { get; internal set; } = false;

        public bool UnterstrichenZeile { get; internal set; } = false;

        public bool DurchgestrichenZeile { get; internal set; } = false;

        public bool KursivZeile { get; internal set; } = false;

        public bool HeaderOben { get; internal set; } = true;

        public bool HeaderUnten { get; set; } = false;

        public bool HasMultiLineHeader => this.Spalten.Any(o => o.Visible && !string.IsNullOrWhiteSpace(o.SubTitle));

        public SpalteSettings Maß { get; } = new SpalteSettings() { Name = "Maß", Title = "Maß", Visible = true, Breite = 15, MergeCells = true, };

        public SpalteSettings Passung { get; } = new SpalteSettings() { Name = "Passung", Title = "Passung", Visible = true, Breite = 15, MergeCells = true, };

        public SpalteSettings MaßePassung { get; } = new SpalteSettings() { Name = "MaßePassung", Title = "MaßePassung", Visible = false, Breite = 20, MergeCells = true, };

        public SpalteSettings Toleranz { get; } = new SpalteSettings() { Name = "Toleranz", Title = "Toleranz", Visible = true, Breite = 20, };

        public SpalteSettings Abmaß { get; } = new SpalteSettings() { Name = "Abmaß", Title = "Abmaß", Visible = true, Breite = 20, };

        public SpalteSettings AbmaßToleranzMitte { get; } = new SpalteSettings() { Name = "AbmaßToleranzMitte", Title = "AbmaßToleranzMitte", Visible = false, Breite = 20, MergeCells = true, };

        public SpalteSettings VorbearbeitungsAbmaße { get; } = new SpalteSettings() { Name = "VorbearbeitungsAbmaße", Title = "VorbearbeitungsAbmaße", Visible = false, Breite = 20, };

        public SpalteSettings VorbearbeitungsToleranzMitte { get; } = new SpalteSettings() { Name = "VorbearbeitungsToleranzMitte", Title = "VorbearbeitungsToleranzMitte", Visible = false, Breite = 20, MergeCells = true, };

        public SpalteSettings Anzahl { get; } = new SpalteSettings() { Name = "Anzahl", Title = "Anzahl", Visible = false, Breite = 20, MergeCells = true, };

        public SpalteSettings Zone { get; } = new SpalteSettings() { Name = "Zone", Title = "Zone", Visible = false, Breite = 20, MergeCells = true, };

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
}

public class SpalteSettings
{
    public string Name { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string SubTitle { get; set; } = string.Empty;

    public bool Visible { get; set; }

    public double Breite { get; set; }

    [XmlIgnore]
    public bool MergeCells { get; init; }
}

public enum LineWidth
{
    Dünn = swLineWeights_e.swLW_THIN,
    Normal = swLineWeights_e.swLW_NORMAL,
    Dick = swLineWeights_e.swLW_THICK,
    Dick_2 = swLineWeights_e.swLW_THICK2,
    Dick_3 = swLineWeights_e.swLW_THICK3,
    Dick_4 = swLineWeights_e.swLW_THICK4,
    Dick_5 = swLineWeights_e.swLW_THICK5,
    Dick_6 = swLineWeights_e.swLW_THICK6,
}
