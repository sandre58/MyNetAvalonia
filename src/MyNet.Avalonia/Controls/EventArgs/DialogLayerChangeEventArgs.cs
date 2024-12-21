// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.EventArgs;

public class DialogLayerChangeEventArgs : RoutedEventArgs
{
    public DialogLayerChangeType ChangeType { get; }

    public DialogLayerChangeEventArgs(DialogLayerChangeType type) => ChangeType = type;

    public DialogLayerChangeEventArgs(RoutedEvent routedEvent, DialogLayerChangeType type) : base(routedEvent) => ChangeType = type;
}
