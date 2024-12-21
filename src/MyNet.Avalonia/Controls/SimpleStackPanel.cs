// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls
{
    /// <summary>
    /// Arranges child elements into a single line that can be oriented horizontally
    /// or vertically.
    /// </summary>
    public class SimpleStackPanel : Panel
    {
        protected override Type StyleKeyOverride => typeof(SimpleStackPanel);

        #region Orientation

        public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<SimpleStackPanel, Orientation>(nameof(Orientation), Orientation.Vertical);

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        #endregion

        #region Spacing

        /// <summary>
        /// Provides Spacing Property.
        /// </summary>
        public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<SimpleStackPanel, double>(nameof(Spacing), (double)ThicknessSize.Medium);

        /// <summary>
        /// Gets or sets the Spacing property.
        /// </summary>
        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        #endregion

        /// <summary>
        /// Measures the child elements of a SimpleStackPanel in anticipation
        /// of arranging them during the SimpleStackPanel.ArrangeOverride(System.Windows.Size)
        /// pass.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns>The System.Windows.Size that represents the desired size of the element.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var stackDesiredSize = new Size();
            var children = Children;
            var layoutSlotSize = availableSize;
            var fHorizontal = Orientation == Orientation.Horizontal;
            var spacing = Spacing;
            var hasVisibleChild = false;

            layoutSlotSize = fHorizontal ? layoutSlotSize.WithWidth(double.PositiveInfinity) : layoutSlotSize.WithHeight(double.PositiveInfinity);

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null) continue;
                var isVisible = child.IsVisible;

                if (isVisible && !hasVisibleChild)
                    hasVisibleChild = true;

                child.Measure(layoutSlotSize);
                var childDesiredSize = child.DesiredSize;

                stackDesiredSize = fHorizontal
                    ? new Size(stackDesiredSize.Width + (isVisible ? spacing : 0) + childDesiredSize.Width, Math.Max(stackDesiredSize.Height, childDesiredSize.Height))
                    : new Size(Math.Max(stackDesiredSize.Width, childDesiredSize.Width), (isVisible ? spacing : 0) + childDesiredSize.Height);
            }

            stackDesiredSize = fHorizontal
                ? stackDesiredSize.WithWidth(stackDesiredSize.Width - (hasVisibleChild ? spacing : 0))
                : stackDesiredSize.WithHeight(stackDesiredSize.Height - (hasVisibleChild ? spacing : 0));

            return stackDesiredSize;
        }

        /// <summary>
        /// Arranges the content of a SimpleStackPanel element.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns>
        /// The System.Windows.Size that represents the arranged size of this SimpleStackPanel
        /// element and its child elements.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = Children;
            var fHorizontal = Orientation == Orientation.Horizontal;
            var rcChild = new Rect(finalSize);
            var previousChildSize = 0.0;
            var spacing = Spacing;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null) continue;
                if (fHorizontal)
                {
                    rcChild = rcChild.WithX(rcChild.X + previousChildSize);
                    previousChildSize = child.DesiredSize.Width;
                    rcChild = rcChild.WithWidth(previousChildSize);
                    rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                }
                else
                {
                    rcChild = rcChild.WithY(rcChild.Y + previousChildSize);
                    previousChildSize = child.DesiredSize.Height;
                    rcChild = rcChild.WithHeight(previousChildSize);
                    rcChild = rcChild.WithWidth(Math.Max(finalSize.Width, child.DesiredSize.Width));
                }

                if (child.IsVisible)
                    previousChildSize += spacing;

                child.Arrange(rcChild);
            }
            return finalSize;
        }
    }
}
