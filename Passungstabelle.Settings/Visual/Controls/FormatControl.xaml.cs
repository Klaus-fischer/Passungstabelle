// <copyright file="FormatControl.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Passungstabelle.Settings;

/// <summary>
/// Code behind for FormatControl.xaml.
/// </summary>
public partial class FormatControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormatControl"/> class.
    /// </summary>
    public FormatControl()
    {
        this.InitializeComponent();
    }

    private void OnPreviewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is TableInsertPoint point && sender is FrameworkElement rectangle)
        {
            (rectangle.VerticalAlignment, rectangle.HorizontalAlignment) =
                point switch
                {
                    TableInsertPoint.TopLeft => (VerticalAlignment.Top, HorizontalAlignment.Left),
                    TableInsertPoint.BottomLeft => (VerticalAlignment.Bottom, HorizontalAlignment.Left),
                    TableInsertPoint.TopRight => (VerticalAlignment.Top, HorizontalAlignment.Right),
                    TableInsertPoint.BottomRight => (VerticalAlignment.Bottom, HorizontalAlignment.Right),
                    _ => (VerticalAlignment.Top, HorizontalAlignment.Right)
                };
        }
    }
}

