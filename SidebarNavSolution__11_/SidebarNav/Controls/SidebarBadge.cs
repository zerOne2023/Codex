using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SidebarNav.ViewModels;

namespace SidebarNav.Controls
{
    /// <summary>
    /// 徽章控件 —— 支持数字/红点/文本三种模式
    /// </summary>
    public class SidebarBadge : Control
    {
        static SidebarBadge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarBadge),
                new FrameworkPropertyMetadata(typeof(SidebarBadge)));
        }

        public static readonly DependencyProperty BadgeTypeProperty =
            DependencyProperty.Register(nameof(BadgeType), typeof(BadgeType),
                typeof(SidebarBadge), new PropertyMetadata(BadgeType.None));

        public BadgeType BadgeType
        {
            get => (BadgeType)GetValue(BadgeTypeProperty);
            set => SetValue(BadgeTypeProperty, value);
        }

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(nameof(Count), typeof(int),
                typeof(SidebarBadge), new PropertyMetadata(0, OnCountChanged));

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static readonly DependencyProperty CountDisplayProperty =
            DependencyProperty.Register(nameof(CountDisplay), typeof(string),
                typeof(SidebarBadge), new PropertyMetadata("0"));

        public string CountDisplay
        {
            get => (string)GetValue(CountDisplayProperty);
            set => SetValue(CountDisplayProperty, value);
        }

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SidebarBadge badge)
            {
                int v = (int)e.NewValue;
                badge.CountDisplay = v > 99 ? "99+" : v.ToString();
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(SidebarBadge), new PropertyMetadata(null));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty BadgeBrushProperty =
            DependencyProperty.Register(nameof(BadgeBrush), typeof(Brush),
                typeof(SidebarBadge), new PropertyMetadata(null));

        /// <summary>自定义徽章颜色（null 时用主题默认色）</summary>
        public Brush BadgeBrush
        {
            get => (Brush)GetValue(BadgeBrushProperty);
            set => SetValue(BadgeBrushProperty, value);
        }
    }
}
