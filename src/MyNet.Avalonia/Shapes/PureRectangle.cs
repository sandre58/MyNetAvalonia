// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace MyNet.Avalonia.Shapes;

/// <summary>
/// Provide the most simplified shape implementation. This is a rectangle with a background, without border and corner radius.
/// </summary>
public class PureRectangle : Control
{
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        Border.BackgroundProperty.AddOwner<PureRectangle>();

    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    static PureRectangle() => AffectsRender<PureRectangle>(BackgroundProperty);

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        context.DrawRectangle(Background, null, new Rect(Bounds.Size));
    }
}
