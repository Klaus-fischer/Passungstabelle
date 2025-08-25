// <copyright file="ObjectExtensions" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp
{
    using System.Linq;

    internal static class ObjectExtensions
    {
        public static T? As<T>(this object obj)
        {
            if (obj is T typedObj)
            {
                return typedObj;
            }

            return default;
        }

        public static T[] AsArrayOfType<T>(this object obj)
        {
            if (obj is null)
            {
                return [];
            }

            if (obj is not object[] array)
            {
                return [];
            }

            return array.OfType<T>().ToArray();
        }
    }
}
