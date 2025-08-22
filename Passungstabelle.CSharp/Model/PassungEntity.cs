// <copyright file="Passung" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using System;
using System.Collections.Generic;
using System.Windows.Controls;

internal class PassungEntity : IEquatable<PassungEntity>
{
    public string Prefix { get; set; } = string.Empty;

    public double Maß { get; set; }

    public string Passung { get; set; } = string.Empty;

    public double ToleranzO { get; set; }

    public double ToleranzU { get; set; }

    public List<string> Zone { get; set; } = new List<string>();

    public PassungsType PassungsType { get; set; } = PassungsType.Hole;

    public bool Equals(PassungEntity? other)
    {
        if (other is null)
        {
            return false;
        }

        return Prefix == other.Prefix && Maß == other.Maß && Passung == other.Passung;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Prefix, Maß, Passung);
    }

    public override string ToString()
    {
        return $"{Prefix}{Maß:0.#} {Passung}";
    }
}
