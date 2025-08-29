namespace Passungstabelle.Settings;

using Passungstabelle.CSharp;
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
        this.ViewModel.Initialize();
    }

    public MainViewModel ViewModel => (MainViewModel)this.DataContext;
}