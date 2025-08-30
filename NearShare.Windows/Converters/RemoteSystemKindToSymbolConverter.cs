// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Data;

namespace NearShare.Windows.Converters;

/// <summary>
/// Converter to convert Device Type to Icon
/// </summary>
public sealed partial class RemoteSystemKindToSymbolConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (string)value switch
        {
            "Desktop" => "\xE770",
            "Phone" or "Unknown" => "\xE8EA",
            "Xbox" => "\xE990",
            "Tablet" => "\xE70A",
            "Laptop" => "\xE7F8",
            "Holographic" => "\xF4BF",
            "Hub" => "\xE8AE",
            "Iot" => "\xF22C",
            _ => "\xE770",
        };
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotImplementedException();
}