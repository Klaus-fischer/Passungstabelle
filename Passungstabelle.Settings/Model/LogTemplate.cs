// <copyright file="Resources" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

public struct LogTemplate(string template)
{
    public string Template { get; } = template;

    public static explicit operator LogTemplate(string template) => new LogTemplate(template);
}
