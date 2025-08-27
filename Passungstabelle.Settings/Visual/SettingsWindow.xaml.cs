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
}