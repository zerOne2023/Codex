using System;
using System.Collections.Generic;
using System.Windows;

namespace SidebarNav.Services
{
    /// <summary>
    /// 主题管理器 —— 支持动态切换主题资源字典
    /// 
    /// 用法：
    ///   ThemeManager.RegisterTheme("Dark", new Uri("pack://application:,,,/SidebarNav;component/Themes/DarkTheme.xaml"));
    ///   ThemeManager.ApplyTheme("Dark");
    /// 
    /// 或者使用程序外部资源：
    ///   ThemeManager.RegisterTheme("Custom", new Uri("/MyApp;component/Themes/CustomSidebar.xaml", UriKind.Relative));
    /// </summary>
    public static class ThemeManager
    {
        private static readonly Dictionary<string, Uri> _themes = new Dictionary<string, Uri>(StringComparer.OrdinalIgnoreCase);
        private static ResourceDictionary _currentThemeDict;
        private static string _currentThemeName;

        /// <summary>当前主题名称</summary>
        public static string CurrentTheme => _currentThemeName;

        /// <summary>主题切换事件</summary>
        public static event EventHandler<string> ThemeChanged;

        static ThemeManager()
        {
            // 注册内置主题
            RegisterTheme("Light", new Uri("pack://application:,,,/SidebarNav;component/Themes/LightTheme.xaml"));
            RegisterTheme("Dark", new Uri("pack://application:,,,/SidebarNav;component/Themes/DarkTheme.xaml"));
            RegisterTheme("Blue", new Uri("pack://application:,,,/SidebarNav;component/Themes/BlueTheme.xaml"));
        }

        /// <summary>注册自定义主题</summary>
        public static void RegisterTheme(string name, Uri resourceUri)
        {
            _themes[name] = resourceUri;
        }

        /// <summary>注册自定义主题（直接传 ResourceDictionary）</summary>
        public static void RegisterTheme(string name, ResourceDictionary dict)
        {
            // 存到临时 URI（用于标记），直接应用时走别的路径
            _themes[name] = null;
            _customDicts[name] = dict;
        }

        private static readonly Dictionary<string, ResourceDictionary> _customDicts =
            new Dictionary<string, ResourceDictionary>(StringComparer.OrdinalIgnoreCase);

        /// <summary>应用指定主题</summary>
        public static void ApplyTheme(string themeName)
        {
            if (Application.Current == null) return;

            var mergedDicts = Application.Current.Resources.MergedDictionaries;

            // 移除旧主题
            if (_currentThemeDict != null)
            {
                mergedDicts.Remove(_currentThemeDict);
                _currentThemeDict = null;
            }

            ResourceDictionary newDict = null;

            if (_customDicts.ContainsKey(themeName))
            {
                newDict = _customDicts[themeName];
            }
            else if (_themes.ContainsKey(themeName))
            {
                newDict = new ResourceDictionary { Source = _themes[themeName] };
            }

            if (newDict != null)
            {
                mergedDicts.Add(newDict);
                _currentThemeDict = newDict;
                _currentThemeName = themeName;
                ThemeChanged?.Invoke(null, themeName);
            }
        }

        /// <summary>获取所有已注册主题名称</summary>
        public static IEnumerable<string> GetAvailableThemes()
        {
            return _themes.Keys;
        }

        /// <summary>获取当前主题中指定资源</summary>
        public static object GetResource(string key)
        {
            if (_currentThemeDict != null && _currentThemeDict.Contains(key))
                return _currentThemeDict[key];
            if (Application.Current != null && Application.Current.Resources.Contains(key))
                return Application.Current.Resources[key];
            return null;
        }
    }
}
