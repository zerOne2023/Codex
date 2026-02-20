using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SidebarNav.ViewModels;

namespace SidebarNav.Controls
{
    /// <summary>
    /// 导航项控件 —— 支持多级嵌套展开、图标、徽章、选中高亮
    /// </summary>
    [TemplatePart(Name = PART_ExpandToggle, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PART_ChildrenHost, Type = typeof(ItemsControl))]
    public class SidebarNavItem : ContentControl
    {
        private const string PART_ExpandToggle = "PART_ExpandToggle";
        private const string PART_ChildrenHost = "PART_ChildrenHost";

        static SidebarNavItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarNavItem),
                new FrameworkPropertyMetadata(typeof(SidebarNavItem)));
        }

        #region 依赖属性

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool),
                typeof(SidebarNavItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool),
                typeof(SidebarNavItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register(nameof(Level), typeof(int),
                typeof(SidebarNavItem), new PropertyMetadata(0));

        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public static readonly DependencyProperty HasChildrenProperty =
            DependencyProperty.Register(nameof(HasChildren), typeof(bool),
                typeof(SidebarNavItem), new PropertyMetadata(false));

        public bool HasChildren
        {
            get => (bool)GetValue(HasChildrenProperty);
            set => SetValue(HasChildrenProperty, value);
        }

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand),
                typeof(SidebarNavItem), new PropertyMetadata(null));

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        public static readonly DependencyProperty ShowExpandedContentProperty =
            DependencyProperty.Register(nameof(ShowExpandedContent), typeof(bool),
                typeof(SidebarNavItem), new PropertyMetadata(true));

        /// <summary>是否显示文本（迷你模式时隐藏）</summary>
        public bool ShowExpandedContent
        {
            get => (bool)GetValue(ShowExpandedContentProperty);
            set => SetValue(ShowExpandedContentProperty, value);
        }

        public static readonly DependencyProperty IndicatorBrushProperty =
            DependencyProperty.Register(nameof(IndicatorBrush), typeof(Brush),
                typeof(SidebarNavItem), new PropertyMetadata(null));

        /// <summary>左侧选中指示条颜色（null 时用主题色）</summary>
        public Brush IndicatorBrush
        {
            get => (Brush)GetValue(IndicatorBrushProperty);
            set => SetValue(IndicatorBrushProperty, value);
        }

        #endregion

        #region 鼠标交互

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (DataContext is SidebarItemViewModel vm)
            {
                // 查找最近的 SidebarNavigation 获取 ViewModel
                var sidebar = FindParent<SidebarNavigation>(this);
                if (sidebar?.ViewModel != null)
                {
                    sidebar.ViewModel.SelectItemCommand.Execute(vm);
                }
            }

            e.Handled = true;
        }

        #endregion

        #region 辅助

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T t) return t;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        #endregion
    }
}
