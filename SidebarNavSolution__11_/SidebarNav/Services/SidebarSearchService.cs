using System;
using System.Collections.Generic;
using System.Linq;
using SidebarNav.ViewModels;

namespace SidebarNav.Services
{
    /// <summary>
    /// 搜索服务 —— 支持递归匹配、拼音首字母（可扩展）、多字段搜索
    /// </summary>
    public static class SidebarSearchService
    {
        /// <summary>
        /// 递归过滤项目树，返回匹配的项
        /// </summary>
        public static List<SidebarItemViewModel> Search(
            IEnumerable<SidebarItemViewModel> items,
            string keyword,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var results = new List<SidebarItemViewModel>();
            if (string.IsNullOrWhiteSpace(keyword) || items == null) return results;

            foreach (var item in items)
            {
                SearchRecursive(item, keyword, comparison, results);
            }
            return results;
        }

        private static bool SearchRecursive(
            SidebarItemViewModel item,
            string keyword,
            StringComparison comparison,
            List<SidebarItemViewModel> results)
        {
            bool selfMatch = MatchItem(item, keyword, comparison);

            bool childMatch = false;
            if (item.Children != null)
            {
                foreach (var child in item.Children)
                {
                    if (SearchRecursive(child, keyword, comparison, results))
                        childMatch = true;
                }
            }

            if (selfMatch)
            {
                results.Add(item);
                return true;
            }

            return childMatch;
        }

        /// <summary>匹配逻辑 —— 可扩展为拼音/标签搜索</summary>
        private static bool MatchItem(SidebarItemViewModel item, string keyword, StringComparison comparison)
        {
            if (item.Title != null && item.Title.IndexOf(keyword, comparison) >= 0)
                return true;

            if (item.Id != null && item.Id.IndexOf(keyword, comparison) >= 0)
                return true;

            if (item.ToolTip != null && item.ToolTip.IndexOf(keyword, comparison) >= 0)
                return true;

            // Tag 字符串匹配
            if (item.Tag is string tagStr && tagStr.IndexOf(keyword, comparison) >= 0)
                return true;

            return false;
        }
    }
}
