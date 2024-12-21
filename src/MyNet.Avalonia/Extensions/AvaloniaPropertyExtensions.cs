// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Extensions
{
    public static class AvaloniaPropertyExtensions
    {
        public static void SetValue<T>(this AvaloniaProperty<T> property, T value, params AvaloniaObject?[] objects)
        {
            for (var i = 0; i < objects.Length; i++)
            {
                objects[i]?.SetValue(property, value);
            }
        }

        public static void SetValue<T, TControl>(this AvaloniaProperty<T> property, T value, IEnumerable<TControl?> objects) where TControl : AvaloniaObject
        {
            foreach (var @object in objects)
            {
                @object?.SetValue(property, value);
            }
        }

        public static void AffectsPseudoClass<TControl>(this AvaloniaProperty<bool> property, string pseudoClass, RoutedEvent<RoutedEventArgs>? routedEvent = null) where TControl : Control
        {
            var pseudoClass2 = pseudoClass;
            var routedEvent2 = routedEvent;
            property.Changed.AddClassHandler(delegate (TControl control, AvaloniaPropertyChangedEventArgs<bool> args)
            {
                OnPropertyChanged(control, args, pseudoClass2, routedEvent2);
            });
        }

        public static void AffectsPseudoClass<TControl, TArgs>(this AvaloniaProperty<bool> property, string pseudoClass, RoutedEvent<TArgs>? routedEvent = null) where TControl : Control where TArgs : RoutedEventArgs, new()
        {
            var pseudoClass2 = pseudoClass;
            var routedEvent2 = routedEvent;
            property.Changed.AddClassHandler(delegate (TControl control, AvaloniaPropertyChangedEventArgs<bool> args)
            {
                OnPropertyChanged(control, args, pseudoClass2, routedEvent2);
            });
        }

        private static void OnPropertyChanged<TControl, TArgs>(TControl control, AvaloniaPropertyChangedEventArgs<bool> args, string pseudoClass, RoutedEvent<TArgs>? routedEvent) where TControl : Control where TArgs : RoutedEventArgs, new()
        {
            PseudolassesExtensions.Set(control.Classes, pseudoClass, args.NewValue.Value);
            if (routedEvent != null)
            {
                control.RaiseEvent(new TArgs
                {
                    RoutedEvent = routedEvent
                });
            }
        }
    }
}
