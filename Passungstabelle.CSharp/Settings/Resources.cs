// <copyright file="Resources" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp.My;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Resources
{
    public static LogTemplate KeinePassungTemplate { get; } = (LogTemplate)"Keine Passung für Bemaßung: {Name} Maß: {Wert} eingetragen.";

    public static LogTemplate UngültigeWellenpassung { get; } = (LogTemplate)"Ungültige Wellenpassung {Passung}.";

    public static LogTemplate UngültigeBohrungspassung{ get; } = (LogTemplate)"Ungültige Bohrungspassung {Passung}.";

    public static LogTemplate LeerePassungsWerte { get; } = (LogTemplate)"Keine Passungswerte für {Maß} {Passung} gefunden.";

    internal static string _keine_Bemaßung_gefunden;
    internal static string _ist_eine_freistehende_Bemaßung;
    internal static string Bemaßung;
    internal static string _hat_den_Wert_0;
    internal static string _Keine_Passung_für;
    internal static string eingetragen;
    internal static string _Keine_Passungswerte_für;
    internal static dynamic gefunden;
    internal static string _passt_nicht_zu_Wellenpassung;
    internal static string Passung;
    internal static string _passt_nicht_zu_Bohrungspassung;
    internal static string _Keine_Blätter_in_der_Zeichnung;
    internal static string _Keine_Passungen_gefunden;
    internal static string _Keine_Formatvorlage;
    internal static string _nicht_gefunden;
    internal static string _Abmessungen_von;
    internal static string Format;
    internal static string _Abmessungen_von_ersten_definierten_Format;
    internal static string Fertig;

    public static string Maß { get; internal set; } = "Maß";
    public static string Bohrungsbeschreibung { get; internal set; } = "Bohrungsbeschreibung";
    public static string Kein_Format_mit_den_Abmessungen { get; internal set; } = "Kein Format mit den Abmessungen";
    public static string Passungstabelle_Add_In_für_SolidWorks { get; internal set; } = "Passungstabelle Add-In für SolidWorks";
    public static string Passungstabelle { get; internal set; } = "Passungstabelle";
    public static string Passungstabelle_Setup { get; internal set; } = "Passungstabelle Setup";
    public static string Passungstabelle_Hilfe { get; internal set; } = "Passungstabelle Hilfe";
    public static string HtmlHelpPfad { get; internal set; } = "HtmlHelpPfad";
}
