// <copyright file="GlobalLog" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings
{
    using Microsoft.Extensions.Logging;

    internal interface ILogging
    {
        static abstract ILogger Default { get; set; }
    }
}