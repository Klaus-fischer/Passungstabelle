namespace Passungstabelle.Settings;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        this.DataContext = new MainViewModel();
    }
}