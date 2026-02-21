using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SidebarNav.ViewModels
{
    /// <summary>
    /// 徽章类型
    /// </summary>
    public enum BadgeType
    {
        None,
        /// <summary>数字徽章</summary>
        Number,
        /// <summary>红点</summary>
        Dot,
        /// <summary>文本徽章</summary>
        Text
    }

    /// <summary>
    /// 图标来源类型
    /// </summary>
    public enum IconType
    {
        None,
        /// <summary>Path Data 矢量图标</summary>
        PathData,
        /// <summary>Font Icon (字体图标字符)</summary>
        FontIcon,
        /// <summary>Image 路径 (URI)</summary>
        Image
    }

    /// <summary>
    /// 导航项 ViewModel —— 支持多级嵌套、搜索过滤、徽章、多种图标源
    /// </summary>
    public class SidebarItemViewModel : ObservableObject
    {
        #region 基本属性

        private string _id;
        /// <summary>唯一标识（用于导航路由和历史记录）</summary>
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _title;
        /// <summary>显示名称</summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _toolTip;
        /// <summary>工具提示（迷你模式下特别有用）</summary>
        public string ToolTip
        {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value);
        }

        private object _tag;
        /// <summary>附加数据（可用于绑定 Page 类型、路由参数等）</summary>
        public object Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private int _sortOrder;
        /// <summary>排序权重</summary>
        public int SortOrder
        {
            get => _sortOrder;
            set => SetProperty(ref _sortOrder, value);
        }

        #endregion

        #region 选中 / 展开 / 层级

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value) && value)
                {
                    // 选中时确保父级展开
                    var p = Parent;
                    while (p != null)
                    {
                        p.IsExpanded = true;
                        p = p.Parent;
                    }
                }
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private int _level;
        /// <summary>层级深度（0 = 顶层）</summary>
        public int Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        private SidebarItemViewModel _parent;
        /// <summary>父级引用</summary>
        public SidebarItemViewModel Parent
        {
            get => _parent;
            set => SetProperty(ref _parent, value);
        }

        /// <summary>是否有子项</summary>
        public bool HasChildren => Children != null && Children.Count > 0;

        #endregion

        #region 图标

        private IconType _iconType = IconType.None;
        public IconType IconType
        {
            get => _iconType;
            set => SetProperty(ref _iconType, value);
        }

        private Geometry _iconPathData;
        /// <summary>Path Data 图标</summary>
        public Geometry IconPathData
        {
            get => _iconPathData;
            set { SetProperty(ref _iconPathData, value); if (value != null) IconType = IconType.PathData; }
        }

        private string _iconPathDataString;
        /// <summary>Path Data 字符串（自动解析为 Geometry）</summary>
        public string IconPathDataString
        {
            get => _iconPathDataString;
            set
            {
                if (SetProperty(ref _iconPathDataString, value) && !string.IsNullOrEmpty(value))
                {
                    try
                    {
                        IconPathData = Geometry.Parse(value);
                    }
                    catch { }
                }
            }
        }

        private string _fontIconGlyph;
        /// <summary>字体图标 Unicode 字符，如 "\uE8A5"</summary>
        public string FontIconGlyph
        {
            get => _fontIconGlyph;
            set { SetProperty(ref _fontIconGlyph, value); if (value != null) IconType = IconType.FontIcon; }
        }

        private FontFamily _fontIconFamily;
        /// <summary>字体图标字体族</summary>
        public FontFamily FontIconFamily
        {
            get => _fontIconFamily;
            set => SetProperty(ref _fontIconFamily, value);
        }

        private string _iconImageSource;
        /// <summary>图片图标 URI</summary>
        public string IconImageSource
        {
            get => _iconImageSource;
            set { SetProperty(ref _iconImageSource, value); if (value != null) IconType = IconType.Image; }
        }

        private double _iconSize = 20;
        public double IconSize
        {
            get => _iconSize;
            set => SetProperty(ref _iconSize, value);
        }

        #endregion

        #region 徽章

        private BadgeType _badgeType = BadgeType.None;
        public BadgeType BadgeType
        {
            get => _badgeType;
            set => SetProperty(ref _badgeType, value);
        }

        private int _badgeCount;
        /// <summary>数字徽章值</summary>
        public int BadgeCount
        {
            get => _badgeCount;
            set
            {
                SetProperty(ref _badgeCount, value);
                OnPropertyChanged(nameof(BadgeCountDisplay));
            }
        }

        /// <summary>显示文本（>99 显示 99+）</summary>
        public string BadgeCountDisplay => BadgeCount > 99 ? "99+" : BadgeCount.ToString();

        private string _badgeText;
        /// <summary>文本徽章内容</summary>
        public string BadgeText
        {
            get => _badgeText;
            set => SetProperty(ref _badgeText, value);
        }

        private Brush _badgeBrush;
        /// <summary>徽章自定义颜色（null 时用主题默认色）</summary>
        public Brush BadgeBrush
        {
            get => _badgeBrush;
            set => SetProperty(ref _badgeBrush, value);
        }

        #endregion

        #region 搜索

        private bool _isVisibleInSearch = true;
        /// <summary>搜索过滤后是否可见</summary>
        public bool IsVisibleInSearch
        {
            get => _isVisibleInSearch;
            set => SetProperty(ref _isVisibleInSearch, value);
        }

        private bool _isSearchHighlighted;
        /// <summary>搜索关键字是否直接匹配本项</summary>
        public bool IsSearchHighlighted
        {
            get => _isSearchHighlighted;
            set => SetProperty(ref _isSearchHighlighted, value);
        }

        #endregion

        #region 子项

        private ObservableCollection<SidebarItemViewModel> _children;
        public ObservableCollection<SidebarItemViewModel> Children
        {
            get => _children;
            set
            {
                if (ReferenceEquals(_children, value))
                    return;

                if (_children != null)
                    _children.CollectionChanged -= OnChildrenCollectionChanged;

                if (SetProperty(ref _children, value))
                {
                    if (_children != null)
                    {
                        _children.CollectionChanged += OnChildrenCollectionChanged;
                        RebuildChildHierarchy();
                    }

                    OnPropertyChanged(nameof(HasChildren));
                }
            }
        }

        /// <summary>添加子项</summary>
        public void AddChild(SidebarItemViewModel child)
        {
            if (child == null) return;

            if (Children == null)
                Children = new ObservableCollection<SidebarItemViewModel>();

            Children.Add(child);
        }

        /// <summary>移除子项</summary>
        public bool RemoveChild(SidebarItemViewModel child)
        {
            if (Children == null) return false;

            return Children.Remove(child);
        }

        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                && e.NewItems != null)
            {
                foreach (SidebarItemViewModel child in e.NewItems)
                    AttachChild(child);
            }

            if ((e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                && e.OldItems != null)
            {
                foreach (SidebarItemViewModel child in e.OldItems)
                    DetachChild(child);
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RebuildChildHierarchy();
            }

            OnPropertyChanged(nameof(HasChildren));
        }

        private void AttachChild(SidebarItemViewModel child)
        {
            if (child == null) return;
            child.Parent = this;
            child.Level = Level + 1;
        }

        private void DetachChild(SidebarItemViewModel child)
        {
            if (child == null) return;
            child.Parent = null;
        }

        private void RebuildChildHierarchy()
        {
            if (Children == null) return;

            foreach (var child in Children)
                AttachChild(child);
        }

        #endregion

        #region 命令

        private ICommand _navigateCommand;
        /// <summary>导航命令（点击时执行）</summary>
        public ICommand NavigateCommand
        {
            get => _navigateCommand;
            set => SetProperty(ref _navigateCommand, value);
        }

        #endregion

        #region 辅助方法

        /// <summary>递归查找所有后代项</summary>
        public System.Collections.Generic.IEnumerable<SidebarItemViewModel> GetAllDescendants()
        {
            if (Children == null) yield break;
            foreach (var child in Children)
            {
                yield return child;
                foreach (var desc in child.GetAllDescendants())
                    yield return desc;
            }
        }

        /// <summary>按 Id 查找项（递归）</summary>
        public SidebarItemViewModel FindById(string id)
        {
            if (Id == id) return this;
            if (Children == null) return null;
            return Children.Select(c => c.FindById(id)).FirstOrDefault(r => r != null);
        }

        public override string ToString() => $"[{Id}] {Title}";

        #endregion
    }
}
