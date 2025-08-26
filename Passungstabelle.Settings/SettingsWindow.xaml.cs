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
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.Content is string culture)
        {
            ResourceLocater.Current.ChangeLanguage(culture);
        }
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