// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Parameters
{
    public static class ThemeAssist
    {
        #region HoverBackground

        /// <summary>
        /// Provides HoverBackground Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> HoverBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBackground", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="HoverBackgroundProperty"/>.</param>
        public static void SetHoverBackground(StyledElement element, IBrush? value) => element.SetValue(HoverBackgroundProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="HoverBackgroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetHoverBackground(StyledElement element) => element.GetValue(HoverBackgroundProperty);

        #endregion

        #region HoverBorderBrush

        /// <summary>
        /// Provides HoverBorderBrush Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> HoverBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverBorderBrush", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="HoverBorderBrushProperty"/>.</param>
        public static void SetHoverBorderBrush(StyledElement element, IBrush? value) => element.SetValue(HoverBorderBrushProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="HoverBorderBrushProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetHoverBorderBrush(StyledElement element) => element.GetValue(HoverBorderBrushProperty);

        #endregion

        #region HoverForeground

        /// <summary>
        /// Provides HoverForeground Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> HoverForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("HoverForeground", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="HoverForegroundProperty"/>.</param>
        public static void SetHoverForeground(StyledElement element, IBrush? value) => element.SetValue(HoverForegroundProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="HoverForegroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetHoverForeground(StyledElement element) => element.GetValue(HoverForegroundProperty);

        #endregion

        #region ActiveBackground

        /// <summary>
        /// Provides ActiveBackground Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> ActiveBackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBackground", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="ActiveBackgroundProperty"/>.</param>
        public static void SetActiveBackground(StyledElement element, IBrush? value) => element.SetValue(ActiveBackgroundProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveBackgroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetActiveBackground(StyledElement element) => element.GetValue(ActiveBackgroundProperty);

        #endregion

        #region ActiveBorderBrush

        /// <summary>
        /// Provides ActiveBorderBrush Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveBorderBrush", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="ActiveBorderBrushProperty"/>.</param>
        public static void SetActiveBorderBrush(StyledElement element, IBrush? value) => element.SetValue(ActiveBorderBrushProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveBorderBrushProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetActiveBorderBrush(StyledElement element) => element.GetValue(ActiveBorderBrushProperty);

        #endregion

        #region ActiveForeground

        /// <summary>
        /// Provides ActiveForeground Property for attached ThemeAssist element.
        /// </summary>
        public static readonly AttachedProperty<IBrush?> ActiveForegroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush?>("ActiveForeground", typeof(ThemeAssist));

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="ActiveForegroundProperty"/>.</param>
        public static void SetActiveForeground(StyledElement element, IBrush? value) => element.SetValue(ActiveForegroundProperty, value);

        /// <summary>
        /// Accessor for Attached  <see cref="ActiveForegroundProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static IBrush? GetActiveForeground(StyledElement element) => element.GetValue(ActiveForegroundProperty);

        #endregion
    }
}
