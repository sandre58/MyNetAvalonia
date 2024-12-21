// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls;

[PseudoClasses(PseudoClassName.Active)]
public class OutlinedIcon : PathIcon
{
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<OutlinedIcon, bool>(
        nameof(IsActive), defaultBindingMode: BindingMode.TwoWay);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveBackgroundProperty = AvaloniaProperty.Register<OutlinedIcon, IBrush?>(
        nameof(ActiveBackground));

    public IBrush? ActiveBackground
    {
        get => GetValue(ActiveBackgroundProperty);
        set => SetValue(ActiveBackgroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ActiveBorderBrushProperty = AvaloniaProperty.Register<OutlinedIcon, IBrush?>(
        nameof(ActiveBorderBrush));

    public IBrush? ActiveBorderBrush
    {
        get => GetValue(ActiveBorderBrushProperty);
        set => SetValue(ActiveBorderBrushProperty, value);
    }

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<OutlinedIcon, double>(
            nameof(StrokeThickness));
    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    static OutlinedIcon()
    {
        AffectsRender<OutlinedIcon>(
            DataProperty,
            BorderBrushProperty,
            BackgroundProperty,
            ActiveBackgroundProperty,
            ActiveBorderBrushProperty);
        IsActiveProperty.AffectsPseudoClass<OutlinedIcon>(PseudoClassName.Active);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        PseudoClasses.Set(PseudoClassName.Active, IsActive);
    }
}
