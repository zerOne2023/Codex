using System;
using System.IO;
using System.Windows;
using ArcMapControl.Layers;
using ArcMapControl.Projection;

namespace ArcMapControlDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {
            MapView.CorrectionParameters.Mode = CorrectionMode.SixDegree;
            MapView.CorrectionParameters.CentralMeridian = 117;
            MapView.CorrectionParameters.ScaleFactor = 1;

            MapView.Layers.Add(new TileLayer("OpenStreetMap", "https://tile.openstreetmap.org/{z}/{x}/{y}.png") { ZIndex = 0, CurrentZoom = 6 });
            MapView.Layers.Add(new TiandituTileLayer("天地图矢量", "请替换你的天地图token", "vec_w") { ZIndex = 1, CurrentZoom = 6 });

            var shpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample.shp");
            if (File.Exists(shpPath))
            {
                MapView.Layers.Add(new ShapefileLayer("SHP图层", shpPath) { ZIndex = 10 });
            }
        }

        private void OnFullExtentClick(object sender, RoutedEventArgs e)
        {
            MapView.ZoomToFullExtent();
        }
    }
}
