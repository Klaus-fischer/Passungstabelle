namespace Passungstabelle.CSharp;

using System;
using System.Diagnostics;

[DebuggerDisplay("{Prefix}{Maß:0.###} {Passung}")]
public class TabellenZeile
{
    internal TabellenZeile(double maß, string passung, double toleranzO, double toleranzU, PassungsType type)
    {
        this.Maß = maß;
        this.Passung = passung;
        this.ToleranzO = toleranzO;
        this.ToleranzU = toleranzU;
        this.AbmaßO = maß + toleranzO;
        this.AbmaßU = maß + toleranzU;
        this.AbmaßToleranzMitte = maß + (toleranzO + toleranzU) / 2;
        this.Type = type;
    }

    public string Prefix { get; set; } = string.Empty;

    public double Maß { get; }

    public string Passung { get; } = string.Empty;

    public string MaßPassung(bool includePlusSign)
    {
        return includePlusSign
            ? $"{this.Prefix}{this.Maß:+0.###} {this.Passung}"
            : $"{this.Prefix}{this.Maß:0.###} {this.Passung}";
    }

    public double ToleranzO { get; }

    public double ToleranzU { get; }

    public double AbmaßO { get; }

    public double AbmaßU { get; }

    public double AbmaßToleranzMitte { get; }

    public double VorbearbeitungAbmaßO(double schichtstärke)
    {
        return this.Type == PassungsType.Hole ? AbmaßO + 2 * schichtstärke : AbmaßO - 2 * schichtstärke;
    }

    public double VorbearbeitungAbmaßU(double schichtstärke)
    {
        return this.Type == PassungsType.Hole ? AbmaßU + 2 * schichtstärke : AbmaßU - 2 * schichtstärke;
    }

    public double VorbearbeitungAbmaßToleranzMitte(double schichtstärke)
    {
        return AbmaßU + (AbmaßO - AbmaßU) / 2.0;
    }

    public PassungsType Type { get; set; } = PassungsType.Hole;

    [Obsolete]
    public string Name { get; internal set; }

    public string Zone { get; internal set; }

    [Obsolete]
    public int Anzahl { get; internal set; }

    public static explicit operator TabellenZeile(PassungEntity entity)
    {
        return new TabellenZeile(
            entity.Maß,
            entity.Passung,
            entity.ToleranzO,
            entity.ToleranzU,
            entity.PassungsType)
        {
            Prefix = entity.Prefix,
            Zone = string.Join(", ", entity.Zone),
        };
    }
}