// <copyright file="PassungsTabelleGenerator" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;

internal class PassungsTabelleGenerator(SettingsLoader  loader)
{
    private readonly SettingsLoader loader = loader;
    private readonly GeneralSettings settings = loader.Settings;

    public void Execute(IDrawingDoc drawing)
    {
        if (settings.LöschenAufRestlichenBlättern)
        {
            this.AlleLöschen(drawing);
        }

        foreach (var sheet in drawing.GetSheets())
        {
            this.Execute(drawing, sheet);

            if (settings.NurAufErstemBlatt)
            {
                break;
            }
        }
    }

    public void Execute(IDrawingDoc drawing, ISheet sheet)
    {
        this.RemoveTable(drawing, sheet);
        var zeilen = this.CollectPassungen(drawing, sheet);
        this.InsertTable((IModelDoc2)drawing, sheet, zeilen);
    }


    private void AlleLöschen(IDrawingDoc drawing)
    {
        foreach (var sheet in drawing.GetSheets())
        {
            RemoveTable(drawing, sheet);
        }
    }

    private void RemoveTable(IDrawingDoc drawing, ISheet sheet)
    {
        var modelDoc2 = (IModelDoc2)drawing;
        drawing.ActivateSheet(sheet.GetName());

        if (modelDoc2.Extension.SelectByID2("PASSUNGSTABELLE@" + sheet.GetName(), "ANNOTATIONTABLES", 0d, 0d, 0d, false, 0, null, 0))
        {
            modelDoc2.EditDelete();
        }
    }

    private TabellenZeile[] CollectPassungen(IDrawingDoc drawing, ISheet sheet)
    {
        var sheetAnalyzer = new SheetAnalyser(drawing, sheet);
        return sheetAnalyzer.GetPassungsEntities();
    }

    private void InsertTable(IModelDoc2 drawing, ISheet sheet, TabellenZeile[] zeilen)
    {
        var tableSettings = loader.GetTableSettings(sheet);
        var formatSettings = loader.GetFormat(sheet, tableSettings);

        var tableWriter = new TableWriter(this.settings, tableSettings, formatSettings);
        tableWriter.InsertTable(drawing, sheet, zeilen);
    }
}
