namespace Passungstabelle.CSharp;

using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// stellt diverse Definitionen für das Projekt Passungstabelle bereit
/// </summary>
public class Definitionen
{
    public StringReader Xmlshema { get; set; } = null!;

    /// <summary>
    /// 'Name der alten Setup-Datei
    /// </summary>
    public const string OLD_INI_File = "Passungstabelle.ini";
    /// <summary>
    /// 'Name der neuen Setup-Datei
    /// </summary>
    public const string INI_File = "Setup.XML";
    /// <summary>
    /// 'Name der Log-Datei
    /// </summary>
    public const string LOGName = "Passungstabelle";
    /// <summary>
    /// 'Typdefinition String für Dictionary
    /// </summary>
    public const string ts = "String";
    /// <summary>
    /// 'Typdefinition Integer für Dictionary
    /// </summary>
    public const string ti = "Integer";
    /// <summary>
    /// 'Typdefinition Double für Dictionary
    /// </summary>
    public const string td = "Double";
    /// <summary>
    /// 'Typdefinition Boolean für Dictionary
    /// </summary>
    public const string tb = "Boolean";
    /// <summary>
    /// 'Typdefinition Radiobuttongroup für Dictionary
    /// </summary>
    public const string trg = "RadioButtonGroup";
    /// <summary>
    /// 'Typdefinition Color für Dictionary
    /// </summary>
    public const string tcolor = "ColorTextField";
    /// <summary>
    /// 'Typdefinition Combobox für Dictionary
    /// </summary>
    public const string tcb = "ComboBox";

    /// <summary>
    /// Aufzählung der möglichen Setup-Einstellungsbereiche
    /// </summary>
    public enum Attribute
    {
        GENERELLE_Attr = 1,
        TABELLENATTR = 2,
        TABELLENTEXTATTR = 3,
        TABELLENRASTERATTR = 4,
        FORMATATTR = 5,
        SPRACHATTR = 100,
        ÜBERSETZUNGSATTR = 200,
    }
    /// <summary>
    /// 'Dictionary für Generelle-Attribute
    /// </summary>
    public Dictionary<string, string> GENERELLE_ATTR { get; set; } = new() { { "RundenAuf", ti }, { "PlusZeichen", tb }, { "ReaktionAufLeerePassung", tb }, { "NeuPositionieren", tb }, { "NurAufErstemBlatt", tb }, { "AnsichtsTypSkizzen", tb }, { "AnsichtsTypTeile", tb }, { "AnsichtsTypBaugruppen", tb }, { "LogDatei", tb }, { "SchichtStärke", td }, { "SchichtStärkeAbfragen", tb }, { "SchichtStärkeKeine", tb }, { "SchichtStärkeFix", tb }, { "Fehlermeldung", tb }, { "LöschenAufRestlichenBlättern", tb }, { "Eventgesteuert", tb }, { "Event_BevorSave", tb }, { "Event_AfterRegen", tb } };

    /// <summary>
    /// 'Vorgabewerte für generelle Attribute
    /// </summary>
    public Dictionary<string, string> GENERELLE_ATTR_Init { get; set; } = new() { { "RundenAuf", "8" }, { "PlusZeichen", "True" }, { "ReaktionAufLeerePassung", "False" }, { "NeuPositionieren", "True" }, { "NurAufErstemBlatt", "True" }, { "AnsichtsTypSkizzen", "True" }, { "AnsichtsTypTeile", "True" }, { "AnsichtsTypBaugruppen", "True" }, { "LogDatei", "False" }, { "SchichtStärke", "0" }, { "SchichtStärkeAbfragen", tb }, { "SchichtStärkeKeine", "True" }, { "SchichtStärkeFix", "False" }, { "Fehlermeldung", "False" }, { "LöschenAufRestlichenBlättern", "True" }, { "Eventgesteuert", "False" }, { "Event_BevorSave", "False" }, { "Event_AfterRegen", "False" } };

    /// <summary>
    /// 'Dictionary für Tabellen-Attribute
    /// </summary>
    public Dictionary<string, string> TABELLENATTR { get; set; } = new() { { "HeaderOben", tb }, { "HeaderUnten", tb }, { "HeaderLanguage", ts }, { "SchriftartZeile", ts }, { "SchriftstilZeile", ts }, { "UnterstrichenZeile", tb }, { "DurchgestrichenZeile", tb }, { "FettZeile", tb }, { "TexthöheZeile", td }, { "FarbeZeile", ts }, { "KursivZeile", tb }, { "SchriftartKopfZeile", ts }, { "SchriftstilKopfZeile", ts }, { "UnterstrichenKopfZeile", tb }, { "DurchgestrichenKopfZeile", tb }, { "FettKopfZeile", tb }, { "TexthöheKopfZeile", td }, { "FarbeKopfZeile", ts }, { "KursivKopfZeile", tb }, { "BreiteSpalteMaß", td }, { "BreiteSpaltePassung", td }, { "BreiteSpalteMaßePassung", td }, { "BreiteSpalteToleranz", td }, { "BreiteSpalteAbmaß", td }, { "BreiteSpalteAbmaßToleranzMitte", td }, { "BreiteSpalteVorbearbeitungsAbmaße", td }, { "BreiteSpalteVorbearbeitungsToleranzMitte", td }, { "BreiteSpalteAnzahl", td }, { "BreiteSpalteZone", td }, { "RasterStrichStärke", ts }, { "RahmenStrichStärke", ts }, { "SpaltenBreiteAutomatisch", tb }, { "TabSpalteMaß", tb }, { "TabSpaltePassung", tb }, { "TabSpalteMaßePassung", tb }, { "TabSpalteToleranz", tb }, { "TabSpalteAbmaß", tb }, { "TabSpalteAbmaßToleranzMitte", tb }, { "TabSpalteVorbearbeitungsAbmaße", tb }, { "TabSpalteVorbearbeitungsToleranzMitte", tb }, { "TabSpalteAnzahl", tb }, { "TabSpalteZone", tb } };

    /// <summary>
    /// 'Vorgabewerte für Tabellen-Attribute
    /// </summary>
    public Dictionary<string, string> TABELLENATTR_Init { get; set; } = new() { { "HeaderOben", "True" }, { "HeaderUnten", "False" }, { "HeaderLanguage", "DE" }, { "SchriftartZeile", "Arial" }, { "SchriftstilZeile", "Standard" }, { "UnterstrichenZeile", "False" }, { "DurchgestrichenZeile", "False" }, { "FettZeile", "False" }, { "TexthöheZeile", "2,5" }, { "FarbeZeile", "0" }, { "KursivZeile", "False" }, { "SchriftartKopfZeile", "Arial" }, { "SchriftstilKopfZeile", "Standard" }, { "UnterstrichenKopfZeile", "False" }, { "DurchgestrichenKopfZeile", "False" }, { "FettKopfZeile", "False" }, { "TexthöheKopfZeile", "2,5" }, { "FarbeKopfZeile", "0" }, { "KursivKopfZeile", "False" }, { "BreiteSpalteMaß", "15" }, { "BreiteSpaltePassung", "15" }, { "BreiteSpalteMaßePassung", "20" }, { "BreiteSpalteToleranz", "20" }, { "BreiteSpalteAbmaß", "20" }, { "BreiteSpalteAbmaßToleranzMitte", "20" }, { "BreiteSpalteVorbearbeitungsAbmaße", "20" }, { "BreiteSpalteVorbearbeitungsToleranzMitte", "20" }, { "BreiteSpalteAnzahl", "20" }, { "BreiteSpalteZone", "20" }, { "RasterStrichStärke", "Dünn" }, { "RahmenStrichStärke", "Dick" }, { "SpaltenBreiteAutomatisch", "False" }, { "TabSpalteMaß", "True" }, { "TabSpaltePassung", "True" }, { "TabSpalteMaßePassung", "True" }, { "TabSpalteToleranz", "True" }, { "TabSpalteAbmaß", "True" }, { "TabSpalteAbmaßToleranzMitte", "True" }, { "TabSpalteVorbearbeitungsAbmaße", "True" }, { "TabSpalteVorbearbeitungsToleranzMitte", "True" }, { "TabSpalteAnzahl", "False" }, { "TabSpalteZone", "False" } };

    /// <summary>
    /// 'Dictionary für Format-Attribute
    /// </summary>
    public Dictionary<string, string> FORMATATTR { get; set; } = new() { { "Breite", td }, { "Höhe", td }, { "EinfügepunktLO", tb }, { "EinfügepunktRO", tb }, { "EinfügepunktLU", tb }, { "EinfügepunktRU", tb }, { "Offset_X", td }, { "Offset_Y", td } };

    /// <summary>
    /// 'Vorgabewerte für Format-Attribute
    /// </summary>
    public Dictionary<string, string> FORMATATTR_Init { get; set; } = new() { { "Breite", "210" }, { "Höhe", "297" }, { "EinfügepunktLO", "False" }, { "EinfügepunktRO", "True" }, { "EinfügepunktLU", "False" }, { "EinfügepunktRU", "False" }, { "Offset_X", "0" }, { "Offset_Y", "0" } };

    /// <summary>
    /// 'Dictionary für Übersetzungs-Attribute
    /// </summary>
    public Dictionary<string, string> ÜBERSETZUNGSATTR { get; set; } = new() { { "Kürzel", ts }, { "Passung", ts }, { "Toleranz", ts }, { "Abmaß", ts }, { "VorbearbeitungsAbmaße", ts }, { "Maß", ts }, { "MaßePassung", ts }, { "AbmaßToleranzMitte", ts }, { "VorbearbeitungsToleranzMitte", ts }, { "Anzahl", ts }, { "Zone", ts } };

    /// <summary>
    /// 'Vorgabewerte für Übersetzungs-Attribute
    /// </summary>
    public Dictionary<string, string> ÜBERSETZUNGSATTR_Init { get; set; } = new() { { "Kürzel", "DE" }, { "Passung", "Passung" }, { "Toleranz", "Toleranz" }, { "Abmaß", "Abmaß" }, { "VorbearbeitungsAbmaße", "Vorbearbeitung" }, { "Maß", "Maß" }, { "MaßePassung", "Maß+Passung" }, { "AbmaßToleranzMitte", "Abmaß Toleranzmitte" }, { "VorbearbeitungsToleranzMitte", "Vorbearbeitungs ToleranzMitte" }, { "Anzahl", "Anzahl" }, { "Zone", "Zone" } };

    /// <summary>
    /// 'Linienarten
    /// </summary>
    public List<string> LINIENARTEN { get; set; } = new() { "Dünn", "Normal", "Dick", "Dick(2)", "Dick(3)", "Dick(4)", "Dick(5)", "Dick(6)" };

    /// <summary>
    /// 'Sprachkürzel, wird benötigt falls keine Setup-Datei gefunden wird
    /// </summary>
    public Dictionary<string, string> SPRACHATTR { get; set; } = new() { { "AA", "Afar" }, { "AB", "Abchasisch" }, { "AF", "Afrikaans" }, { "AM", "Amharisch" }, { "AR", "Arabisch" }, { "AS", "Assamesisch" }, { "AY", "Aymara" }, { "AZ", "Aserbaidschanisch" }, { "BA", "Baschkirisch" }, { "BE", "Belorussisch" }, { "BG", "Bulgarisch" }, { "BH", "Biharisch" }, { "BI", "Bislamisch" }, { "BN", "Bengalisch" }, { "BO", "Tibetanisch" }, { "BR", "Bretonisch" }, { "CA", "Katalanisch" }, { "CO", "Korsisch" }, { "CS", "Tschechisch" }, { "CY", "Walisisch" }, { "DA", "Dänisch" }, { "DE", "Deutsch" }, { "DZ", "Dzongkha/ Bhutani" }, { "EL", "Griechisch" }, { "EN", "Englisch" }, { "EO", "Esperanto" }, { "ES", "Spanisch" }, { "ET", "Estnisch" }, { "EU", "Baskisch" }, { "FA", "Persisch" }, { "FI", "Finnisch" }, { "FJ", "Fiji" }, { "FO", "Färöisch" }, { "FR", "Französisch" }, { "FY", "Friesisch" }, { "GA", "Irisch" }, { "GD", "Schottisches Gälisch" }, { "GL", "Galizisch" }, { "GN", "Guarani" }, { "GU", "Gujaratisch" }, { "HA", "Haussa" }, { "HE", "Hebräisch" }, { "HI", "Hindi" }, { "HR", "Kroatisch" }, { "HU", "Ungarisch" }, { "HY", "Armenisch" }, { "IA", "Interlingua" }, { "ID", "Indonesisch" }, { "IE", "Interlingue" }, { "IK", "Inupiak" }, { "IS", "Isländisch" }, { "IT", "Italienisch" }, { "IU", "Inuktitut (Eskimo)" }, { "IW", "Hebräisch (aktualisiert: HE)" }, { "JA", "Japanisch" }, { "JI", "Jiddish (aktualisiert: YI)" }, { "JV", "Javanisch" }, { "KA", "Georgisch" }, { "KK", "Kasachisch" }, { "KL", "Kalaallisut" }, { "KM", "Kambodschanisch" }, { "KN", "Kannada" }, { "KO", "Koreanisch" }, { "KS", "Kaschmirisch" }, { "KU", "Kurdisch" }, { "KY", "Kirgisisch" }, { "LA", "Lateinisch" }, { "LN", "Lingala" }, { "LO", "Laotisch" }, { "LT", "Litauisch" }, { "LV", "Lettisch" }, { "MG", "Malagasisch" }, { "MI", "Maorisch" }, { "MK", "Mazedonisch" }, { "ML", "Malajalam" }, { "MN", "Mongolisch" }, { "MO", "Moldavisch" }, { "MR", "Marathi" }, { "MS", "Malaysisch" }, { "MT", "Maltesisch" }, { "MY", "Burmesisch" }, { "NA", "Nauruisch" }, { "NE", "Nepalesisch" }, { "NL", "Holländisch" }, { "NO", "Norwegisch" }, { "OC", "Okzitanisch" }, { "OM", "Oromo" }, { "OR", "Oriya" }, { "PA", "Pundjabisch" }, { "PL", "Polnisch" }, { "PS", "Paschtu" }, { "PT", "Portugiesisch" }, { "QU", "Quechua" }, { "RM", "Rätoromanisch" }, { "RN", "Kirundisch" }, { "RO", "Rumänisch" }, { "RU", "Russisch" }, { "RW", "Kijarwanda" }, { "SA", "Sanskrit" }, { "SD", "Zinti" }, { "SG", "Sango" }, { "SH", "Serbokroatisch" }, { "SI", "Singhalesisch" }, { "SK", "Slowakisch" }, { "SL", "Slowenisch" }, { "SM", "Samoanisch" }, { "SN", "Schonisch" }, { "SO", "Somalisch" }, { "SQ", "Albanisch" }, { "SR", "Serbisch" }, { "SS", "Swasiländisch" }, { "ST", "Sesothisch" }, { "SU", "Sudanesisch" }, { "SV", "Schwedisch" }, { "SW", "Suaheli" }, { "TA", "Tamilisch" }, { "TE", "Tegulu" }, { "TG", "Tadschikisch" }, { "TH", "Thai" }, { "TI", "Tigrinja" }, { "TK", "Turkmenisch" }, { "TL", "Tagalog" }, { "TN", "Sezuan" }, { "TO", "Tongaisch" }, { "TR", "Türkisch" }, { "TS", "Tsongaisch" }, { "TT", "Tatarisch" }, { "TW", "Twi" }, { "UG", "Uigur" }, { "UK", "Ukrainisch" }, { "UR", "Urdu" }, { "UZ", "Usbekisch" }, { "VI", "Vietnamesisch" }, { "VO", "Volapük" }, { "WO", "Wolof" }, { "XH", "Xhosa" }, { "YI", "Jiddish" }, { "YO", "Joruba" }, { "ZA", "Zhuang" }, { "ZH", "Chinesisch" }, { "ZU", "Zulu" } };

    /// <summary>
    /// 'Attribute einer Zeile
    /// </summary>
    public List<string> ZEILENATTR { get; set; } = new() { "Maß", "Passung", "MaßPassung", "ToleranzO", "ToleranzU", "AbmaßO", "AbmaßU", "AbmaßToleranzMitte", "VorbearbeitungAbmaßO", "VorbearbeitungAbmaßU", "VorbearbeitungAbmaßToleranzMitte", "Anzahl", "Zone" };

    /// <summary>
    /// 'Dictionary für Übersetzungs-Attribute
    /// </summary>
    public Dictionary<string, string> MSGATTR { get; set; } = new() { { "Msg", ts }, { "Show", tb } };
    /// <summary>
    /// 'Vorgabewerte für Übersetzungs-Attribute
    /// </summary>
    public Dictionary<string, string> MSGATTR_Init { get; set; } = new() { { "Msg", "" }, { "Show", "False" } };

    /// <summary>
    /// Struktur für Blatteigenschaften
    /// </summary>
    public struct BlattEigenschaften
    {


        /// <summary>
        /// 'Formatname
        /// </summary>
        public string Formatname { get; set; }

        /// <summary>
        /// Sprache
        /// </summary>
        public string sprache { get; set; }

        /// <summary>
        /// Blatteigenschaften (Abmessungen, Formatvorlage, ....)
        /// </summary>
        public object Eigenschaften { get; set; }
    }

    public struct Werte
    {
        public string s { get; set; }
        public double d { get; set; }
        public int i { get; set; }
        public bool b { get; set; }
        public int co { get; set; }
    }

    /// <summary>
    /// Definition der im Setup abgebildeten Einstellungen
    /// </summary>
    public class Strctformat
    {
        public string format { get; set; }

        /// <summary>
        /// Generelle Einstellungen
        /// </summary>
        public Dictionary<string, Werte> generelle_paramter { get; set; }

        /// <summary>
        /// Tabellen Einstellungen
        /// </summary>
        public Dictionary<string, Werte> tabbellen_paramter { get; set; }
        /// <summary>
        /// Format Einstellungen
        /// </summary>
        public Dictionary<string, Werte> format_paramter { get; set; }
    }

    /// <summary>
    /// Struktur für Sprachcodes
    /// </summary>
    struct Sprach_Codes
    {
        string Kürzel { get; set; } // Sprachkürzel
        string Beschreibung { get; set; }

    }

}