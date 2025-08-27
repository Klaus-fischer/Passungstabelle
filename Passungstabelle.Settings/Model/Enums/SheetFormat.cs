// <copyright file="FormatCollection" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using SolidWorks.Interop.swconst;

public enum SheetFormat
{
    All = swDwgPaperSizes_e.swDwgPapersUserDefined,
    A0 = swDwgPaperSizes_e.swDwgPaperA0size,
    A1 = swDwgPaperSizes_e.swDwgPaperA1size,
    A2 = swDwgPaperSizes_e.swDwgPaperA2size, 
    A3 = swDwgPaperSizes_e.swDwgPaperA3size,
    A4 = swDwgPaperSizes_e.swDwgPaperA4size,
    A4V = swDwgPaperSizes_e.swDwgPaperA4sizeVertical,
}
