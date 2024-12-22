// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.MarkupExtensions
{
    public class ThicknessExtension : MarkupExtension
    {
        public ThicknessExtension()
        {
        }

        public ThicknessExtension(ThicknessSize size) => Size = size;

        public ThicknessExtension(ThicknessSize size, ThicknessDirection direction)
        {
            Size = size;
            Direction = direction;
        }

        [ConstructorArgument("size")]
        public ThicknessSize Size { get; set; } = ThicknessSize.None;

        [ConstructorArgument("direction")]
        public ThicknessDirection Direction { get; set; } = ThicknessDirection.All;

        public double? Left { get; set; }

        public double? Top { get; set; }

        public double? Right { get; set; }

        public double? Bottom { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var top = GetSize(Top, Direction is ThicknessDirection.All or ThicknessDirection.Vertical or ThicknessDirection.Top);
            var bottom = GetSize(Bottom, Direction is ThicknessDirection.All or ThicknessDirection.Vertical or ThicknessDirection.Bottom);
            var left = GetSize(Left, Direction is ThicknessDirection.All or ThicknessDirection.Horizontal or ThicknessDirection.Left);
            var right = GetSize(Right, Direction is ThicknessDirection.All or ThicknessDirection.Horizontal or ThicknessDirection.Right);

            return new Thickness(left, top, right, bottom);

            double GetSize(double? prioritySize, bool condition) => prioritySize ?? (condition ? (int)Size : 0);
        }
    }
}
