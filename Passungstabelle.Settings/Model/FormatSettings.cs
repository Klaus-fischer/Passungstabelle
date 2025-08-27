// <copyright file="FormatCollection" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System.Windows;

public class FormatSettings
{
    public string Name { get; set; } = "Default_A4V";

    public SheetFormat SheetFormat { get; set; } = SheetFormat.A4V;

    public TableInsertPoint InsertPoint { get; set; }

    public Thickness Margin { get; set; } = new Thickness(20, 10, 10, 10);

    public string MaxZone { get; set; } = "H6";

    public Vector Offset { get; set; }
}
