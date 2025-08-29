// <copyright file="XmlReaderExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.Globalization;
using System.Windows;
using System.Xml;

internal static class XmlReaderExtensions
{
    public static T? ReadEnum<T>(this XmlReader reader) where T : struct, Enum
    {
        return Enum.TryParse<T>(reader.Value, true, out var result) ? result : default;
    }

    public static bool ReadBool(this XmlReader reader)
        => reader.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || reader.Value == "1";

    public static double? ReadDouble(this XmlReader reader)
        => double.TryParse(
            reader.Value,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var value)
        ? value : null;

    public static int? ReadInt(this XmlReader reader)
      => int.TryParse(
          reader.Value,
          NumberStyles.Any,
          CultureInfo.InvariantCulture,
          out var value)
      ? value : null;

    public static Thickness ReadThickness(this XmlReader reader, Thickness origin)
    {
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            if (reader.Name == nameof(Thickness.Top))
            {
                origin.Top = reader.ReadDouble() ?? origin.Top;
            }
            else if (reader.Name == nameof(Thickness.Left))
            {
                origin.Left = reader.ReadDouble() ?? origin.Left;
            }
            else if (reader.Name == nameof(Thickness.Right))
            {
                origin.Right = reader.ReadDouble() ?? origin.Right;
            }
            else if (reader.Name == nameof(Thickness.Bottom))
            {
                origin.Bottom = reader.ReadDouble() ?? origin.Bottom;
            }
        }

        reader.MoveToElement();
        // Leeres Element überspringen
        if (!reader.IsEmptyElement)
            reader.Read();

        return origin;
    }

    public static Vector ReadVector(this XmlReader reader, Vector origin)
    {
        for (int i = 0; i < reader.AttributeCount; i++)
        {
            reader.MoveToAttribute(i);
            if (reader.Name == nameof(Vector.X))
            {
                origin.X = reader.ReadDouble() ?? origin.X;
            }
            else if (reader.Name == nameof(Vector.Y))
            {
                origin.Y = reader.ReadDouble() ?? origin.Y;
            }
        }

        reader.MoveToElement();
        // Leeres Element überspringen
        if (!reader.IsEmptyElement)
            reader.Read();

        return origin;
    }
}
