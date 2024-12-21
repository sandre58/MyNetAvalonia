// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.EventArgs;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.Models;

namespace MyNet.Avalonia.Controls;

[TemplatePart(PART_CloseButton, typeof(Button))]
public abstract class DrawerControlBase : OverlayFeedbackElement
{
    public const string PART_CloseButton = "PART_CloseButton";

    protected internal Button? _closeButton;

    public static readonly StyledProperty<Position> PositionProperty =
        AvaloniaProperty.Register<DrawerControlBase, Position>(
            nameof(Position), defaultValue: Position.Right);

    public static readonly StyledProperty<bool> CanResizeProperty = AvaloniaProperty.Register<DrawerControlBase, bool>(
        nameof(CanResize));

    public bool CanResize
    {
        get => GetValue(CanResizeProperty);
        set => SetValue(CanResizeProperty, value);
    }

    public Position Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<DrawerControlBase, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    internal bool? IsCloseButtonVisible { get; set; }

    protected internal bool CanLightDismiss { get; set; }

    static DrawerControlBase() => DataContextProperty.Changed.AddClassHandler<DrawerControlBase, object?>((o, e) => o.OnDataContextChange(e));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseButtonClick, _closeButton);
        _closeButton = e.NameScope.Find<Button>(PART_CloseButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClick, _closeButton);
    }

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext)
        {
            oldContext.CloseRequest -= OnContextRequestClose;
        }
        if (args.NewValue.Value is IDialogViewModel newContext)
        {
            newContext.CloseRequest += OnContextRequestClose;
        }
    }

    private void OnContextRequestClose(object? sender, object? e) => RaiseEvent(new ResultEventArgs(ClosedEvent, e));

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e) => Close();

    public override void Close()
    {
        if (DataContext is IDialogViewModel context)
        {
            context.Close();
        }
        else
        {
            RaiseEvent(new ResultEventArgs(ClosedEvent, null));
        }
    }
}
