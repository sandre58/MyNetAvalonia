// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters.Internal;

internal class NavigationMenuMarginConverter : IMultiValueConverter
{
    public static NavigationMenuMarginConverter Default { get; } = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        => values[0] is double indent && values[1] is int level
            ? new Thickness(indent * (level - 1), 0, 0, 0)
            : AvaloniaProperty.UnsetValue;
}
