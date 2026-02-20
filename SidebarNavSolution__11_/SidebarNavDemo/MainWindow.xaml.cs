using System.Windows;
using SidebarNav.Services;

namespace SidebarNavDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLightTheme(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.Sidebar.CurrentTheme = "Light";
        }

        private void OnDarkTheme(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.Sidebar.CurrentTheme = "Dark";
        }

        private void OnBlueTheme(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.Sidebar.CurrentTheme = "Blue";
        }
    }
}
