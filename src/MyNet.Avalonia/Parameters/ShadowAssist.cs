// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Parameters
{
    public static class ShadowProvider
    {
        public static Color ShadowColor { get; set; } = Color.FromArgb(76, 0, 0, 0);

        public static BoxShadows ToBoxShadows(this ShadowDepth shadowDepth, Color? overrideColor = null)
            => shadowDepth switch
            {
                ShadowDepth.Depth0 => new BoxShadows(new BoxShadow()),
                ShadowDepth.Depth1 => new BoxShadows(new BoxShadow
                { Blur = 5, OffsetX = 1, OffsetY = 1, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.Depth2 => new BoxShadows(new BoxShadow
                { Blur = 8, OffsetX = 1.5, OffsetY = 1.5, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.Depth3 => new BoxShadows(new BoxShadow
                { Blur = 14, OffsetX = 4.5, OffsetY = 4.5, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.Depth4 => new BoxShadows(new BoxShadow
                { Blur = 25, OffsetX = 8, OffsetY = 8, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.Depth5 => new BoxShadows(new BoxShadow
                { Blur = 35, OffsetX = 13, OffsetY = 13, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.CenterDepth1 => new BoxShadows(new BoxShadow
                { Blur = 5, OffsetY = 1, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.CenterDepth2 => new BoxShadows(new BoxShadow
                { Blur = 8, OffsetY = 1.5, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.CenterDepth3 => new BoxShadows(new BoxShadow
                { Blur = 14, OffsetY = 4.5, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.CenterDepth4 => new BoxShadows(new BoxShadow
                { Blur = 25, OffsetY = 8, Color = overrideColor ?? ShadowColor }),
                ShadowDepth.CenterDepth5 => new BoxShadows(new BoxShadow
                { Blur = 35, OffsetY = 13, Color = overrideColor ?? ShadowColor }),
                _ => throw new ArgumentOutOfRangeException(nameof(shadowDepth))
            };
    }

    public static class ShadowAssist
    {
        public static readonly AvaloniaProperty<ShadowDepth> ShadowDepthProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ShadowDepth>("ShadowDepth", typeof(ShadowAssist));

        public static readonly AvaloniaProperty<bool> DarkenProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("Darken", typeof(ShadowAssist));

        static ShadowAssist()
        {
            ShadowDepthProperty.Changed.Subscribe(ShadowDepthChangedCallback);
            DarkenProperty.Changed.Subscribe(DarkenPropertyChangedCallback);
        }

        public static void SetShadowDepth(AvaloniaObject element, ShadowDepth value) => element.SetValue(ShadowDepthProperty, value);

        public static ShadowDepth GetShadowDepth(AvaloniaObject element) => element.GetValue<ShadowDepth>(ShadowDepthProperty);

        public static void SetDarken(AvaloniaObject element, bool value) => element.SetValue(DarkenProperty, value);

        public static bool GetDarken(AvaloniaObject element) => element.GetValue<bool>(DarkenProperty);

        private static void ShadowDepthChangedCallback(AvaloniaPropertyChangedEventArgs args)
        {
            if (args.Sender is Border border)
            {
                border.BoxShadow = (args.NewValue as ShadowDepth? ?? ShadowDepth.Depth0).ToBoxShadows();
            }
        }

        private static void DarkenPropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
        {
            if (obj.Sender is not Border border)
                return;

            var targetBoxShadows = (bool?)obj.NewValue == true
                ? GetShadowDepth(border).ToBoxShadows(Colors.Black)
                : GetShadowDepth(border).ToBoxShadows();

            border.SetValue(Border.BoxShadowProperty, targetBoxShadows);
        }
    }
}
