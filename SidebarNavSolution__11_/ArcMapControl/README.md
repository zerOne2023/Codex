# ArcMapControl（VS2019 / WPF / .NET Framework 4.7.2）

## 目标
- 模仿 ArcGIS 地图显示窗口的交互与层级结构。
- 尽量 0 第三方依赖，仅使用 .NET Framework / WPF 自带能力。

## 核心能力
- `MapViewControl`：支持平移、滚轮缩放、全图。
- SHP 图层：内置基础 `.shp`（点/线/面）读取与渲染（不依赖 GDAL）。
- 叠瓦图层：支持标准 XYZ 模板。
- 天地图挂接：`TiandituTileLayer`，只需传入 token 与样式。
- 动校正参数：椭球体、三度带、六度带、经纬度模式参数可实时调整。

## 大型项目分层建议
- `Controls/`：UI 控件入口，稳定对外 API。
- `Layers/`：图层抽象与实现（SHP/瓦片/用户扩展图层）。
- `Projection/`：坐标校正、椭球参数、投影策略。
- `Rendering/`：视口与坐标转屏幕逻辑。
- `Configuration/`：参数配置对象。
- `Models/`：基础数据结构。

## 二次扩展建议
- 在 `IMapLayer` 基础上扩展企业用户库图层（数据库、服务接口）。
- 如果需要高精度投影，可在 `ProjectionEngine` 中替换为更完整高斯克吕格算法。
- 如果需要出图模板，可在宿主程序叠加标题栏、比例尺、指北针与打印布局。
