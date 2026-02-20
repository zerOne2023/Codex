using System.Collections.ObjectModel;
using System.Windows.Media;
using SidebarNav.ViewModels;

namespace SidebarNavDemo
{
    /// <summary>
    /// Demo 主 ViewModel —— 展示全部功能
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        public SidebarViewModel Sidebar { get; }

        private string _currentPageTitle = "Welcome";
        public string CurrentPageTitle
        {
            get => _currentPageTitle;
            set => SetProperty(ref _currentPageTitle, value);
        }

        private string _currentPageDescription = "Select a navigation item from the sidebar to get started.";
        public string CurrentPageDescription
        {
            get => _currentPageDescription;
            set => SetProperty(ref _currentPageDescription, value);
        }

        public MainViewModel()
        {
            Sidebar = new SidebarViewModel
            {
                ExpandedWidth = 260,
                MiniWidth = 64,
                SearchPlaceholder = "Search navigation…"
            };

            BuildNavigationTree();

            Sidebar.SelectedItemChanged += (s, item) =>
            {
                if (item != null)
                {
                    CurrentPageTitle = item.Title;
                    CurrentPageDescription = $"Navigated to: {item.Title}  (Id: {item.Id}, Level: {item.Level})";
                }
            };

            // 默认选中第一项
            Sidebar.SelectById("home");
        }

        private void BuildNavigationTree()
        {
            // ═══════════ Group 1: Main (无标题) ═══════════
            var mainGroup = new SidebarGroupViewModel { GroupName = null, ShowSeparator = false };

            mainGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "home",
                Title = "Home",
                ToolTip = "Home Page",
                IconPathDataString = "M10.5495 2.53189C11.3874 1.82531 12.6126 1.82531 13.4505 2.5319L20.2005 8.224C20.7074 8.65152 21 9.2809 21 9.94406V19.7468C21 20.7133 20.2165 21.4968 19.25 21.4968H15.75C14.7835 21.4968 14 20.7133 14 19.7468V14.2468C14 14.1088 13.8881 13.9968 13.75 13.9968H10.25C10.1119 13.9968 9.99999 14.1088 9.99999 14.2468V19.7468C9.99999 20.7133 9.2165 21.4968 8.25 21.4968H4.75C3.7835 21.4968 3 20.7133 3 19.7468V9.94406C3 9.2809 3.29255 8.65152 3.79952 8.224L10.5495 2.53189Z"
            });

            mainGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "dashboard",
                Title = "Dashboard",
                ToolTip = "Dashboard Overview",
                IconPathDataString = "M4 5a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V5Zm10 0a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1h-4a1 1 0 0 1-1-1V5ZM4 15a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1v-4Zm10 0a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1h-4a1 1 0 0 1-1-1v-4Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 5
            });

            mainGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "inbox",
                Title = "Inbox",
                ToolTip = "Messages",
                IconPathDataString = "M21.75 6.75v10.5a2.25 2.25 0 0 1-2.25 2.25h-15a2.25 2.25 0 0 1-2.25-2.25V6.75m19.5 0A2.25 2.25 0 0 0 19.5 4.5h-15a2.25 2.25 0 0 0-2.25 2.25m19.5 0v.243a2.25 2.25 0 0 1-1.07 1.916l-7.5 4.615a2.25 2.25 0 0 1-2.36 0L3.32 8.91a2.25 2.25 0 0 1-1.07-1.916V6.75",
                BadgeType = BadgeType.Number,
                BadgeCount = 128
            });

            mainGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "notifications",
                Title = "Notifications",
                ToolTip = "Notification Center",
                IconPathDataString = "M14.857 17.082a23.848 23.848 0 0 0 5.454-1.31A8.967 8.967 0 0 1 18 9.75V9A6 6 0 0 0 6 9v.75a8.967 8.967 0 0 1-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 0 1-5.714 0m5.714 0a3 3 0 1 1-5.714 0",
                BadgeType = BadgeType.Dot
            });

            // ═══════════ Group 2: Content ═══════════
            var contentGroup = new SidebarGroupViewModel("CONTENT");

            // 多级嵌套: Documents
            var documents = new SidebarItemViewModel
            {
                Id = "documents",
                Title = "Documents",
                ToolTip = "Document Manager",
                IconPathDataString = "M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m2.25 0H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Z",
                BadgeType = BadgeType.Dot,
                Children = new ObservableCollection<SidebarItemViewModel>
                {
                    new SidebarItemViewModel
                    {
                        Id = "docs-reports",
                        Title = "Reports",
                        IconPathDataString = "M3 13.125C3 12.504 3.504 12 4.125 12h2.25c.621 0 1.125.504 1.125 1.125v6.75C7.5 20.496 6.996 21 6.375 21h-2.25A1.125 1.125 0 0 1 3 19.875v-6.75ZM9.75 8.625c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125v11.25c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 0 1-1.125-1.125V8.625ZM16.5 4.125c0-.621.504-1.125 1.125-1.125h2.25C20.496 3 21 3.504 21 4.125v15.75c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 0 1-1.125-1.125V4.125Z",
                        BadgeType = BadgeType.Number,
                        BadgeCount = 3,
                        Children = new ObservableCollection<SidebarItemViewModel>
                        {
                            new SidebarItemViewModel { Id = "docs-reports-monthly", Title = "Monthly" },
                            new SidebarItemViewModel { Id = "docs-reports-annual", Title = "Annual", BadgeType = BadgeType.Text, BadgeText = "NEW" },
                            new SidebarItemViewModel { Id = "docs-reports-custom", Title = "Custom Reports" }
                        }
                    },
                    new SidebarItemViewModel { Id = "docs-templates", Title = "Templates" },
                    new SidebarItemViewModel { Id = "docs-shared", Title = "Shared With Me", BadgeType = BadgeType.Number, BadgeCount = 12 },
                    new SidebarItemViewModel { Id = "docs-archive", Title = "Archive" }
                }
            };
            contentGroup.Items.Add(documents);

            contentGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "media",
                Title = "Media Library",
                IconPathDataString = "M2.25 15.75l5.159-5.159a2.25 2.25 0 0 1 3.182 0l5.159 5.159m-1.5-1.5 1.409-1.409a2.25 2.25 0 0 1 3.182 0l2.909 2.909m-18 3.75h16.5a1.5 1.5 0 0 0 1.5-1.5V6a1.5 1.5 0 0 0-1.5-1.5H3.75A1.5 1.5 0 0 0 2.25 6v12a1.5 1.5 0 0 0 1.5 1.5Zm10.5-11.25h.008v.008h-.008V8.25Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z",
                BadgeType = BadgeType.Text,
                BadgeText = "PRO"
            });

            contentGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "calendar",
                Title = "Calendar",
                IconPathDataString = "M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5"
            });

            contentGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "tasks",
                Title = "Tasks",
                ToolTip = "Task Management",
                IconPathDataString = "M9 12.75L11.25 15 15 9.75M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 23
            });

            // ═══════════ Group 3: Analytics ═══════════
            var analyticsGroup = new SidebarGroupViewModel("ANALYTICS");

            var analytics = new SidebarItemViewModel
            {
                Id = "analytics",
                Title = "Analytics",
                ToolTip = "Data Analytics",
                IconPathDataString = "M3 13.125C3 12.504 3.504 12 4.125 12h2.25c.621 0 1.125.504 1.125 1.125v6.75C7.5 20.496 6.996 21 6.375 21h-2.25A1.125 1.125 0 0 1 3 19.875v-6.75ZM9.75 8.625c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125v11.25c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 0 1-1.125-1.125V8.625ZM16.5 4.125c0-.621.504-1.125 1.125-1.125h2.25C20.496 3 21 3.504 21 4.125v15.75c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 0 1-1.125-1.125V4.125Z",
                Children = new ObservableCollection<SidebarItemViewModel>
                {
                    new SidebarItemViewModel { Id = "analytics-overview", Title = "Overview" },
                    new SidebarItemViewModel { Id = "analytics-traffic", Title = "Traffic", BadgeType = BadgeType.Text, BadgeText = "LIVE" },
                    new SidebarItemViewModel { Id = "analytics-revenue", Title = "Revenue" },
                    new SidebarItemViewModel { Id = "analytics-conversion", Title = "Conversion Rate" }
                }
            };
            analyticsGroup.Items.Add(analytics);

            analyticsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "funnel",
                Title = "Sales Funnel",
                IconPathDataString = "M12 3C17.5228 3 22 4.11929 22 5.5C22 6.88071 17.5228 8 12 8C6.47715 8 2 6.88071 2 5.5C2 4.11929 6.47715 3 12 3ZM4 8.5C4 8.5 7 11 12 11C17 11 20 8.5 20 8.5V12.5C20 12.5 17 15 12 15C7 15 4 12.5 4 12.5V8.5ZM6 15.5C6 15.5 8 17 12 17C16 17 18 15.5 18 15.5V18.5C18 19.8807 15.3137 21 12 21C8.68629 21 6 19.8807 6 18.5V15.5Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 47
            });

            analyticsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "heatmap",
                Title = "Heatmap",
                IconPathDataString = "M15.362 5.214A8.252 8.252 0 0 1 12 21 8.25 8.25 0 0 1 6.038 7.047 8.287 8.287 0 0 0 9 9.601a8.983 8.983 0 0 1 3.361-6.867 8.21 8.21 0 0 0 3 2.48Z",
                BadgeType = BadgeType.Text,
                BadgeText = "BETA"
            });

            analyticsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "export",
                Title = "Export Data",
                IconPathDataString = "M3 16.5v2.25A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75V16.5M16.5 12L12 16.5m0 0L7.5 12m4.5 4.5V3"
            });

            // ═══════════ Group 4: Communication ═══════════
            var commGroup = new SidebarGroupViewModel("COMMUNICATION");

            commGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "chat",
                Title = "Chat",
                ToolTip = "Team Chat",
                IconPathDataString = "M8.625 12a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H8.25m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0H12m4.125 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm0 0h-.375M21 12c0 4.556-4.03 8.25-9 8.25a9.764 9.764 0 0 1-2.555-.337A5.972 5.972 0 0 1 5.41 20.97a5.969 5.969 0 0 1-.474-.065 4.48 4.48 0 0 0 .978-2.025c.09-.457-.133-.901-.467-1.226C3.93 16.178 3 14.189 3 12c0-4.556 4.03-8.25 9-8.25s9 3.694 9 8.25Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 3
            });

            commGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "contacts",
                Title = "Contacts",
                IconPathDataString = "M15 9h3.75M15 12h3.75M15 15h3.75M4.5 19.5h15a2.25 2.25 0 0 0 2.25-2.25V6.75A2.25 2.25 0 0 0 19.5 4.5h-15a2.25 2.25 0 0 0-2.25 2.25v10.5A2.25 2.25 0 0 0 4.5 19.5Zm6-10.125a1.875 1.875 0 1 1-3.75 0 1.875 1.875 0 0 1 3.75 0Zm1.294 6.336a6.721 6.721 0 0 1-3.17.789 6.721 6.721 0 0 1-3.168-.789 3.376 3.376 0 0 1 6.338 0Z"
            });

            var meetings = new SidebarItemViewModel
            {
                Id = "meetings",
                Title = "Meetings",
                IconPathDataString = "M15.75 10.5l4.72-4.72a.75.75 0 0 1 1.28.53v11.38a.75.75 0 0 1-1.28.53l-4.72-4.72M4.5 18.75h9a2.25 2.25 0 0 0 2.25-2.25v-9a2.25 2.25 0 0 0-2.25-2.25h-9A2.25 2.25 0 0 0 2.25 7.5v9a2.25 2.25 0 0 0 2.25 2.25Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 2,
                Children = new ObservableCollection<SidebarItemViewModel>
                {
                    new SidebarItemViewModel { Id = "meetings-upcoming", Title = "Upcoming", BadgeType = BadgeType.Number, BadgeCount = 2 },
                    new SidebarItemViewModel { Id = "meetings-past", Title = "Past Meetings" },
                    new SidebarItemViewModel { Id = "meetings-recordings", Title = "Recordings", BadgeType = BadgeType.Text, BadgeText = "NEW" }
                }
            };
            commGroup.Items.Add(meetings);

            commGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "announcements",
                Title = "Announcements",
                IconPathDataString = "M10.34 15.84c-.688-.06-1.386-.09-2.09-.09H7.5a4.5 4.5 0 1 1 0-9h.75c.704 0 1.402-.03 2.09-.09m0 9.18c.253.962.584 1.892.985 2.783.247.55.06 1.21-.463 1.511l-.657.38a.75.75 0 0 1-1.042-.29l-.252-.44a16.1 16.1 0 0 1-1.571-4.944m5-9.18c.068.023.137.044.205.068a15.98 15.98 0 0 1 1.366 4.932 15.976 15.976 0 0 1-1.366 4.932c-.068.023-.137.044-.205.068m0-9.932a15.76 15.76 0 0 0 0 9.864"
            });

            // ═══════════ Group 5: E-Commerce ═══════════
            var shopGroup = new SidebarGroupViewModel("E-COMMERCE");

            var products = new SidebarItemViewModel
            {
                Id = "products",
                Title = "Products",
                IconPathDataString = "M20.25 7.5l-.625 10.632a2.25 2.25 0 0 1-2.247 2.118H6.622a2.25 2.25 0 0 1-2.247-2.118L3.75 7.5M10 11.25h4M3.375 7.5h17.25c.621 0 1.125-.504 1.125-1.125v-1.5c0-.621-.504-1.125-1.125-1.125H3.375c-.621 0-1.125.504-1.125 1.125v1.5c0 .621.504 1.125 1.125 1.125Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 156,
                Children = new ObservableCollection<SidebarItemViewModel>
                {
                    new SidebarItemViewModel { Id = "products-all", Title = "All Products" },
                    new SidebarItemViewModel { Id = "products-categories", Title = "Categories" },
                    new SidebarItemViewModel { Id = "products-inventory", Title = "Inventory", BadgeType = BadgeType.Dot },
                    new SidebarItemViewModel { Id = "products-pricing", Title = "Pricing" }
                }
            };
            shopGroup.Items.Add(products);

            shopGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "orders",
                Title = "Orders",
                IconPathDataString = "M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 0 0-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 0 0-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Zm12.75 0a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 18
            });

            shopGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "customers",
                Title = "Customers",
                IconPathDataString = "M18 18.72a9.094 9.094 0 0 0 3.741-.479 3 3 0 0 0-4.682-2.72m.94 3.198.001.031c0 .225-.012.447-.037.666A11.944 11.944 0 0 1 12 21c-2.17 0-4.207-.576-5.963-1.584A6.062 6.062 0 0 1 6 18.719m12 0a5.971 5.971 0 0 0-.941-3.197m0 0A5.995 5.995 0 0 0 12 12.75a5.995 5.995 0 0 0-5.058 2.772m0 0a3 3 0 0 0-4.681 2.72 8.986 8.986 0 0 0 3.74.477m.94-3.197a5.971 5.971 0 0 0-.94 3.197M15 6.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Zm6 3a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Zm-13.5 0a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Z"
            });

            shopGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "coupons",
                Title = "Coupons",
                IconPathDataString = "M16.5 6v.75m0 3v.75m0 3v.75m0 3V18m-9-5.25h5.25M7.5 15h3M3.375 5.25c-.621 0-1.125.504-1.125 1.125v3.026a2.999 2.999 0 0 1 0 5.198v3.026c0 .621.504 1.125 1.125 1.125h17.25c.621 0 1.125-.504 1.125-1.125v-3.026a2.999 2.999 0 0 1 0-5.198V6.375c0-.621-.504-1.125-1.125-1.125H3.375Z",
                BadgeType = BadgeType.Text,
                BadgeText = "SALE"
            });

            // ═══════════ Group 6: System ═══════════
            var settingsGroup = new SidebarGroupViewModel("SYSTEM");

            var settings = new SidebarItemViewModel
            {
                Id = "settings",
                Title = "Settings",
                ToolTip = "Application Settings",
                IconPathDataString = "M9.594 3.94c.09-.542.56-.94 1.11-.94h2.593c.55 0 1.02.398 1.11.94l.213 1.281c.063.374.313.686.645.87.074.04.147.083.22.127.324.196.72.257 1.075.124l1.217-.456a1.125 1.125 0 0 1 1.37.49l1.296 2.247a1.125 1.125 0 0 1-.26 1.431l-1.003.827c-.293.24-.438.613-.431.992a6.759 6.759 0 0 1 0 .255c-.007.378.138.75.43.99l1.005.828c.424.35.534.954.26 1.43l-1.298 2.247a1.125 1.125 0 0 1-1.369.491l-1.217-.456c-.355-.133-.75-.072-1.076.124a6.57 6.57 0 0 1-.22.128c-.331.183-.581.495-.644.869l-.213 1.28c-.09.543-.56.941-1.11.941h-2.594c-.55 0-1.02-.398-1.11-.94l-.213-1.281c-.062-.374-.312-.686-.644-.87a6.52 6.52 0 0 1-.22-.127c-.325-.196-.72-.257-1.076-.124l-1.217.456a1.125 1.125 0 0 1-1.369-.49l-1.297-2.247a1.125 1.125 0 0 1 .26-1.431l1.004-.827c.292-.24.437-.613.43-.992a6.932 6.932 0 0 1 0-.255c.007-.378-.138-.75-.43-.99l-1.004-.828a1.125 1.125 0 0 1-.26-1.43l1.297-2.247a1.125 1.125 0 0 1 1.37-.491l1.216.456c.356.133.751.072 1.076-.124.072-.044.146-.087.22-.128.332-.183.582-.495.644-.869l.214-1.281Z",
                Children = new ObservableCollection<SidebarItemViewModel>
                {
                    new SidebarItemViewModel { Id = "settings-general", Title = "General" },
                    new SidebarItemViewModel { Id = "settings-appearance", Title = "Appearance" },
                    new SidebarItemViewModel { Id = "settings-notifications", Title = "Notifications", BadgeType = BadgeType.Dot },
                    new SidebarItemViewModel { Id = "settings-security", Title = "Security" },
                    new SidebarItemViewModel { Id = "settings-integrations", Title = "Integrations", BadgeType = BadgeType.Number, BadgeCount = 6 },
                    new SidebarItemViewModel { Id = "settings-advanced", Title = "Advanced", IsEnabled = false }
                }
            };
            settingsGroup.Items.Add(settings);

            settingsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "users",
                Title = "User Management",
                IconPathDataString = "M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z",
                BadgeType = BadgeType.Number,
                BadgeCount = 8
            });

            settingsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "audit",
                Title = "Audit Log",
                IconPathDataString = "M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m0 12.75h7.5m-7.5 3H12M10.5 2.25H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Z"
            });

            settingsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "help",
                Title = "Help & Support",
                IconPathDataString = "M9.879 7.519c1.171-1.025 3.071-1.025 4.242 0 1.172 1.025 1.172 2.687 0 3.712-.203.179-.43.326-.67.442-.745.361-1.45.999-1.45 1.827v.75M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9 5.25h.008v.008H12v-.008Z"
            });

            settingsGroup.Items.Add(new SidebarItemViewModel
            {
                Id = "about",
                Title = "About",
                IconPathDataString = "M11.25 11.25l.041-.02a.75.75 0 0 1 1.063.852l-.708 2.836a.75.75 0 0 0 1.063.853l.041-.021M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9-3.75h.008v.008H12V8.25Z"
            });

            // 添加全部分组
            Sidebar.Groups.Add(mainGroup);
            Sidebar.Groups.Add(contentGroup);
            Sidebar.Groups.Add(analyticsGroup);
            Sidebar.Groups.Add(commGroup);
            Sidebar.Groups.Add(shopGroup);
            Sidebar.Groups.Add(settingsGroup);

            // 为所有子项设置正确的层级
            foreach (var group in Sidebar.Groups)
            {
                foreach (var item in group.Items)
                {
                    SetLevelsRecursive(item, 0);
                }
            }
        }

        private void SetLevelsRecursive(SidebarItemViewModel item, int level)
        {
            item.Level = level;
            if (item.Children != null)
            {
                foreach (var child in item.Children)
                {
                    child.Parent = item;
                    SetLevelsRecursive(child, level + 1);
                }
            }
        }
    }
}
