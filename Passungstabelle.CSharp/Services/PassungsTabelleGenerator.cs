// <copyright file="PassungsTabelleGenerator" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

internal class PassungsTabelleGenerator(GeneralSettings settings, TableSettings tableSettings)
{
    private readonly GeneralSettings settings = settings;
    private readonly TableSettings tableSettings = tableSettings;

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
        var passungen = this.CollectPassungen(drawing, sheet);
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
        var collection = new PassungEntityCollection();
        sheetAnalyzer.GetPassungsEntities(ref collection);
        return collection.BuildTable();
    }
}
