// <copyright file="ColorResources" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Passungstabelle.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

public partial class ColorResources
{
    public ColorResources()
    {
        this.InitializeComponent();
        var colors = (ColorCollection)this["AllColors"];
        colors.AddRange(this
            .GetAllBrushes());
    }

    private IEnumerable<ColorClass> GetAllBrushes()
    {
        // Alle statischen Eigenschaften von Brushes holen
        var brushProps = typeof(Brushes)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(SolidColorBrush));

        // Hilfsfunktion: Farbton bestimmen
        double GetHue(Color c)
        {
            double r = c.R / 255.0, g = c.G / 255.0, b = c.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b));
            double hue = 0;
            if (max == min) return 0;
            if (max == r) hue = (60 * ((g - b) / (max - min)) + 360) % 360;
            else if (max == g) hue = (60 * ((b - r) / (max - min)) + 120) % 360;
            else if (max == b) hue = (60 * ((r - g) / (max - min)) + 240) % 360;
            return hue;
        }

        // Hilfsfunktion: Helligkeit bestimmen (perceived brightness)
        double GetBrightness(Color c) => (0.299 * c.R + 0.587 * c.G + 0.114 * c.B);

        // Farbkategorie bestimmen
        int GetCategory(Color c)
        {
            if (c.R == 0 && c.G == 0 && c.B == 0) return 0; // Schwarz
            if (c.R == c.G && c.G == c.B) return 1; // Grautöne
            double hue = GetHue(c);
            if (hue >= 200 && hue < 260) return 2; // Blau
            if (hue >= 80 && hue < 170) return 3; // Grün
            if ((hue >= 330 && hue <= 360) || (hue >= 0 && hue < 30)) return 4; // Rot
            if (hue >= 260 && hue < 330) return 5; // Violett
            if (hue >= 170 && hue < 200) return 6; // Cyan
            if (hue >= 30 && hue < 80) return 7; // Gelb
            return 8; // Sonstige
        }

        var brushes = brushProps
            .Select(p => ((SolidColorBrush)p.GetValue(null), p.Name))
            .Where(t => t.Item1 is not null)
            .Select(t => (Brush: t.Item1, Name: t.Name, Color: t.Item1!.Color))
            .Select(t => (t.Brush, t.Name, Category: GetCategory(t.Color), Brightness: GetBrightness(t.Color)))
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Brightness);

        return brushes.Select(t => new ColorClass(t.Name, t.Brush!, t.Brush!.Color.R << 16 | t.Brush.Color.G << 8 | t.Brush.Color.B));
    }
}

public class ColorCollection : List<ColorClass>
{
}

public class ColorClass(string name, SolidColorBrush brush, int colorCode)
{
    public string Name { get; } = name;

    public SolidColorBrush Brush { get; } = brush;

    public int Color { get; } = colorCode;

    public string ColorCode => $"# {this.Brush.Color.R:X2} {this.Brush.Color.G:X2} {this.Brush.Color.B:X2}";
}
