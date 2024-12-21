// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls;

[PseudoClasses(PseudoClassName.Active)]
public class TitleBar : ContentControl
{
    private CaptionButtons? _captionButtons;
    private Window? _visualRoot;
    private IDisposable? _activeSubscription;

    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<TitleBar, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<bool> IsTitleVisibleProperty = AvaloniaProperty.Register<TitleBar, bool>(
        nameof(IsTitleVisible));

    public bool IsTitleVisible
    {
        get => GetValue(IsTitleVisibleProperty);
        set => SetValue(IsTitleVisibleProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _captionButtons?.Detach();
        _captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");
        var background = e.NameScope.Get<InputElement>("PART_Background");
        DoubleTappedEvent.AddHandler(OnDoubleTapped, background);
        PointerPressedEvent.AddHandler(OnPointerPressed, background);
        _captionButtons?.Attach(_visualRoot);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _visualRoot = VisualRoot as Window;
        if (_visualRoot is not null)
        {
            _activeSubscription = _visualRoot.GetObservable(WindowBase.IsActiveProperty).Subscribe(isActive => PseudoClasses.Set(PseudoClassName.Active, isActive));
        }
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_visualRoot is not null && _visualRoot.WindowState == WindowState.FullScreen)
        {
            return;
        }
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && e.ClickCount < 2)
        {
            _visualRoot?.BeginMoveDrag(e);
        }
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (_visualRoot is null) return;
        if (!_visualRoot.CanResize) return;
        if (_visualRoot.WindowState == WindowState.FullScreen) return;
        _visualRoot.WindowState = _visualRoot.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _captionButtons?.Detach();
        _activeSubscription?.Dispose();
    }
}
