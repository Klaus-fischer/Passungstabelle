// <copyright file="GlobalLog" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

internal static class GlobalLog
{
    public static ILogger Default { get; set; } = new DebugLogger();

    private class DebugLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null; // No scope management in this simple logger
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true; // Always enabled for simplicity
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            Debug.WriteLine($"[{logLevel}] {message}");
        }
    }
}
