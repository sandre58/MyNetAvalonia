// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls;

/// <summary>
///     Navigation Menu Item
/// </summary>
[PseudoClasses(PC_Highlighted, PC_HorizontalCollapsed, PC_VerticalCollapsed, PC_FirstLevel, PC_Selector)]
public class NavigationMenuItem : HeaderedItemsControl
{
    public const string PC_Highlighted = ":highlighted";
    public const string PC_FirstLevel = ":first-level";
    public const string PC_HorizontalCollapsed = ":horizontal-collapsed";
    public const string PC_VerticalCollapsed = ":vertical-collapsed";
    public const string PC_Selector = ":selector";

    private static readonly Point InvalidPoint = new(double.NaN, double.NaN);

    public static readonly StyledProperty<object?> IconProperty = AvaloniaProperty.Register<NavigationMenuItem, object?>(
        nameof(Icon));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<NavigationMenuItem, IDataTemplate?>(
            nameof(IconTemplate));

    public static readonly StyledProperty<ICommand?> CommandProperty = Button.CommandProperty.AddOwner<NavigationMenuItem>();

    public static readonly StyledProperty<object?> CommandParameterProperty =
        Button.CommandParameterProperty.AddOwner<NavigationMenuItem>();

    public static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<NavigationMenuItem>();

    public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent =
        RoutedEvent.Register<SelectingItemsControl, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

    public static readonly DirectProperty<NavigationMenuItem, bool> IsHighlightedProperty =
        AvaloniaProperty.RegisterDirect<NavigationMenuItem, bool>(
            nameof(IsHighlighted), o => o.IsHighlighted, (o, v) => o.IsHighlighted = v,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsHorizontalCollapsedProperty =
        NavigationMenu.IsHorizontalCollapsedProperty.AddOwner<NavigationMenuItem>();

    public static readonly StyledProperty<bool> IsVerticalCollapsedProperty =
        AvaloniaProperty.Register<NavigationMenuItem, bool>(
            nameof(IsVerticalCollapsed));

    public static readonly StyledProperty<double> SubMenuIndentProperty =
        NavigationMenu.SubMenuIndentProperty.AddOwner<NavigationMenuItem>();

    internal static readonly DirectProperty<NavigationMenuItem, int> LevelProperty =
        AvaloniaProperty.RegisterDirect<NavigationMenuItem, int>(
            nameof(Level), o => o.Level, (o, v) => o.Level = v);

    public static readonly StyledProperty<bool> IsSeparatorProperty = AvaloniaProperty.Register<NavigationMenuItem, bool>(
        nameof(IsSeparator));

    private bool _isHighlighted;
    private int _level;
    private Panel? _overflowPanel;
    private Point _pointerDownPoint = InvalidPoint;
    private Popup? _popup;

    private NavigationMenu? _rootMenu;

    static NavigationMenuItem()
    {
        PressedMixin.Attach<NavigationMenuItem>();
        FocusableProperty.OverrideDefaultValue<NavigationMenuItem>(true);
        LevelProperty.Changed.AddClassHandler<NavigationMenuItem, int>((item, args) => item.OnLevelChange(args));
        IsHighlightedProperty.AffectsPseudoClass<NavigationMenuItem>(PC_Highlighted);
        IsHorizontalCollapsedProperty.AffectsPseudoClass<NavigationMenuItem>(PC_HorizontalCollapsed);
        IsVerticalCollapsedProperty.AffectsPseudoClass<NavigationMenuItem>(PC_VerticalCollapsed);
        IsSelectedProperty.AffectsPseudoClass<NavigationMenuItem>(PseudoClassName.Selected, IsSelectedChangedEvent);
        IsHorizontalCollapsedProperty.Changed.AddClassHandler<NavigationMenuItem, bool>((item, args) =>
            item.OnIsHorizontalCollapsedChanged(args));
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public bool IsHighlighted
    {
        get => _isHighlighted;
        private set => SetAndRaise(IsHighlightedProperty, ref _isHighlighted, value);
    }

    public bool IsHorizontalCollapsed
    {
        get => GetValue(IsHorizontalCollapsedProperty);
        set => SetValue(IsHorizontalCollapsedProperty, value);
    }

    public bool IsVerticalCollapsed
    {
        get => GetValue(IsVerticalCollapsedProperty);
        set => SetValue(IsVerticalCollapsedProperty, value);
    }

    public double SubMenuIndent
    {
        get => GetValue(SubMenuIndentProperty);
        set => SetValue(SubMenuIndentProperty, value);
    }

    public int Level
    {
        get => _level;
        set => SetAndRaise(LevelProperty, ref _level, value);
    }

    public bool IsSeparator
    {
        get => GetValue(IsSeparatorProperty);
        set => SetValue(IsSeparatorProperty, value);
    }

    private void OnIsHorizontalCollapsedChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            if (ItemsPanelRoot is OverflowStackPanel s) s.MoveChildrenToOverflowPanel();
        }
        else
        {
            if (ItemsPanelRoot is OverflowStackPanel s) s.MoveChildrenToMainPanel();
        }
    }

    private void OnLevelChange(AvaloniaPropertyChangedEventArgs<int> args) => PseudoClasses.Set(PC_FirstLevel, args.NewValue.Value == 1);

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) => NeedsContainer<NavigationMenuItem>(item, out recycleKey);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new NavigationMenuItem();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _rootMenu = GetRootMenu();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetCurrentValue(LevelProperty, CalculateDistanceFromLogicalParent<NavigationMenu>(this));
        _popup = e.NameScope.Find<Popup>("PART_Popup");
        _overflowPanel = e.NameScope.Find<Panel>("PART_OverflowPanel");
        if (_rootMenu is not null)
        {
            this.TryBind(IconProperty, _rootMenu.Icon);
            this.TryBind(HeaderProperty, _rootMenu.HeaderBinding);
            this.TryBind(ItemsSourceProperty, _rootMenu.SubMenu);
            this.TryBind(CommandProperty, _rootMenu.Command);
            this[!IconTemplateProperty] = _rootMenu[!NavigationMenu.IconTemplateProperty];
            this[!HeaderTemplateProperty] = _rootMenu[!NavigationMenu.HeaderTemplateProperty];
            this[!SubMenuIndentProperty] = _rootMenu[!NavigationMenu.SubMenuIndentProperty];
            this[!IsHorizontalCollapsedProperty] = _rootMenu[!NavigationMenu.IsHorizontalCollapsedProperty];
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var root = ItemsPanelRoot;
        if (root is OverflowStackPanel stack) stack.OverflowPanel = _overflowPanel;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (IsSeparator)
        {
            e.Handled = true;
            return;
        }

        base.OnPointerPressed(e);
        if (e.Handled) return;

        var p = e.GetCurrentPoint(this);
        if (p.Properties.PointerUpdateKind is not (PointerUpdateKind.LeftButtonPressed
            or PointerUpdateKind.RightButtonPressed)) return;
        if (p.Pointer.Type == PointerType.Mouse)
        {
            if (ItemCount == 0)
            {
                SelectItem(this);
                Command?.Execute(CommandParameter);
                e.Handled = true;
            }
            else
            {
                if (!IsHorizontalCollapsed)
                {
                    SetCurrentValue(IsVerticalCollapsedProperty, !IsVerticalCollapsed);
                    e.Handled = true;
                }
                else
                {
                    if (_popup is null || e.Source is not Visual v || _popup.IsInsidePopup(v)) return;
                    if (_popup.IsOpen)
                        _popup.Close();
                    else
                        _popup.Open();
                }
            }
        }
        else
        {
            _pointerDownPoint = p.Position;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!e.Handled && !double.IsNaN(_pointerDownPoint.X) &&
            e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            if (!new Rect(Bounds.Size).ContainsExclusive(point.Position) || e.Pointer.Type != PointerType.Touch) return;
            if (ItemCount == 0)
            {
                SelectItem(this);
                Command?.Execute(CommandParameter);
                e.Handled = true;
            }
            else
            {
                if (!IsHorizontalCollapsed)
                {
                    SetCurrentValue(IsVerticalCollapsedProperty, !IsVerticalCollapsed);
                    e.Handled = true;
                }
                else
                {
                    if (_popup is null || e.Source is not Visual v || _popup.IsInsidePopup(v)) return;
                    if (_popup.IsOpen)
                        _popup.Close();
                    else
                        _popup.Open();
                }
            }
        }
    }

    internal void SelectItem(NavigationMenuItem item)
    {
        if (item == this)
        {
            SetCurrentValue(IsSelectedProperty, true);
            SetCurrentValue(IsHighlightedProperty, true);
        }
        else
        {
            SetCurrentValue(IsSelectedProperty, false);
            SetCurrentValue(IsHighlightedProperty, true);
        }

        if (Parent is NavigationMenuItem menuItem)
        {
            menuItem.SelectItem(item);
            var items = menuItem.LogicalChildren.OfType<NavigationMenuItem>();
            foreach (var child in items)
                if (child != this)
                    child.ClearSelection();
        }
        else if (Parent is NavigationMenu menu)
        {
            menu.SelectItem(item, this);
        }

        _popup?.Close();
    }

    internal void ClearSelection()
    {
        SetCurrentValue(IsHighlightedProperty, false);
        SetCurrentValue(IsSelectedProperty, false);
        foreach (var child in LogicalChildren)
            if (child is NavigationMenuItem item)
                item.ClearSelection();
    }

    private NavigationMenu? GetRootMenu()
    {
        var root = this.FindAncestorOfType<NavigationMenu>() ?? this.FindLogicalAncestorOfType<NavigationMenu>();
        return root;
    }

    private static int CalculateDistanceFromLogicalParent<T>(ILogical? logical, int @default = -1) where T : class
    {
        var result = 0;

        while (logical != null && logical is not T)
        {
            if (logical is NavigationMenuItem) result++;
            logical = logical.LogicalParent;
        }

        return logical != null ? result : @default;
    }

    internal IEnumerable<NavigationMenuItem> GetLeafMenus()
    {
        if (ItemCount == 0)
        {
            yield return this;
            yield break;
        }

        foreach (var child in LogicalChildren)
            if (child is NavigationMenuItem item)
            {
                var items = item.GetLeafMenus();
                foreach (var i in items) yield return i;
            }
    }
}
