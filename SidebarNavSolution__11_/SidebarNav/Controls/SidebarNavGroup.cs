using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SidebarNav.Controls
{
    /// <summary>
    /// 分组标题控件 —— 带分隔线和可折叠分组
    /// </summary>
    public class SidebarNavGroup : HeaderedItemsControl
    {
        /// <summary>切换折叠状态的路由命令</summary>
        public static readonly RoutedUICommand ToggleCollapseCommand =
            new RoutedUICommand("Toggle Collapse", "ToggleCollapse", typeof(SidebarNavGroup));

        static SidebarNavGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarNavGroup),
                new FrameworkPropertyMetadata(typeof(SidebarNavGroup)));

            CommandManager.RegisterClassCommandBinding(typeof(SidebarNavGroup),
                new CommandBinding(ToggleCollapseCommand, OnToggleCollapse));
        }

        private static void OnToggleCollapse(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is SidebarNavGroup group)
            {
                group.IsCollapsed = !group.IsCollapsed;
            }
        }

        public static readonly DependencyProperty ShowSeparatorProperty =
            DependencyProperty.Register(nameof(ShowSeparator), typeof(bool),
                typeof(SidebarNavGroup), new PropertyMetadata(true));

        public bool ShowSeparator
        {
            get => (bool)GetValue(ShowSeparatorProperty);
            set => SetValue(ShowSeparatorProperty, value);
        }

        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(nameof(IsCollapsed), typeof(bool),
                typeof(SidebarNavGroup), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        public static readonly DependencyProperty ShowGroupNameProperty =
            DependencyProperty.Register(nameof(ShowGroupName), typeof(bool),
                typeof(SidebarNavGroup), new PropertyMetadata(true));

        /// <summary>是否显示分组名称（迷你模式可隐藏）</summary>
        public bool ShowGroupName
        {
            get => (bool)GetValue(ShowGroupNameProperty);
            set => SetValue(ShowGroupNameProperty, value);
        }
    }
}
