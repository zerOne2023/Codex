# SidebarNav — WPF Sidebar Navigation Custom Control Library

> 适用于 **Win7+ / VS2019 / .NET Framework 4.7.2** 的企业级侧边栏导航控件库。
> 纯 MVVM 架构，零第三方依赖，开箱即用，主题可切换。

---

## ✨ 核心功能

| 功能 | 说明 |
|------|------|
| **嵌套子项** | 无限层级递归展开，支持 TreeView 式多级导航 |
| **实时搜索过滤** | 输入关键字实时递归匹配，命中项高亮，父级自动展开 |
| **3 种徽章** | `Number`（数字，>99 显示 99+）、`Dot`（红点）、`Text`（文本标签） |
| **3 种图标源** | `PathData`（矢量 SVG Path）、`FontIcon`（字体图标）、`Image`（图片 URI） |
| **分组标题 + 分隔线** | 可折叠分组、自定义分组名、可选分隔线 |
| **展开/迷你模式** | 动画切换，支持自定义宽度和动画时长 |
| **Header/Footer 插槽** | 自定义 Logo、品牌栏、用户信息、操作按钮 |
| **导航历史后退** | 自动记录历史栈，支持 GoBack 操作 |
| **键盘导航** | ↑↓ 选择、←→ 展开/折叠、Enter 确认 |
| **主题切换** | 内置 Light/Dark/Blue，支持注册自定义主题 |
| **MVVM** | ViewModel 驱动，所有属性可绑定，事件可命令化 |

---

## 📁 项目结构

```
SidebarNavSolution.sln
│
├── SidebarNav/                        # 控件库（编译为 DLL）
│   ├── Controls/
│   │   ├── SidebarNavigation.cs       # 主容器控件
│   │   ├── SidebarNavItem.cs          # 导航项控件
│   │   ├── SidebarNavGroup.cs         # 分组控件
│   │   ├── SidebarBadge.cs            # 徽章控件
│   │   ├── SidebarSearchBox.cs        # 搜索框控件
│   │   └── SidebarIconPresenter.cs    # 图标呈现器
│   ├── ViewModels/
│   │   ├── ObservableObject.cs        # MVVM 基类
│   │   ├── RelayCommand.cs            # 命令实现
│   │   ├── SidebarItemViewModel.cs    # 导航项 ViewModel
│   │   ├── SidebarGroupViewModel.cs   # 分组 ViewModel
│   │   └── SidebarViewModel.cs        # 主 ViewModel
│   ├── Services/
│   │   ├── ThemeManager.cs            # 主题管理器
│   │   ├── NavigationHistoryService.cs# 导航历史栈
│   │   └── SidebarSearchService.cs    # 搜索服务
│   ├── Converters/
│   │   └── BoolToVisibilityConverter.cs  # 全部值转换器
│   └── Themes/
│       ├── Generic.xaml               # 控件默认样式入口
│       ├── LightTheme.xaml            # 亮色主题
│       ├── DarkTheme.xaml             # 暗色主题
│       ├── BlueTheme.xaml             # 蓝色主题
│       ├── SidebarNavigationStyle.xaml
│       ├── SidebarNavItemStyle.xaml
│       ├── SidebarNavGroupStyle.xaml
│       ├── SidebarBadgeStyle.xaml
│       ├── SidebarSearchBoxStyle.xaml
│       └── SidebarIconPresenterStyle.xaml
│
└── SidebarNavDemo/                    # 演示程序
    ├── MainWindow.xaml/cs
    ├── MainViewModel.cs
    └── ...
```

---

## 🚀 快速上手

### 1. 引用控件库

在目标项目中添加对 `SidebarNav.dll` 的引用（或项目引用）。

### 2. App.xaml 引入主题

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/SidebarNav;component/Themes/Generic.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 3. XAML 中使用控件

```xml
xmlns:sidebar="clr-namespace:SidebarNav.Controls;assembly=SidebarNav"

<sidebar:SidebarNavigation
    ViewModel="{Binding SidebarVM}"
    IsExpanded="True"
    ExpandedWidth="260"
    MiniWidth="64"
    ShowSearchBox="True"
    ShowBackButton="True">

    <sidebar:SidebarNavigation.HeaderContent>
        <!-- 自定义 Logo 区域 -->
    </sidebar:SidebarNavigation.HeaderContent>

    <sidebar:SidebarNavigation.FooterContent>
        <!-- 自定义用户信息区域 -->
    </sidebar:SidebarNavigation.FooterContent>

</sidebar:SidebarNavigation>
```

### 4. ViewModel 配置导航树

```csharp
var sidebar = new SidebarViewModel();

var group = new SidebarGroupViewModel("MAIN");
group.Items.Add(new SidebarItemViewModel
{
    Id = "home",
    Title = "Home",
    IconPathDataString = "M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z",
    BadgeType = BadgeType.Number,
    BadgeCount = 5,
    Children = new ObservableCollection<SidebarItemViewModel>
    {
        new SidebarItemViewModel { Id = "sub1", Title = "Sub Page 1" },
        new SidebarItemViewModel { Id = "sub2", Title = "Sub Page 2" }
    }
});

sidebar.Groups.Add(group);

// 监听选中变更
sidebar.SelectedItemChanged += (s, item) =>
{
    // 导航到对应页面
};
```

---

## 🎨 主题系统

### 内置主题
- `Light` — 浅色（默认）
- `Dark` — 深色（Catppuccin 风格）
- `Blue` — 深蓝

### 切换主题
```csharp
// 方式一：通过 ViewModel
sidebarVM.CurrentTheme = "Dark";

// 方式二：直接调用 ThemeManager
ThemeManager.ApplyTheme("Dark");
```

### 注册自定义主题
```csharp
// 使用 URI
ThemeManager.RegisterTheme("Corporate",
    new Uri("pack://application:,,,/MyApp;component/Themes/Corporate.xaml"));

// 使用 ResourceDictionary
var dict = new ResourceDictionary();
dict["Sidebar.Background"] = new SolidColorBrush(Colors.Navy);
// ... 设置其他 Key
ThemeManager.RegisterTheme("Navy", dict);

ThemeManager.ApplyTheme("Corporate");
```

### 主题颜色 Key 一览

| Key | 用途 |
|-----|------|
| `Sidebar.Background` | 侧边栏背景 |
| `Sidebar.BorderBrush` | 边框 |
| `Sidebar.AccentBrush` | 主色调 |
| `Sidebar.AccentLightBrush` | 主色调浅色 |
| `Sidebar.ForegroundBrush` | 主文字 |
| `Sidebar.SecondaryForeground` | 次要文字 |
| `Sidebar.IconBrush` | 图标默认色 |
| `Sidebar.IconActiveBrush` | 图标激活色 |
| `Sidebar.Item.HoverBrush` | 悬停背景 |
| `Sidebar.Item.SelectedBrush` | 选中背景 |
| `Sidebar.IndicatorBrush` | 左侧指示条 |
| `Sidebar.Badge.Background` | 数字/红点徽章背景 |
| `Sidebar.Badge.Foreground` | 数字徽章文字 |
| `Sidebar.Badge.TextBg` | 文本徽章背景 |
| `Sidebar.Badge.TextFg` | 文本徽章文字 |
| `Sidebar.Search.Background` | 搜索框背景 |
| `Sidebar.Search.FocusBorder` | 搜索框聚焦边框 |
| `Sidebar.SeparatorBrush` | 分隔线 |
| `Sidebar.GroupForeground` | 分组标题 |
| `Sidebar.SearchHighlightFg` | 搜索匹配高亮色 |
| `Sidebar.ExpandArrowBrush` | 展开箭头 |
| `Sidebar.ScrollBarThumb` | 滚动条滑块 |

---

## ⌨️ 键盘导航

| 按键 | 操作 |
|------|------|
| `↑` | 上移选中 |
| `↓` | 下移选中 |
| `→` | 展开当前项子级 |
| `←` | 折叠当前项 / 跳到父级 |
| `Enter` / `Space` | 确认选中 / 展开 |

控件获取焦点后自动支持键盘导航。

---

## 📋 API 参考

### SidebarNavigation 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `ViewModel` | `SidebarViewModel` | 绑定的主 ViewModel |
| `IsExpanded` | `bool` | 展开/迷你模式 |
| `ExpandedWidth` | `double` | 展开宽度（默认 240） |
| `MiniWidth` | `double` | 迷你宽度（默认 60） |
| `AnimationDuration` | `Duration` | 动画时长 |
| `ShowSearchBox` | `bool` | 是否显示搜索框 |
| `ShowBackButton` | `bool` | 是否显示后退按钮 |
| `HeaderContent` | `object` | Header 插槽内容 |
| `FooterContent` | `object` | Footer 插槽内容 |
| `CornerRadius` | `CornerRadius` | 圆角 |

### SidebarViewModel 属性

| 属性 | 说明 |
|------|------|
| `Groups` | 分组集合 |
| `SelectedItem` | 当前选中项 |
| `IsExpanded` | 展开状态 |
| `SearchText` | 搜索关键字 |
| `CurrentTheme` | 当前主题名 |
| `ToggleExpandCommand` | 切换展开命令 |
| `GoBackCommand` | 后退命令 |
| `SelectItemCommand` | 选中项命令 |
| `ClearSearchCommand` | 清除搜索命令 |
| `SwitchThemeCommand` | 切换主题命令 |

### SidebarNavigation 事件

| 事件 | 触发时机 | 事件参数 |
|------|----------|----------|
| `ItemSelected` | `SidebarViewModel.SelectedItem` 发生变化后触发（点击导航项、键盘确认、执行 GoBack）。仅在 `ViewModel` 绑定到 `SidebarNavigation` 时自动转发。 | `SidebarItemSelectedEventArgs`，可通过 `e.SelectedItem` 拿到当前选中项。 |
| `ModeChanged` | `IsExpanded` 发生变化并启动宽度动画时触发。 | `RoutedEventArgs` |

事件使用示例：

```xml
<sidebar:SidebarNavigation
    ViewModel="{Binding SidebarVM}"
    ItemSelected="SidebarNavigation_OnItemSelected" />
```

```csharp
private void SidebarNavigation_OnItemSelected(object sender, SidebarItemSelectedEventArgs e)
{
    var selected = e.SelectedItem;
    if (selected == null) return;

    // 在这里执行页面切换 / 埋点 / 权限判断
}
```

### SidebarItemViewModel 属性

| 属性 | 说明 |
|------|------|
| `Id` | 唯一标识 |
| `Title` | 显示名称 |
| `ToolTip` | 提示文字 |
| `IconType` | 图标类型 |
| `IconPathData` / `IconPathDataString` | Path 矢量图标 |
| `FontIconGlyph` / `FontIconFamily` | 字体图标 |
| `IconImageSource` | 图片图标 |
| `BadgeType` | 徽章类型 |
| `BadgeCount` | 数字徽章 |
| `BadgeText` | 文本徽章 |
| `Children` | 子项集合（多级嵌套） |
| `IsSelected` | 选中状态 |
| `IsExpanded` | 展开状态 |
| `IsEnabled` | 启用/禁用 |
| `Level` | 层级深度 |
| `Tag` | 附加数据 |

---

## 🛠 编译运行

1. 用 **VS2019** 打开 `SidebarNavSolution.sln`
2. 确保目标框架为 **.NET Framework 4.7.2**
3. 将 `SidebarNavDemo` 设为启动项目
4. **F5** 运行

---

## 📝 扩展建议

- **路由导航**：在 `SelectedItemChanged` 事件中配合 Frame/Page 实现页面切换
- **拼音搜索**：扩展 `SidebarSearchService.MatchItem()` 添加拼音首字母匹配
- **拖拽排序**：扩展 `SidebarNavItem` 添加 DragDrop 支持
- **动画增强**：子项展开可添加 SlideDown 动画
- **右键菜单**：通过 `ContextMenu` 在项上添加快捷操作
- **收藏/置顶**：在 ViewModel 添加 `IsPinned` 属性
