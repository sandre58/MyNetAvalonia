// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Utilities;

namespace MyNet.Avalonia.MarkupExtensions
{
    public class IconExtension : MarkupExtension
    {
        private static readonly string GeometryPattern = $"{MyTheme.ResourcePrefix}.Geometry.{"{0}"}";

        public IconExtension(string data) => Data = data;

        public IconExtension(string data, IconSize size)
        {
            Data = data;
            DefinedSize = size;
        }

        [ConstructorArgument("data")]
        public string Data { get; set; }

        [ConstructorArgument("size")]
        public IconSize DefinedSize { get; set; } = IconSize.Default;

        public double? Size { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = new PathIcon()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var data = new StaticResourceExtension(GeometryPattern.FormatWith(Data)).ProvideValue(serviceProvider);
            result.SetValue(PathIcon.DataProperty, data);
            if (Size.HasValue)
            {
                result.Width = Size.Value;
                result.Height = Size.Value;
            }
            else
                result.Classes.Add(DefinedSize.ToString());

            return result;
        }
    }
}
