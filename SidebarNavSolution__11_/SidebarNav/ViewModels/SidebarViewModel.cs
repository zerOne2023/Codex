using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SidebarNav.Services;

namespace SidebarNav.ViewModels
{
    /// <summary>
    /// Sidebar 主 ViewModel —— 管理分组、搜索、导航历史、模式切换
    /// </summary>
    public class SidebarViewModel : ObservableObject
    {
        #region 分组 & 项

        private ObservableCollection<SidebarGroupViewModel> _groups;
        /// <summary>带分组标题的导航项集合</summary>
        public ObservableCollection<SidebarGroupViewModel> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        private SidebarItemViewModel _selectedItem;
        /// <summary>当前选中的导航项</summary>
        public SidebarItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                var previous = _selectedItem;
                if (SetProperty(ref _selectedItem, value))
                {
                    // 更新选中状态
                    if (previous != null) previous.IsSelected = false;
                    if (value != null) value.IsSelected = true;

                    // 推入历史栈
                    if (previous != null && value != null && previous != value)
                        _historyService.Push(previous);

                    GoBackCommand?.RaiseCanExecuteChanged();
                    SelectedItemChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>选中项变更事件</summary>
        public event EventHandler<SidebarItemViewModel> SelectedItemChanged;

        #endregion

        #region 展开 / 迷你模式

        private bool _isExpanded = true;
        /// <summary>是否展开模式（false = 迷你模式）</summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value, onChanged: () => OnPropertyChanged(nameof(CurrentWidth)));
        }

        private double _expandedWidth = 240;
        /// <summary>展开时宽度</summary>
        public double ExpandedWidth
        {
            get => _expandedWidth;
            set => SetProperty(ref _expandedWidth, value, onChanged: () => OnPropertyChanged(nameof(CurrentWidth)));
        }

        private double _miniWidth = 60;
        /// <summary>迷你模式宽度</summary>
        public double MiniWidth
        {
            get => _miniWidth;
            set => SetProperty(ref _miniWidth, value, onChanged: () => OnPropertyChanged(nameof(CurrentWidth)));
        }

        /// <summary>当前宽度</summary>
        public double CurrentWidth => IsExpanded ? ExpandedWidth : MiniWidth;

        #endregion

        #region 搜索

        private string _searchText;
        /// <summary>搜索关键字</summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplySearch(value);
                }
            }
        }

        private bool _isSearchActive;
        public bool IsSearchActive
        {
            get => _isSearchActive;
            set => SetProperty(ref _isSearchActive, value);
        }

        private string _searchPlaceholder = "Search…";
        public string SearchPlaceholder
        {
            get => _searchPlaceholder;
            set => SetProperty(ref _searchPlaceholder, value);
        }

        #endregion

        #region 导航历史

        private readonly NavigationHistoryService _historyService = new NavigationHistoryService();

        public bool CanGoBack => _historyService.CanGoBack;

        #endregion

        #region 主题

        private string _currentTheme = "Light";
        public string CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (SetProperty(ref _currentTheme, value))
                {
                    ThemeManager.ApplyTheme(value);
                }
            }
        }

        #endregion

        #region 命令

        private RelayCommand _toggleExpandCommand;
        public RelayCommand ToggleExpandCommand =>
            _toggleExpandCommand ?? (_toggleExpandCommand = new RelayCommand(() =>
            {
                IsExpanded = !IsExpanded;
            }));

        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand =>
            _goBackCommand ?? (_goBackCommand = new RelayCommand(
                () =>
                {
                    var prev = _historyService.Pop();
                    if (prev != null)
                    {
                        // 直接设置不推入历史
                        var old = _selectedItem;
                        if (old != null) old.IsSelected = false;
                        _selectedItem = prev;
                        prev.IsSelected = true;
                        OnPropertyChanged(nameof(SelectedItem));
                        _goBackCommand.RaiseCanExecuteChanged();
                        SelectedItemChanged?.Invoke(this, prev);
                    }
                },
                () => _historyService.CanGoBack));

        private RelayCommand<SidebarItemViewModel> _selectItemCommand;
        public RelayCommand<SidebarItemViewModel> SelectItemCommand =>
            _selectItemCommand ?? (_selectItemCommand = new RelayCommand<SidebarItemViewModel>(
                item =>
                {
                    if (item == null) return;
                    if (item.HasChildren)
                    {
                        item.IsExpanded = !item.IsExpanded;
                    }
                    else
                    {
                        SelectedItem = item;
                        item.NavigateCommand?.Execute(item);
                    }
                }));

        private RelayCommand _clearSearchCommand;
        public RelayCommand ClearSearchCommand =>
            _clearSearchCommand ?? (_clearSearchCommand = new RelayCommand(() => SearchText = string.Empty));

        private RelayCommand<string> _switchThemeCommand;
        public RelayCommand<string> SwitchThemeCommand =>
            _switchThemeCommand ?? (_switchThemeCommand = new RelayCommand<string>(theme => CurrentTheme = theme));

        #endregion

        #region 构造

        public SidebarViewModel()
        {
            Groups = new ObservableCollection<SidebarGroupViewModel>();
        }

        #endregion

        #region 搜索过滤

        private void ApplySearch(string keyword)
        {
            bool hasKeyword = !string.IsNullOrWhiteSpace(keyword);
            IsSearchActive = hasKeyword;
            keyword = keyword?.Trim().ToLowerInvariant();

            foreach (var group in Groups)
            {
                bool groupHasVisibleItem = false;
                foreach (var item in group.Items)
                {
                    bool visible = ApplySearchRecursive(item, keyword, hasKeyword);
                    if (visible) groupHasVisibleItem = true;
                }
                group.IsVisibleInSearch = !hasKeyword || groupHasVisibleItem;
            }
        }

        /// <summary>递归搜索：如果本项或任一子项匹配，则可见并自动展开</summary>
        private bool ApplySearchRecursive(SidebarItemViewModel item, string keyword, bool hasKeyword)
        {
            if (!hasKeyword)
            {
                item.IsVisibleInSearch = true;
                item.IsSearchHighlighted = false;
                if (item.Children != null)
                    foreach (var c in item.Children)
                        ApplySearchRecursive(c, keyword, false);
                return true;
            }

            bool selfMatch = item.Title != null &&
                             item.Title.ToLowerInvariant().Contains(keyword);
            item.IsSearchHighlighted = selfMatch;

            bool childMatch = false;
            if (item.Children != null)
            {
                foreach (var child in item.Children)
                {
                    if (ApplySearchRecursive(child, keyword, true))
                        childMatch = true;
                }
            }

            bool visible = selfMatch || childMatch;
            item.IsVisibleInSearch = visible;

            // 子项匹配时自动展开父级
            if (childMatch)
                item.IsExpanded = true;

            return visible;
        }

        #endregion

        #region 键盘导航

        /// <summary>获取当前可见的扁平化项列表</summary>
        public List<SidebarItemViewModel> GetVisibleFlatList()
        {
            var list = new List<SidebarItemViewModel>();
            foreach (var group in Groups)
            {
                if (!group.IsVisibleInSearch) continue;
                foreach (var item in group.Items)
                {
                    CollectVisible(item, list);
                }
            }
            return list;
        }

        private void CollectVisible(SidebarItemViewModel item, List<SidebarItemViewModel> list)
        {
            if (!item.IsVisibleInSearch || !item.IsEnabled) return;
            list.Add(item);
            if (item.IsExpanded && item.Children != null)
            {
                foreach (var child in item.Children)
                    CollectVisible(child, list);
            }
        }

        /// <summary>键盘导航处理</summary>
        public void HandleKeyNavigation(Key key)
        {
            var flatList = GetVisibleFlatList();
            if (flatList.Count == 0) return;

            int currentIndex = SelectedItem != null ? flatList.IndexOf(SelectedItem) : -1;

            switch (key)
            {
                case Key.Down:
                    if (currentIndex < flatList.Count - 1)
                        SelectedItem = flatList[currentIndex + 1];
                    else if (currentIndex == -1 && flatList.Count > 0)
                        SelectedItem = flatList[0];
                    break;

                case Key.Up:
                    if (currentIndex > 0)
                        SelectedItem = flatList[currentIndex - 1];
                    break;

                case Key.Right:
                    if (SelectedItem != null && SelectedItem.HasChildren)
                        SelectedItem.IsExpanded = true;
                    break;

                case Key.Left:
                    if (SelectedItem != null)
                    {
                        if (SelectedItem.HasChildren && SelectedItem.IsExpanded)
                            SelectedItem.IsExpanded = false;
                        else if (SelectedItem.Parent != null)
                            SelectedItem = SelectedItem.Parent;
                    }
                    break;

                case Key.Enter:
                case Key.Space:
                    if (SelectedItem != null)
                        SelectItemCommand.Execute(SelectedItem);
                    break;
            }
        }

        #endregion

        #region 公开辅助

        /// <summary>按 Id 查找项</summary>
        public SidebarItemViewModel FindItemById(string id)
        {
            foreach (var group in Groups)
                foreach (var item in group.Items)
                {
                    var found = item.FindById(id);
                    if (found != null) return found;
                }
            return null;
        }

        /// <summary>按 Id 选中项</summary>
        public bool SelectById(string id)
        {
            var item = FindItemById(id);
            if (item != null)
            {
                SelectedItem = item;
                return true;
            }
            return false;
        }

        /// <summary>获取所有项的扁平列表</summary>
        public IEnumerable<SidebarItemViewModel> GetAllItems()
        {
            return Groups.SelectMany(g => g.Items.SelectMany(i =>
                new[] { i }.Concat(i.GetAllDescendants())));
        }

        #endregion
    }
}
