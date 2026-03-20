namespace Passungstabelle.Settings;

using System.Collections.Generic;
using System.ComponentModel;

internal interface ISelectedItemHost<T> : INotifyPropertyChanged
    where T : new()
{
    T SelectedItem { get; set; }

    IList<T> Collection { get; }
}
