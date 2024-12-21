// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.UI.Dialogs.Models;

namespace MyNet.Avalonia.Controls;

public class CustomDialogControl : DialogControlBase
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var closeButtonVisible = IsCloseButtonVisible ?? DataContext is IDialogViewModel;
        IsHitTestVisibleProperty.SetValue(closeButtonVisible, _closeButton);
        if (!closeButtonVisible)
        {
            OpacityProperty.SetValue(0, _closeButton);
        }
    }

    public override void Close()
    {
        if (DataContext is IDialogViewModel context)
            context.Close();
        else
            OnElementClosing(this, null);
    }
}
