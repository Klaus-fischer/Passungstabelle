// <copyright file="TableSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using SolidWorks.Interop.swconst;
using System.Xml.Serialization;

namespace Passungstabelle.CSharp
{
    using System.Collections.Generic;

    public class TableSettings
    {
        public string HeaderLanguage { get; internal set; } = "DE";
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

        //public double BreiteSpalteMaß { get; set; } = 15;
        //public double BreiteSpaltePassung { get; set; } = 15;
        //public double BreiteSpalteMaßePassung { get; set; } = 20;
        //public double BreiteSpalteToleranz { get; set; } = 20;
        //public double BreiteSpalteAbmaß { get; set; } = 20;
        //public double BreiteSpalteAbmaßToleranzMitte { get; set; } = 20;
        //public double BreiteSpalteVorbearbeitungsAbmaße { get; set; } = 20;
        //public double BreiteSpalteVorbearbeitungsToleranzMitte { get; set; } = 20;
        //public double BreiteSpalteAnzahl { get; set; } = 20;
        //public double BreiteSpalteZone { get; set; } = 20;

        //public bool TabSpalteMaß { get; set; } = true;
        //public bool TabSpaltePassung { get; set; } = true;
        //public bool TabSpalteMaßePassung { get; set; } = true;
        //public bool TabSpalteToleranz { get; set; } = true;
        //public bool TabSpalteAbmaß { get; set; } = true;
        //public bool TabSpalteAbmaßToleranzMitte { get; set; } = true;
        //public bool TabSpalteVorbearbeitungsAbmaße { get; set; } = true;
        //public bool TabSpalteVorbearbeitungsToleranzMitte { get; set; } = true;


        public bool TabSpalteAnzahl { get; set; } = false;
        public bool TabSpalteZone { get; set; } = false;

        public IReadOnlyCollection<SpaltenSettings> Spalten { get; }
            = new List<SpaltenSettings>()
            {
                new(){ Name = "Maß", Visible = true, Breite = 15, MergeCells=true, },
                new(){ Name = "Passung", Visible = true, Breite = 15, MergeCells=true,},
                new(){ Name = "MaßePassung", Visible = false, Breite = 20, MergeCells=true,},
                new(){ Name = "Toleranz", Visible = true, Breite = 20, },
                new(){ Name = "Abmaß", Visible = true, Breite = 20, },
                new(){ Name = "AbmaßToleranzMitte", Visible = false, Breite = 20, MergeCells=true,},
                new(){ Name = "VorbearbeitungsAbmaße", Visible = false, Breite = 20, },
                new(){ Name = "VorbearbeitungsToleranzMitte", Visible = false, Breite = 20, MergeCells=true,},
                new(){ Name = "Anzahl", Visible = false, Breite = 20, MergeCells=true,},
                new(){ Name = "Zone", Visible = false, Breite = 20, MergeCells=true,},

            }.AsReadOnly();
    }
}

public class SpaltenSettings
{
    public string Name { get; set; } = string.Empty;

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
