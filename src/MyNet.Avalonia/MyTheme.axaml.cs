// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theming;
using MyNet.Utilities;
using AvaloniaStyles = Avalonia.Styling.Styles;

namespace MyNet.Avalonia;

public class MyTheme : AvaloniaStyles, IResourceNode
{
    private static MyTheme _theme = null!;
    public static MyTheme Current => _theme ??= Application.Current?.Styles.OfType<MyTheme>().FirstOrDefault() ?? throw new InvalidOperationException("Cannot locate Theme in Avalonia application styles. Be sure that you include Theme in your App.xaml in Application.Styles section");

    public static readonly string ResourcePrefix = "MyNet";
    public static readonly string ColorPattern = $"{ResourcePrefix}.Color.{"{0}"}";
    public static readonly string BrushPattern = $"{ResourcePrefix}.Brush.{"{0}"}";

    private static readonly Color DefaultPrimaryColor = Color.Parse("#2196F3");
    private static readonly Color DefaultAccentColor = Color.Parse("#FFC107");

    public static readonly StyledProperty<Color> PrimaryColorProperty = AvaloniaProperty.Register<MyTheme, Color>(nameof(PrimaryColor), DefaultPrimaryColor);
    public static readonly StyledProperty<Color?> PrimaryForegroundColorProperty = AvaloniaProperty.Register<MyTheme, Color?>(nameof(PrimaryForegroundColor));

    public static readonly StyledProperty<Color> AccentColorProperty = AvaloniaProperty.Register<MyTheme, Color>(nameof(AccentColor), DefaultAccentColor);
    public static readonly StyledProperty<Color?> AccentForegroundColorProperty = AvaloniaProperty.Register<MyTheme, Color?>(nameof(AccentForegroundColor));

    private readonly IServiceProvider? _serviceProvider;
    private IDisposable? _themeUpdateDisposable;
    private Task? _currentThemeUpdateTask;
    private bool _isResourcedAccessed;
    private CancellationTokenSource? _themeUpdateCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTheme"/> class.
    /// </summary>
    /// <param name="serviceProvider">The parent's service provider.</param>
    public MyTheme(IServiceProvider? serviceProvider) => _serviceProvider = serviceProvider;

    public Color PrimaryColor
    {
        get => GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    public Color? PrimaryForegroundColor
    {
        get => GetValue(PrimaryForegroundColorProperty);
        set => SetValue(PrimaryForegroundColorProperty, value);
    }

    public Color AccentColor
    {
        get => GetValue(AccentColorProperty);
        set => SetValue(AccentColorProperty, value);
    }

    public Color? AccentForegroundColor
    {
        get => GetValue(AccentForegroundColorProperty);
        set => SetValue(AccentForegroundColorProperty, value);
    }

    bool IResourceNode.TryGetResource(object key, ThemeVariant? theme, out object? value) => TryGetResource(key, theme, out value);

    protected new virtual bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        if (!_isResourcedAccessed)
        {
            _isResourcedAccessed = true;
            OnResourcedAccessed();
        }

        return base.TryGetResource(key, theme, out value) || Resources.TryGetResource(key, theme, out value);
    }

    /// <summary>
    /// This event is raised when all brushes is changed.
    /// </summary>
    public event EventHandler? ThemeChanged;

    private void OnResourcedAccessed()
    {
        AvaloniaXamlLoader.Load(_serviceProvider, this);

        EnqueueThemeUpdate();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PrimaryColorProperty || change.Property == PrimaryForegroundColorProperty
            || change.Property == AccentColorProperty || change.Property == AccentForegroundColorProperty)
            EnqueueThemeUpdate();
    }

    private void UpdatePrimaryColors() => StartUpdatingColor("Primary", new(PrimaryColor, PrimaryForegroundColor));

    private void UpdateAccentColors() => StartUpdatingColor("Accent", new(AccentColor, AccentForegroundColor));

    private void UpdateTheme()
    {
        UpdatePrimaryColors();
        UpdateAccentColors();
    }

    private void StartUpdatingColor(string colorName, ColorPair color)
        => Task.Run(async () =>
        {
            _themeUpdateCancellationTokenSource?.Cancel();
            _themeUpdateCancellationTokenSource?.Dispose();

            var currentToken = new CancellationTokenSource();
            _themeUpdateCancellationTokenSource = currentToken;

            if (_currentThemeUpdateTask != null) await _currentThemeUpdateTask;
            if (!currentToken.IsCancellationRequested)
            {
                // If control is not attached to visual tree (is doesn't have Parent)
                // And we inside a dispatcher thread (since it required for SolidColorBrush creation/changing)
                // We can just invoke all color changes RIGHT NOW ON CURRENT THREAD
                // -------------------------------------------------------------------
                // If we already attached to something (e.g. theme was changed while app is running)
                // We enqueue everything to dispatcher thread
                // Cuz if we execute everything RIGHT NOW on dispatcher thread it will cause lag spike
                // So we changing colors one by one
                Func<Action, DispatcherPriority, Task> contextSync = Owner is null && Dispatcher.UIThread.CheckAccess()
                    ? (x, _) => InvokeImmediately(x)
                    : (action, priority) => Dispatcher.UIThread.InvokeAsync(action, priority).GetTask();
                var task = AddOrUpdateColors(colorName, color, contextSync);

                _currentThemeUpdateTask = task;

                await task.ContinueWith(delegate
                {
                    ThemeChanged?.Invoke(this, EventArgs.Empty);
                }, CancellationToken.None);
            }
        });

    private Task AddOrUpdateColors(string colorName, ColorPair color, Func<Action, DispatcherPriority, Task> contextSync)
    {
        var light = new ColorPair(color.Color.Lighten(), color.ForegroundColor);
        var dark = new ColorPair(color.Color.Darken(), color.ForegroundColor);

        var dictionary = new Dictionary<string, Color>
        {
            { colorName, color.Color },
            { $"{colorName}.Foreground", color.GetForegroundColor() },

            { $"{colorName}.Light", light.GetForegroundColor() },
            { $"{colorName}.Light.Foreground", light.GetForegroundColor() },

            { $"{colorName}.Dark", dark.Color },
            { $"{colorName}.Dark.Foreground", dark.GetForegroundColor() }
        };
        return Task.WhenAll(dictionary.Select(x => AddOrUpdateColor(x.Key, x.Value, contextSync)));
    }

    private Task AddOrUpdateColor(string name, Color newColor, Func<Action, DispatcherPriority, Task> contextSync)
        => contextSync(() =>
        {
            var resourceDictionary = Resources;
            resourceDictionary.AddOrUpdate(ColorPattern.FormatWith(name), newColor);
            resourceDictionary.AddOrUpdate(BrushPattern.FormatWith(name), new SolidColorBrush(newColor) { Transitions = [new ColorTransition { Duration = TimeSpan.FromSeconds(0.35), Easing = new SineEaseOut(), Property = SolidColorBrush.ColorProperty }] });
        }, DispatcherPriority.Normal);

    private static Task InvokeImmediately(Action action)
    {
        action();
        return Task.CompletedTask;
    }

    private void EnqueueThemeUpdate()
    {
        _themeUpdateDisposable?.Dispose();
        _themeUpdateDisposable = DispatcherTimer.RunOnce(UpdateTheme, TimeSpan.FromMilliseconds(100));
    }
}
