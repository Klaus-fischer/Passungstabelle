// <copyright file="UiTextExtension" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

public enum UiText
{
    ButtonAddText,
    ButtonDeleteText,
    ButtonUpdateText,
    GroupCentralLocationHeader,
    GroupFormatSettingsHeader,
    GroupGeneralHeader,
    GroupInsertPositionsHeader,
    GroupLogLocationHeader,
    GroupMarginHeader,
    GroupOffsetHeader,
    GroupSavedFormatsHeader,
    GroupUsersettingsHeader,
    GroupZoneHeader,
    OptionAfterRebuild,
    OptionBeforeSave,
    OptionBottomLeft,
    OptionBottomRight,
    OptionCreateLogFile,
    OptionNameHeader,
    OptionOnlyOnFirstSheet,
    OptionRemoveAtAllPages,
    OptionSheetFormatHeader,
    OptionSuppressMessages,
    OptionTopLeft,
    OptionTopRight,
    OptionUseCentralLocation,
    OptionUseEvents,
    OptionUsePlusSign,
    SpracheHeader,
    TabFormatHeader,
    TabGeneralHeader,
    TabTableHeader,
    TabTemplateHeader,
    WindowTitle,
}

public class UiTextExtension : MarkupExtension
{
    public UiTextExtension(UiText key)
    {
        this.Key = key;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public UiText Key { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding($"[{Key}]")
        {
            Source = ResourceLocater.Current,
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}

