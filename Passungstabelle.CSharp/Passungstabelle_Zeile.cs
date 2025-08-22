namespace Passungstabelle.CSharp;

using System;

public class Passungstabelle_Zeile : IComparable<Passungstabelle_Zeile>, IEquatable<Passungstabelle_Zeile>
{
    public Passungstabelle_Zeile(double maß, string passung, double toleranzO, double toleranzU, bool hole, double schichtstärke)
    {
        this.Maß = maß;
        this.Passung = passung;
        this.ToleranzO = toleranzO;
        this.ToleranzU = toleranzU;
        this.AbmaßO = maß + toleranzO;
        this.AbmaßU = maß + toleranzU;
        this.AbmaßToleranzMitte = maß + (toleranzO + toleranzU) / 2;
        this.Type = hole ? PassungsType.Hole : PassungsType.Shaft;

        this.VorbearbeitungAbmaßO = hole ? AbmaßO + 2 * schichtstärke : AbmaßO - 2 * schichtstärke;
        this.VorbearbeitungAbmaßU = hole ? AbmaßU + 2 * schichtstärke : AbmaßU - 2 * schichtstärke;
        this.VorbearbeitungAbmaßToleranzMitte = AbmaßU + (AbmaßO - AbmaßU) / 2.0;
        this.MaßPassung = $"{maß:0.########} {passung}";
    }

    public string Prefix { get; set; } = string.Empty;

    public double Maß { get; }

    public string Passung { get; } = string.Empty;

    public string MaßPassung { get; set; } = string.Empty;

    public double ToleranzO { get; }

    public double ToleranzU { get; }

    public double AbmaßO { get; }

    public double AbmaßU { get; }

    public double AbmaßToleranzMitte { get; }

    public double VorbearbeitungAbmaßO { get; }

    public double VorbearbeitungAbmaßU { get; }

    public double VorbearbeitungAbmaßToleranzMitte { get; }

    public PassungsType Type { get; set; } = PassungsType.Hole;

    public string Name { get; internal set; }

    public string Zone { get; internal set; }

    public int Anzahl { get; internal set; }

    /// <summary>
    /// Vergleichsfunktion zum Sortieren
    /// </summary>
    public int CompareTo(Passungstabelle_Zeile? other)
    {
        return this.Maß.CompareTo(other?.Maß);
    }

    /// <summary>
    /// Vergleichsfunktion für eindeutige Einträge
    /// </summary>
    public bool Equals(Passungstabelle_Zeile? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Maß == other.Maß && this.Passung == other.Passung;
    }

    // Hashcode für eindeutige Einträge
    public override int GetHashCode()
        => HashCode.Combine(this.Maß, this.Passung);
}

public enum PassungsType
{
    Hole,
    Shaft,
}