// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.MarkupExtensions
{
    public class BrushExtension : MarkupExtension
    {
        public BrushExtension(string path) => Path = path;

        [ConstructorArgument("path")]
        public string Path { get; set; }

        public double? Opacity { get; set; }

        public bool? Contrast { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var converter = BrushConverter.Default;
            object? converterParameter = null;

            if (Opacity.HasValue)
            {
                converter = BrushConverter.Opacity;
                converterParameter = Opacity.Value;
            }
            else if (Contrast.HasValue)
            {
                converter = BrushConverter.Contrast;
            }

            return new Binding(Path)
            {
                Converter = converter,
                ConverterParameter = converterParameter
            };
        }
    }
}
