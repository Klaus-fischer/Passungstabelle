// <copyright file="TemplateViewModel" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public class FormatSelector(FormatSettings format) : BaseViewModel
{
    private FormatSettings format = format;
    private bool _IsSelected = default;

    public FormatSettings Format
    {
        get => this.format;
        set => this.Set(ref format, value);
    }

    public bool IsSelected
    {
        get => this._IsSelected;
        set => this.Set(ref _IsSelected, value);
    }
}
