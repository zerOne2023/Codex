using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SidebarNav.ViewModels;

namespace SidebarNav.Controls
{
    /// <summary>
    /// Sidebar 主容器控件 —— 包含 Header / 搜索 / 分组列表 / Footer
    /// 支持展开/迷你模式动画切换、键盘导航
    /// </summary>
    [TemplatePart(Name = PART_ScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_ItemsHost, Type = typeof(ItemsControl))]
    [TemplatePart(Name = PART_RootBorder, Type = typeof(Border))]
    public class SidebarNavigation : Control
    {
        private const string PART_ScrollViewer = "PART_ScrollViewer";
        private const string PART_ItemsHost = "PART_ItemsHost";
        private const string PART_RootBorder = "PART_RootBorder";

        static SidebarNavigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarNavigation),
                new FrameworkPropertyMetadata(typeof(SidebarNavigation)));
        }

        #region ViewModel 属性

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SidebarViewModel),
                typeof(SidebarNavigation), new PropertyMetadata(null, OnViewModelChanged));

        public SidebarViewModel ViewModel
        {
            get => (SidebarViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SidebarNavigation navigation)
            {
                if (e.OldValue is SidebarViewModel oldViewModel)
                    oldViewModel.SelectedItemChanged -= navigation.OnSelectedItemChanged;

                if (e.NewValue is SidebarViewModel newViewModel)
                    newViewModel.SelectedItemChanged += navigation.OnSelectedItemChanged;
            }

            // ViewModel 通过 DependencyProperty 存储，模板中通过
            // RelativeSource TemplatedParent 访问，不需要覆盖 DataContext。
            // 覆盖 DataContext 会破坏外部的 ViewModel="{Binding Sidebar}" 绑定。
        }

        private void OnSelectedItemChanged(object sender, SidebarItemViewModel selectedItem)
        {
            RaiseEvent(new SidebarItemSelectedEventArgs(ItemSelectedEvent, this, selectedItem));
        }

        #endregion

        #region Header / Footer 插槽

        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(object),
                typeof(SidebarNavigation), new PropertyMetadata(null));

        /// <summary>顶部 Header 区域（Logo、品牌等）</summary>
        public object HeaderContent
        {
            get => GetValue(HeaderContentProperty);
            set => SetValue(HeaderContentProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate),
                typeof(SidebarNavigation), new PropertyMetadata(null));

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty FooterContentProperty =
            DependencyProperty.Register(nameof(FooterContent), typeof(object),
                typeof(SidebarNavigation), new PropertyMetadata(null));

        /// <summary>底部 Footer 区域（用户信息、设置等）</summary>
        public object FooterContent
        {
            get => GetValue(FooterContentProperty);
            set => SetValue(FooterContentProperty, value);
        }

        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register(nameof(FooterTemplate), typeof(DataTemplate),
                typeof(SidebarNavigation), new PropertyMetadata(null));

        public DataTemplate FooterTemplate
        {
            get => (DataTemplate)GetValue(FooterTemplateProperty);
            set => SetValue(FooterTemplateProperty, value);
        }

        #endregion

        #region 尺寸 & 模式

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool),
                typeof(SidebarNavigation), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsExpandedChanged));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static readonly DependencyProperty ExpandedWidthProperty =
            DependencyProperty.Register(nameof(ExpandedWidth), typeof(double),
                typeof(SidebarNavigation), new PropertyMetadata(240.0));

        public double ExpandedWidth
        {
            get => (double)GetValue(ExpandedWidthProperty);
            set => SetValue(ExpandedWidthProperty, value);
        }

        public static readonly DependencyProperty MiniWidthProperty =
            DependencyProperty.Register(nameof(MiniWidth), typeof(double),
                typeof(SidebarNavigation), new PropertyMetadata(60.0));

        public double MiniWidth
        {
            get => (double)GetValue(MiniWidthProperty);
            set => SetValue(MiniWidthProperty, value);
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(Duration),
                typeof(SidebarNavigation), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(250))));

        public Duration AnimationDuration
        {
            get => (Duration)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        public static readonly DependencyProperty ShowSearchBoxProperty =
            DependencyProperty.Register(nameof(ShowSearchBox), typeof(bool),
                typeof(SidebarNavigation), new PropertyMetadata(true));

        public bool ShowSearchBox
        {
            get => (bool)GetValue(ShowSearchBoxProperty);
            set => SetValue(ShowSearchBoxProperty, value);
        }

        public static readonly DependencyProperty ShowBackButtonProperty =
            DependencyProperty.Register(nameof(ShowBackButton), typeof(bool),
                typeof(SidebarNavigation), new PropertyMetadata(true));

        public bool ShowBackButton
        {
            get => (bool)GetValue(ShowBackButtonProperty);
            set => SetValue(ShowBackButtonProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
                typeof(SidebarNavigation), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region 事件

        public static readonly RoutedEvent ItemSelectedEvent =
            EventManager.RegisterRoutedEvent(nameof(ItemSelected), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SidebarNavigation));

        public event RoutedEventHandler ItemSelected
        {
            add { AddHandler(ItemSelectedEvent, value); }
            remove { RemoveHandler(ItemSelectedEvent, value); }
        }

        public static readonly RoutedEvent ModeChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ModeChanged), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SidebarNavigation));

        public event RoutedEventHandler ModeChanged
        {
            add { AddHandler(ModeChangedEvent, value); }
            remove { RemoveHandler(ModeChangedEvent, value); }
        }

        #endregion

        #region 展开动画

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SidebarNavigation nav)
            {
                nav.AnimateWidth((bool)e.NewValue);
                nav.RaiseEvent(new RoutedEventArgs(ModeChangedEvent));

                if (nav.ViewModel != null)
                    nav.ViewModel.IsExpanded = (bool)e.NewValue;
            }
        }

        private void AnimateWidth(bool expand)
        {
            var target = expand ? ExpandedWidth : MiniWidth;
            var from = expand ? MiniWidth : ExpandedWidth;

            var animation = new DoubleAnimation
            {
                From = from,
                To = target,
                Duration = AnimationDuration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            this.BeginAnimation(WidthProperty, animation);
        }

        #endregion

        #region 键盘导航

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (ViewModel == null) return;

            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                case Key.Left:
                case Key.Right:
                case Key.Enter:
                case Key.Space:
                    ViewModel.HandleKeyNavigation(e.Key);
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Focusable = true;
        }

    }

    public class SidebarItemSelectedEventArgs : RoutedEventArgs
    {
        public SidebarItemSelectedEventArgs(RoutedEvent routedEvent, object source, SidebarItemViewModel selectedItem)
            : base(routedEvent, source)
        {
            SelectedItem = selectedItem;
        }

        public SidebarItemViewModel SelectedItem { get; }
    }
}
