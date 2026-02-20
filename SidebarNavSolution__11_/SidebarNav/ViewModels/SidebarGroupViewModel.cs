using System.Collections.ObjectModel;

namespace SidebarNav.ViewModels
{
    /// <summary>
    /// 分组 ViewModel —— 带分组标题和分隔线
    /// </summary>
    public class SidebarGroupViewModel : ObservableObject
    {
        private string _groupName;
        /// <summary>分组标题</summary>
        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        private bool _showSeparator = true;
        /// <summary>是否在分组前显示分隔线</summary>
        public bool ShowSeparator
        {
            get => _showSeparator;
            set => SetProperty(ref _showSeparator, value);
        }

        private bool _isCollapsed;
        /// <summary>分组是否折叠</summary>
        public bool IsCollapsed
        {
            get => _isCollapsed;
            set => SetProperty(ref _isCollapsed, value);
        }

        private bool _isVisibleInSearch = true;
        public bool IsVisibleInSearch
        {
            get => _isVisibleInSearch;
            set => SetProperty(ref _isVisibleInSearch, value);
        }

        private ObservableCollection<SidebarItemViewModel> _items;
        /// <summary>分组下的导航项</summary>
        public ObservableCollection<SidebarItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public SidebarGroupViewModel()
        {
            Items = new ObservableCollection<SidebarItemViewModel>();
        }

        public SidebarGroupViewModel(string groupName) : this()
        {
            GroupName = groupName;
        }
    }
}
