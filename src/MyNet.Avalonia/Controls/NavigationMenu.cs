// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls;

[PseudoClasses(PC_HorizontalCollapsed)]
public class NavigationMenu : ItemsControl
{
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";

    public static readonly StyledProperty<object?> SelectedItemProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IBinding?> IconProperty =
        AvaloniaProperty.Register<NavigationMenu, IBinding?>(
            nameof(Icon));

    public static readonly StyledProperty<IBinding?> HeaderBindingProperty =
        AvaloniaProperty.Register<NavigationMenu, IBinding?>(
            nameof(HeaderBinding));

    public static readonly StyledProperty<IBinding?> SubMenuProperty =
        AvaloniaProperty.Register<NavigationMenu, IBinding?>(
            nameof(SubMenu));

    public static readonly StyledProperty<IBinding?> CommandProperty =
        AvaloniaProperty.Register<NavigationMenu, IBinding?>(
            nameof(Command));

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<NavigationMenu, IDataTemplate?>(
            nameof(HeaderTemplate));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<NavigationMenu, IDataTemplate?>(
            nameof(IconTemplate));

    public static readonly StyledProperty<double> SubMenuIndentProperty = AvaloniaProperty.Register<NavigationMenu, double>(
        nameof(SubMenuIndent));

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        AvaloniaProperty.Register<NavigationMenu, bool>(
            nameof(IsHorizontalCollapsed));

    public static readonly StyledProperty<object?> HeaderProperty =
        HeaderedContentControl.HeaderProperty.AddOwner<NavigationMenu>();

    public static readonly StyledProperty<object?> FooterProperty = AvaloniaProperty.Register<NavigationMenu, object?>(
        nameof(Footer));

    public static readonly StyledProperty<double> ExpandWidthProperty = AvaloniaProperty.Register<NavigationMenu, double>(
        nameof(ExpandWidth), double.NaN);

    public static readonly StyledProperty<double> CollapseWidthProperty = AvaloniaProperty.Register<NavigationMenu, double>(
        nameof(CollapseWidth), double.NaN);

    public static readonly AttachedProperty<bool> CanToggleProperty =
        AvaloniaProperty.RegisterAttached<NavigationMenu, InputElement, bool>("CanToggle");

    public static readonly RoutedEvent<SelectionChangedEventArgs> SelectionChangedEvent =
        RoutedEvent.Register<NavigationMenu, SelectionChangedEventArgs>(nameof(SelectionChanged), RoutingStrategies.Bubble);

    private bool _updateFromUI;

    static NavigationMenu()
    {
        SelectedItemProperty.Changed.AddClassHandler<NavigationMenu, object?>((o, e) => o.OnSelectedItemChange(e));
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavigationMenu>(PC_HorizontalCollapsed);
        CanToggleProperty.Changed.AddClassHandler<InputElement, bool>(OnInputRegisteredAsToggle);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? HeaderBinding
    {
        get => GetValue(HeaderBindingProperty);
        set => SetValue(HeaderBindingProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? SubMenu
    {
        get => GetValue(SubMenuProperty);
        set => SetValue(SubMenuProperty, value);
    }

    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public IBinding? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    ///     Header Template is used for MenuItem headers, not menu header.
    /// </summary>
    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public double ExpandWidth
    {
        get => GetValue(ExpandWidthProperty);
        set => SetValue(ExpandWidthProperty, value);
    }

    public double CollapseWidth
    {
        get => GetValue(CollapseWidthProperty);
        set => SetValue(CollapseWidthProperty, value);
    }

    public static void SetCanToggle(InputElement obj, bool value) => obj.SetValue(CanToggleProperty, value);

    public static bool GetCanToggle(InputElement obj) => obj.GetValue(CanToggleProperty);

    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    private static void OnInputRegisteredAsToggle(InputElement input, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue.Value)
            input.AddHandler(PointerPressedEvent, OnElementToggle);
        else
            input.RemoveHandler(PointerPressedEvent, OnElementToggle);
    }

    private static void OnElementToggle(object? sender, RoutedEventArgs args)
    {
        if (sender is not InputElement input) return;
        var nav = input.FindLogicalAncestorOfType<NavigationMenu>();
        if (nav is null) return;
        var collapsed = nav.IsHorizontalCollapsed;
        nav.IsHorizontalCollapsed = !collapsed;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        TryToSelectItem(SelectedItem);
    }

    /// <summary>
    ///     this implementation only works in the case that only leaf menu item is allowed to select. It will be changed if we
    ///     introduce parent level selection in the future.
    /// </summary>
    /// <param name="args"></param>
    private void OnSelectedItemChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        var a = new SelectionChangedEventArgs(
            SelectionChangedEvent,
            new[] { args.OldValue.Value },
            new[] { args.NewValue.Value });
        if (_updateFromUI)
        {
            RaiseEvent(a);
            return;
        }

        var newValue = args.NewValue.Value;
        if (newValue is null)
        {
            ClearAll();
            RaiseEvent(a);
            return;
        }
        var found = TryToSelectItem(newValue);
        if (!found) ClearAll();
        RaiseEvent(a);
    }

    private bool TryToSelectItem(object? item)
    {
        if (item is null) return false;
        var leaves = GetLeafMenus();
        var found = false;
        foreach (var leaf in leaves)
            if (leaf == item || leaf.DataContext == item)
            {
                leaf.SelectItem(leaf);
                found = true;
            }

        return found;
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) => NeedsContainer<NavigationMenuItem>(item, out recycleKey);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new NavigationMenuItem();

    internal void SelectItem(NavigationMenuItem item, NavigationMenuItem parent)
    {
        _updateFromUI = true;
        foreach (var child in LogicalChildren)
        {
            if (Equals(child, parent)) continue;
            if (child is NavigationMenuItem navMenuItem) navMenuItem.ClearSelection();
        }

        SelectedItem = item.DataContext is not null && item.DataContext != DataContext ? item.DataContext : item;
        item.BringIntoView();
        _updateFromUI = false;
    }

    private IEnumerable<NavigationMenuItem> GetLeafMenus()
    {
        foreach (var child in LogicalChildren)
            if (child is NavigationMenuItem item)
            {
                var leafs = item.GetLeafMenus();
                foreach (var leaf in leafs) yield return leaf;
            }
    }

    private void ClearAll()
    {
        foreach (var child in LogicalChildren)
            if (child is NavigationMenuItem item)
                item.ClearSelection();
    }
}
