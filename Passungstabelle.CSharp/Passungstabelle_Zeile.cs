namespace Passungstabelle.CSharp;

using System;
using System.Collections.Generic;

public class Passungstabelle_Zeile : IComparable<Passungstabelle_Zeile>, IEquatable<Passungstabelle_Zeile>
{
    public string Prefix { get; set; } = string.Empty;
    
    public double Maß { get; set; }
    
    public string Passung { get; set; } = string.Empty;
    
    public string MaßPassung { get; set; } = string.Empty;
    
    public double ToleranzO { get; set; }
    
    public double ToleranzU { get; set; }
    
    public double AbmaßO { get; set; }
    
    public double AbmaßU { get; set; }
    
    public double AbmaßToleranzMitte { get; set; }
    
    public double VorbearbeitungAbmaßO { get; set; }
    
    public double VorbearbeitungAbmaßU { get; set; }
    
    public double VorbearbeitungAbmaßToleranzMitte { get; set; }

    public ZeilenTyp Type { get; set; }
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

public enum ZeilenTyp
{
    Shaft, 
    Hole,
}