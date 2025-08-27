// <copyright file="GeneralControl.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Passungstabelle.Settings;

/// <summary>
/// Code behind for GeneralControl.xaml.
/// </summary>
public partial class GeneralControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralControl"/> class.
    /// </summary>
    public GeneralControl()
    {
        this.InitializeComponent();
    }

    private void OnLanguageSelected(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox cb)
        {
            if (cb.SelectedItem is ComboBoxItem selected && selected.Content is string language)
            {
                ResourceLocater.Current.ChangeLanguage(language);
            }
        }
    }

    private readonly Guid CommonFolderDialogGuid = new("39754DB2-8D6B-4E91-8E23-28C020AE2EE2");
    private readonly Guid LogFileDialogGuid = new("82F6FFFC-74BF-46BF-964A-EA6FF6F498B1");


    private void SearchCommonLocation(object sender, RoutedEventArgs e)
    {
        SaveFileDialog ofd = new SaveFileDialog
        {
            AddToRecent = true,
            FileName = "Diesen Ordner verwenden",
            Filter = "XML-Dateien|*.XML",
            ClientGuid = CommonFolderDialogGuid,
        };

        if (ofd.ShowDialog(Application.Current.MainWindow) == true)
        {

        }

    }

    private void SearchLogFilePath(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog()
        {
            AddToRecent = true,
            Filter = "Log-Dateien|*.LOG;*.txt",
            ClientGuid = LogFileDialogGuid,
        };

        if (sfd.ShowDialog(Application.Current.MainWindow) == true)
        {

        }
    }
}

