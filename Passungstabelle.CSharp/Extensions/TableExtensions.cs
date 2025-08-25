// <copyright file="TableExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

internal static class TableExtensions
{
    public static void SetHeaderPosition(this TableAnnotation table, TableSettings settings)
    {
        table.SetHeader(
        (int)settings.HeaderPosition,
        1);// settings.HasMultiLineHeader ? 2 : 1);
    }

    public static void SetTextFormat(this TableAnnotation table, Settings.TextFormat format)
    {
        var textFormat = table.GetTextFormat();

        textFormat.TypeFaceName = format.Schriftart;
        textFormat.CharHeight = format.Texthöhe / 1000;
        textFormat.Bold = format.Fett;
        textFormat.Underline = format.Unterstrichen;
        textFormat.Strikeout = format.Durchgestrichen;
        textFormat.Italic = format.Kursiv;

        table.SetTextFormat(false, textFormat);
    }

    public static void SetCellTextFormat(this TableAnnotation table, int row, int column, Settings.TextFormat format)
    {
        var textFormat = table.GetTextFormat();

        textFormat.TypeFaceName = format.Schriftart;
        textFormat.CharHeight = format.Texthöhe / 1000;
        textFormat.Bold = format.Fett;
        textFormat.Underline = format.Unterstrichen;
        textFormat.Strikeout = format.Durchgestrichen;
        textFormat.Italic = format.Kursiv;

        table.SetCellTextFormat(row, column, false, textFormat);
    }

    public static Size GetTableSize(this ITableAnnotation table)
    {
        var width = 0.0;
        for (int column = 0; column < table.ColumnCount; column++)
        {
            width += table.GetColumnWidth(column);
        }

        var height = 0.0;
        for (int row = 0; row < table.RowCount; row++)
        {
            height += table.GetRowHeight(row);
        }

        return new Size(width, height);
    }
}
