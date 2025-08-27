// <copyright file="TableWriter" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

internal class TableWriter(GeneralSettings settings, TableSettings tableSettings, FormatSettings formatSettings)
{
    private const double factor = 1000;
    private readonly GeneralSettings settings = settings;
    private readonly TableSettings tableSettings = tableSettings;
    private readonly FormatSettings formatSettings = formatSettings;
    private readonly SpalteSettings[] spalten = tableSettings.Spalten.Where(o => o.Visible).ToArray();

    public double Schichtdicke { get; set; } = settings.SchichtStärke;

    public void InsertTable(IModelDoc2 modelDoc, ISheet sheet, TabellenZeile[] zeilen)
    {
        var rows = (zeilen.Length * 2) + (this.tableSettings.HasMultiLineHeader ? 2 : 1);
        var position = sheet.GetInsertPoint(this.formatSettings);

        var swTable = modelDoc.Extension.InsertGeneralTableAnnotation(
            false,
            position.X / factor,
            position.Y / factor,
            (int)this.formatSettings.InsertPoint,
            "",
            rows,
            this.tableSettings.Spalten.Where(o => o.Visible).Count());

        var annotation = swTable.GetAnnotation();
        // hide table
        annotation.Visible = (int)swAnnotationVisibilityState_e.swAnnotationHidden;

        for (var i = 0; i < swTable.ColumnCount; i++)
        {
            swTable.SetColumnWidth(
                i,
                1.0,
                (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
        }

        annotation.SetName("PASSUNGSTABELLE");
        swTable.Title = "Passungstabelle";
        swTable.GeneralTableFeature.GetFeature().Name = $"Passungstabelle-{sheet.GetName()}";

        swTable.BorderLineWeight = (int)this.tableSettings.RahmenStrichStärke;
        swTable.GridLineWeight = (int)this.tableSettings.RasterStrichStärke;

        this.GetSchichtdicke(modelDoc);

        this.WriteHeader(swTable);
        this.WriteRows(swTable, zeilen);

        this.FormatColumnWidth(swTable);

        position = this.GetEinfügepunkt(swTable, position);
        annotation.SetPosition2(position.X / factor, position.Y / factor, 0);

        annotation.Visible = (int)swAnnotationVisibilityState_e.swAnnotationVisible;
    }

    private void GetSchichtdicke(IModelDoc2 model)
    {
        bool isSchichtdickeRequired = this.tableSettings.VorbearbeitungsAbmaße.Visible ||
            this.tableSettings.VorbearbeitungsToleranzMitte.Visible;

        if (!isSchichtdickeRequired)
        {
            return;
        }


        if (model.GetStringProperty("Passungstabelle_Schichtdicke") is string value)
        {
            value = value.Replace(',', '.');
            if (double.TryParse(value, CultureInfo.InvariantCulture, out var result))
            {
                this.Schichtdicke = result;
            }
        }

        model.SetStringProperty(
            "Passungstabelle_Schichtdicke",
            this.Schichtdicke.ToString("0.###", CultureInfo.InvariantCulture));

        if (settings.SchichtStärkeAbfragen)
        {
            MessageBox.Show("""
            Die Schichtdicke ist nicht definiert, der Standardwert wird verwendet.
            Bitte den Parameter in Zeichnungseigenschaften aktualisieren und Tabelle neu erzeugen.                
            """);

        }
    }

    private void WriteHeader(TableAnnotation swTable)
    {
        swTable.SetHeaderPosition(this.tableSettings);

        var color = this.tableSettings.HeaderFormat.SwFarbe;
        var row = this.tableSettings.HeaderPosition == HeaderPosition.Oben ? 0 : swTable.RowCount - 1;

        for (int column = 0; column < spalten.Length; column++)
        {
            var spalte = spalten[column];
            var title = spalte.Title;
            if (!string.IsNullOrWhiteSpace(spalte.SubTitle))
            {
                title = $"{title}\n{spalte.SubTitle}";
            }

            swTable.SetColumnTitle2(column, $"<FONT color={color}>{title}", true);
            swTable.SetCellTextFormat(row, column, this.tableSettings.HeaderFormat);
        }
    }

    private void WriteRows(TableAnnotation swTable, TabellenZeile[] zeilen)
    {
        swTable.SetTextFormat(this.tableSettings.TextFormat);
        var rowStart = this.tableSettings.HeaderPosition == HeaderPosition.Oben ? 1 : 0;

        for (int row = 0; row < zeilen.Length; row++)
        {
            var zeile = zeilen[row];
            this.WriteRowText(swTable, row * 2 + rowStart, zeile);
        }
    }

    private void WriteRowText(TableAnnotation table, int row, TabellenZeile zeile)
    {
        for (int column = 0; column < spalten.Length; column++)
        {
            var content = Format(zeile, spalten[column]);

            if (content is [])
            {
                continue;
            }

            if (content.Length == 1)
            {
                table.Text[row, column] = content[0];
                table.Text[row + 1, column] = "";
                table.MergeCells(row, column, row + 1, column);
                continue;
            }

            table.Text[row, column] = content[0];
            table.Text[row + 1, column] = content[1];
        }
    }

    private string[] Format(TabellenZeile zeile, SpalteSettings spalte)
    {
        return spalte.Name switch
        {
            nameof(TableSettings.Maß)
                => FormatNumber([zeile.Maß], zeile.Prefix),
            nameof(TableSettings.Passung)
                => [zeile.Passung],
            nameof(TableSettings.MaßePassung)
                => [zeile.MaßPassung(this.settings.PlusZeichen)],
            nameof(TableSettings.Toleranz)
                => FormatNumber([zeile.ToleranzO, zeile.ToleranzU], "", this.settings.PlusZeichen),
            nameof(TableSettings.Abmaß)
                => FormatNumber([zeile.AbmaßO, zeile.AbmaßU]),
            nameof(TableSettings.AbmaßToleranzMitte)
                => FormatNumber([zeile.AbmaßToleranzMitte]),
            nameof(TableSettings.VorbearbeitungsAbmaße)
                => FormatNumber([zeile.VorbearbeitungAbmaßO(this.Schichtdicke), zeile.VorbearbeitungAbmaßU(this.Schichtdicke)]),
            nameof(TableSettings.VorbearbeitungsToleranzMitte)
                => FormatNumber([zeile.VorbearbeitungAbmaßToleranzMitte(this.Schichtdicke)]),
            _ => []
        };
    }

    public string[] FormatNumber(double[] number, string prefix = "", bool includePlus = false)
    {
        var result = new string[number.Length];

        for (int i = 0; i < number.Length; i++)
        {
            result[i] = includePlus && number[i] > 0
                ? $"{prefix}{number[i]:+0.###}"
                : $"{prefix}{number[i]:0.###}";
        }

        return result;
    }

    private void FormatColumnWidth(TableAnnotation swTable)
    {
        var annotation = swTable.GetAnnotation();
        var display = annotation.GetDisplayData().As<DisplayData>()!;

        for (int column = 0; column < spalten.Length; column++)
        {
            var spalte = this.spalten[column];

            var width = spalte.AutoBreite
                ? this.GetMaxWidth(column, display, swTable)
                : spalte.Breite;

            swTable.SetColumnWidth(column, width, (int)swTableRowColSizeChangeBehavior_e.swTableRowColChange_TableSizeCanChange);
        }
    }

    private double GetMaxWidth(int column, DisplayData display, TableAnnotation swTable)
    {
        var width = 0.0;
        for (int row = 0; row < swTable.RowCount; row++)
        {
            var index = column + row * swTable.ColumnCount;
            var cellWidth = display.GetTextInBoxWidthAtIndex(index);

            width = Math.Max(width, cellWidth);
        }

        return width + 0.001;
    }

    private Point GetEinfügepunkt(TableAnnotation swTable, Point current)
    {
        var size = swTable.GetTableSize();

        var offsetX = current.X;
        var offsetY = current.Y;

        if (formatSettings.InsertPoint == TableInsertPoint.BottomLeft)
        {
            offsetY += size.Height;
        }

        if (formatSettings.InsertPoint == TableInsertPoint.TopRight)
        {
            offsetX -= size.Width;
        }

        if (formatSettings.InsertPoint == TableInsertPoint.BottomRight)
        {
            offsetX -= size.Width;
            offsetY += size.Height;
        }

        return new Point(offsetX, offsetY);
    }
}
