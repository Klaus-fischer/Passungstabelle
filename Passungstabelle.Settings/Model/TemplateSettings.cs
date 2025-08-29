// <copyright file="TemplateSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public class TemplateSettings
{
    public string TemplateNamePattern { get; set; } = "*";

    public string TableSchemaName { get; set; } = "Default";

    public string[] FormatNames { get; set; } = [];
}
