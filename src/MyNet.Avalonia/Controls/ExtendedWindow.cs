// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Extended Window is an advanced Window control that provides a lot of features and customization options.
/// </summary>
public class ExtendedWindow : Window
{
    private bool _canClose = false;

    protected override Type StyleKeyOverride => typeof(ExtendedWindow);

    public static readonly StyledProperty<bool> IsFullScreenButtonVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsFullScreenButtonVisible));

    public bool IsFullScreenButtonVisible
    {
        get => GetValue(IsFullScreenButtonVisibleProperty);
        set => SetValue(IsFullScreenButtonVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsMinimizeButtonVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsMinimizeButtonVisible), true);

    public bool IsMinimizeButtonVisible
    {
        get => GetValue(IsMinimizeButtonVisibleProperty);
        set => SetValue(IsMinimizeButtonVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsRestoreButtonVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsRestoreButtonVisible), true);

    public bool IsRestoreButtonVisible
    {
        get => GetValue(IsRestoreButtonVisibleProperty);
        set => SetValue(IsRestoreButtonVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsCloseButtonVisible), true);

    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsTitleBarVisible), true);

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsManagedResizerVisibleProperty = AvaloniaProperty.Register<ExtendedWindow, bool>(
        nameof(IsManagedResizerVisible));

    public bool IsManagedResizerVisible
    {
        get => GetValue(IsManagedResizerVisibleProperty);
        set => SetValue(IsManagedResizerVisibleProperty, value);
    }

    public static readonly StyledProperty<object?> TitleBarContentProperty = AvaloniaProperty.Register<ExtendedWindow, object?>(
        nameof(TitleBarContent));

    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    public static readonly StyledProperty<object?> LeftContentProperty = AvaloniaProperty.Register<ExtendedWindow, object?>(
        nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty = AvaloniaProperty.Register<ExtendedWindow, object?>(
        nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<Thickness> TitleBarMarginProperty = AvaloniaProperty.Register<ExtendedWindow, Thickness>(
        nameof(TitleBarMargin));

    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }

    protected virtual async Task<bool> CanClose() => await Task.FromResult(true);

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        VerifyAccess();
        if (!_canClose)
        {
            e.Cancel = true;
            _canClose = await CanClose();
            if (_canClose)
            {
                Close();
                return;
            }
        }
        base.OnClosing(e);
    }
}
