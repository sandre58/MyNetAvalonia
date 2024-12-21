// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Input;

namespace MyNet.Avalonia.Parameters;

public class FocusAssist
{
    protected FocusAssist() { }

    #region DialogFocusHint

    /// <summary>
    /// Provides DialogFocusHint Property for attached FocusAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> DialogFocusHintProperty =
        AvaloniaProperty.RegisterAttached<FocusAssist, InputElement, bool>("DialogFocusHint");

    /// <summary>
    /// Accessor for Attached  <see cref="DialogFocusHintProperty"/>.
    /// </summary>
    /// <param name="element">Target element</param>
    /// <param name="value">The value to set  <see cref="DialogFocusHintProperty"/>.</param>
    public static void SetDialogFocusHint(InputElement element, bool value) => element.SetValue(DialogFocusHintProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DialogFocusHintProperty"/>.
    /// </summary>
    /// <param name="element">Target element</param>
    public static bool GetDialogFocusHint(InputElement element) => element.GetValue(DialogFocusHintProperty);

    #endregion
}
