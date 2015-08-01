using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

using CrossPlatformLibrary.Extensions;

using Microsoft.Phone.Maps.Toolkit;

namespace CrossPlatformLibrary.Maps.Controls // TODO GATH: Rename namespace! Dont use .net framework namespacesy
{
    [ContentProperty("Content")]
    public sealed class Pushpin : MapChildControl
    {
        public static readonly DependencyProperty MenuContentProperty = DependencyProperty.Register("MenuContent", typeof(object), typeof(Pushpin), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty IsMenuVisibleProperty = DependencyProperty.Register("IsMenuVisible", typeof(bool), typeof(Pushpin), new PropertyMetadata(OnMenuVisibilityChanged));

        ////public static readonly DependencyProperty MenuMarginProperty = DependencyProperty.RegisterAttached(
        ////    "MenuMargin",
        ////    typeof(Thickness),
        ////    typeof(Pushpin),
        ////    new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty PushpinBrushProperty = DependencyProperty.Register(
            "PushpinBrush",
            typeof(SolidColorBrush),
            typeof(Pushpin),
            new PropertyMetadata(default(SolidColorBrush)));

        private static void OnMenuVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pushpin = (Pushpin)d;
            pushpin.Dispatcher.BeginInvoke(() => ToggleMenu(pushpin, pushpin.MenuContent as FrameworkElement, (bool)e.NewValue));
        }

        public Pushpin()
        {
            this.DefaultStyleKey = typeof(Pushpin);
        }

        public override void OnApplyTemplate()
        {
            ////Dispatcher.BeginInvoke(() => ToggleMenu(this, this.MenuContent as FrameworkElement, this.IsMenuVisible)); // TODO Dispatcher really needed? Delay ok?
            base.OnApplyTemplate();
        }

        private static void ToggleMenu(Pushpin pushpin, FrameworkElement content, bool makeMenuVisible)
        {
            if (content != null)
            {
                if (makeMenuVisible)
                {
                    content.Visibility = Visibility.Visible;

                    // UpdateLayout is necessary to render the ActualWidth and ActualHeight values.
                    content.UpdateLayout(); // TODO GATH: Check if this is obsolete since we're now calling using Dispatcher

                    // To avoid that the pushpin is randomly moved horizontally for +/- 1px, we check if the actual size is odd/even.
                    // We only accept even sizes because we divide the values later by 2 (horizontally centered).
                    double evenWidth = Convert.ToInt32(content.ActualWidth).IsOdd() ? content.ActualWidth + 1 : content.ActualWidth;
                    double evenHeight = Convert.ToInt32(content.ActualHeight).IsOdd() ? content.ActualHeight + 1 : content.ActualHeight;

                    // Set pushpin margin to center pin w/ opened menu
                    if (Math.Abs(evenWidth) > 0.0001 && Math.Abs(evenHeight) > 0.0001)
                    {
                        pushpin.Margin = pushpin.MenuMargin = new Thickness(((evenWidth / 2) * -1) + 16 - 2, ((evenHeight + 78 + 12 + 4) * -1) + 7 + 2, 0, 0);
                    }

                    pushpin.Margin = pushpin.MenuMargin;
                }
                else
                {
                    // Hide pushpin content and reset margin (=position if content menu is opened)
                    content.Visibility = Visibility.Collapsed;
                    pushpin.Margin = new Thickness(0, 0, 0, 0);
                }
            }
        }

        public bool IsMenuVisible
        {
            get
            {
                return (bool)this.GetValue(IsMenuVisibleProperty);
            }
            set
            {
                this.SetValue(IsMenuVisibleProperty, value);
            }
        }

        public static bool GetIsMenuVisible(UIElement element)
        {
            return (bool)element.GetValue(IsMenuVisibleProperty);
        }

        public static void SetIsMenuVisible(UIElement element, bool value)
        {
            element.SetValue(IsMenuVisibleProperty, value);
        }

        public Thickness MenuMargin { get; set; }

        ////public static Thickness GetMenuMargin(UIElement element)
        ////{
        ////    return (Thickness)element.GetValue(MenuMarginProperty);
        ////}

        ////public static void SetMenuMargin(UIElement element, Thickness value)
        ////{
        ////    element.SetValue(MenuMarginProperty, value);
        ////}

        public object MenuContent
        {
            get
            {
                return this.GetValue(MenuContentProperty);
            }
            set
            {
                this.SetValue(MenuContentProperty, value);
            }
        }

        public SolidColorBrush PushpinBrush
        {
            get
            {
                return (SolidColorBrush)this.GetValue(PushpinBrushProperty);
            }
            set
            {
                this.SetValue(PushpinBrushProperty, value);
            }
        }

        public static object GetMenuContent(UIElement element)
        {
            return (object)element.GetValue(MenuContentProperty);
        }

        public static void SetMenuContent(UIElement element, object value)
        {
            element.SetValue(MenuContentProperty, value);
        }
    }
}