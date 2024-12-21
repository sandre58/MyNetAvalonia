// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Converters
{
    public enum DateTimeToStringConverterKind
    {
        Default,

        Current,

        Local,

        Utc,
    }

    public sealed class DateTimeToStringConverter : IValueConverter, IMultiValueConverter
    {
        private readonly DateTimeToStringConverterKind _target;
        private readonly LetterCasing _casing;

        public static readonly DateTimeToStringConverter Default = new(DateTimeToStringConverterKind.Default);
        public static readonly DateTimeToStringConverter ToLocal = new(DateTimeToStringConverterKind.Local);
        public static readonly DateTimeToStringConverter ToUtc = new(DateTimeToStringConverterKind.Utc);
        public static readonly DateTimeToStringConverter ToCurrent = new(DateTimeToStringConverterKind.Current);

        public DateTimeToStringConverter(DateTimeToStringConverterKind target, LetterCasing letterCasing = LetterCasing.Normal)
        {
            _target = target;
            _casing = letterCasing;
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateTime? dateToConvert = null;

            if (value is DateTime date)
                dateToConvert = date;

            if (value is DateOnly date1)
                dateToConvert = date1.BeginningOfDay();

            if (value is TimeSpan time)
                dateToConvert = DateTime.Today.At(time);

            if (value is TimeOnly time1)
                dateToConvert = DateTime.Today.At(time1);

            if (!dateToConvert.HasValue) return AvaloniaProperty.UnsetValue;

            var finalDate = _target switch
            {
                DateTimeToStringConverterKind.Utc => dateToConvert.Value.ToUniversalTime(),
                DateTimeToStringConverterKind.Local => dateToConvert.Value.ToLocalTime(),
                DateTimeToStringConverterKind.Current => GlobalizationService.Current.Convert(dateToConvert.Value),
                _ => dateToConvert.Value
            };

            var format = parameter is not null ? TranslateDatePattern(parameter.ToString().OrEmpty(), culture) : null;
            return finalDate.ToString(format, culture)?.ApplyCase(_casing);
        }

        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => Convert(values.Count > 0 ? values[0] : null, targetType, values.Count > 1 ? values[1] : null, culture);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();

        private static string? TranslateDatePattern(string key, CultureInfo culture)
        {
            var format = culture.DateTimeFormat;
            var prop = format.GetType().GetProperty(key);
            return prop != null ? prop.GetValue(format)?.ToString() ?? string.Empty : TranslationService.Get(culture)[key];
        }
    }
}
