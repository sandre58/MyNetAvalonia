// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia.Interactivity;

namespace MyNet.Avalonia.Controls.EventArgs;

public class ResultEventArgs : RoutedEventArgs
{
    public object? Result { get; set; }

    public ResultEventArgs(object? result) => Result = result;

    public ResultEventArgs(RoutedEvent routedEvent, object? result) : base(routedEvent) => Result = result;
}
