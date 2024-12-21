// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Extended window is designed to 
/// </summary>
public class ExtendedView : ContentControl
{
    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        ExtendedWindow.IsTitleBarVisibleProperty.AddOwner<ExtendedView>();

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public static readonly StyledProperty<object?> LeftContentProperty =
        ExtendedWindow.LeftContentProperty.AddOwner<ExtendedView>();

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty =
        ExtendedWindow.RightContentProperty.AddOwner<ExtendedView>();

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<object?> TitleBarContentProperty =
        ExtendedWindow.TitleBarContentProperty.AddOwner<ExtendedView>();

    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    public static readonly StyledProperty<Thickness> TitleBarMarginProperty =
        ExtendedWindow.TitleBarMarginProperty.AddOwner<ExtendedView>();

    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }
}
