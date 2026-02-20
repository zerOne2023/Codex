using System.Collections.Generic;
using SidebarNav.ViewModels;

namespace SidebarNav.Services
{
    /// <summary>
    /// 导航历史栈服务 —— 支持后退
    /// </summary>
    public class NavigationHistoryService
    {
        private readonly Stack<SidebarItemViewModel> _backStack = new Stack<SidebarItemViewModel>();
        private readonly int _maxDepth;

        public bool CanGoBack => _backStack.Count > 0;
        public int Count => _backStack.Count;

        public NavigationHistoryService(int maxDepth = 50)
        {
            _maxDepth = maxDepth;
        }

        public void Push(SidebarItemViewModel item)
        {
            if (item == null) return;
            _backStack.Push(item);

            // 防止内存膨胀：超限后转移到新栈裁剪
            if (_backStack.Count > _maxDepth)
            {
                var temp = new Stack<SidebarItemViewModel>();
                int keep = _maxDepth / 2;
                int i = 0;
                foreach (var it in _backStack)
                {
                    if (i++ >= keep) break;
                    temp.Push(it);
                }
                _backStack.Clear();
                foreach (var it in temp)
                    _backStack.Push(it);
            }
        }

        public SidebarItemViewModel Pop()
        {
            return _backStack.Count > 0 ? _backStack.Pop() : null;
        }

        public SidebarItemViewModel Peek()
        {
            return _backStack.Count > 0 ? _backStack.Peek() : null;
        }

        public void Clear()
        {
            _backStack.Clear();
        }
    }
}
