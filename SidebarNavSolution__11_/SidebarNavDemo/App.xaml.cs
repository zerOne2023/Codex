using System.Windows;
using SidebarNav.Services;

namespace SidebarNavDemo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 应用默认 Light 主题
            ThemeManager.ApplyTheme("Light");
        }
    }
}
