using System;
using System.Globalization;
using System.IO;
using System.Windows;
using ArcMapControl.Layers;
using ArcMapControl.Projection;

namespace ArcMapControlDemo
{
    public partial class MainWindow : Window
    {
        private TileLayer _osmLayer;
        private TiandituTileLayer _tiandituLayer;
        private ShapefileLayer _shapefileLayer;
        private DemoUserLibraryLayer _userLayer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUiOptions();
            InitializeMap();
        }

        private void InitializeUiOptions()
        {
            ModeComboBox.ItemsSource = Enum.GetValues(typeof(CorrectionMode));
            ModeComboBox.SelectedItem = CorrectionMode.SixDegree;

            EllipsoidComboBox.Items.Add("CGCS2000");
            EllipsoidComboBox.Items.Add("WGS84");
            EllipsoidComboBox.SelectedIndex = 0;
        }

        private void InitializeMap()
        {
            _osmLayer = new TileLayer("OpenStreetMap", "https://tile.openstreetmap.org/{z}/{x}/{y}.png") { ZIndex = 0, CurrentZoom = 5 };
            _tiandituLayer = new TiandituTileLayer("天地图矢量", TiandituTokenTextBox.Text, TiandituStyleTextBox.Text) { ZIndex = 1, CurrentZoom = 5, IsVisible = false };
            _userLayer = new DemoUserLibraryLayer("企业用户库") { ZIndex = 20 };

            MapView.Layers.Add(_osmLayer);
            MapView.Layers.Add(_tiandituLayer);
            MapView.Layers.Add(_userLayer);

            var shpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample.shp");
            if (File.Exists(shpPath))
            {
                _shapefileLayer = new ShapefileLayer("SHP图层", shpPath) { ZIndex = 10 };
                MapView.Layers.Add(_shapefileLayer);
            }
            else
            {
                ShpLayerCheckBox.IsEnabled = false;
                ShpLayerCheckBox.Content = "SHP图层（未找到 Data/sample.shp）";
            }

            ApplyCorrectionFromUi();
            MapView.ZoomToFullExtent();
        }

        private void OnFullExtentClick(object sender, RoutedEventArgs e)
        {
            MapView.ZoomToFullExtent();
        }

        private void OnApplyCorrectionClick(object sender, RoutedEventArgs e)
        {
            ApplyCorrectionFromUi();
        }

        private void OnLayerVisibilityChanged(object sender, RoutedEventArgs e)
        {
            if (_osmLayer != null) _osmLayer.IsVisible = OsmLayerCheckBox.IsChecked == true;
            if (_tiandituLayer != null) _tiandituLayer.IsVisible = TiandituLayerCheckBox.IsChecked == true;
            if (_userLayer != null) _userLayer.IsVisible = UserLayerCheckBox.IsChecked == true;
            if (_shapefileLayer != null) _shapefileLayer.IsVisible = ShpLayerCheckBox.IsChecked == true;

            MapView.InvalidateVisual();
        }

        private void OnUpdateTiandituClick(object sender, RoutedEventArgs e)
        {
            if (_tiandituLayer != null)
            {
                MapView.Layers.Remove(_tiandituLayer);
            }

            _tiandituLayer = new TiandituTileLayer("天地图矢量", TiandituTokenTextBox.Text, TiandituStyleTextBox.Text)
            {
                ZIndex = 1,
                CurrentZoom = 5,
                IsVisible = TiandituLayerCheckBox.IsChecked == true
            };

            MapView.Layers.Add(_tiandituLayer);
            MapView.InvalidateVisual();
        }

        private void ApplyCorrectionFromUi()
        {
            double centralMeridian;
            double scale;
            double falseEasting;
            double falseNorthing;

            if (!double.TryParse(CentralMeridianTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out centralMeridian)) centralMeridian = 117;
            if (!double.TryParse(ScaleFactorTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out scale)) scale = 1;
            if (!double.TryParse(FalseEastingTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out falseEasting)) falseEasting = 0;
            if (!double.TryParse(FalseNorthingTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out falseNorthing)) falseNorthing = 0;

            var selectedMode = (CorrectionMode)(ModeComboBox.SelectedItem ?? CorrectionMode.Geographic);
            var selectedEllipsoid = (EllipsoidComboBox.SelectedItem as string) == "WGS84"
                ? ReferenceEllipsoid.Wgs84
                : ReferenceEllipsoid.Cgcs2000;

            MapView.CorrectionParameters.Mode = selectedMode;
            MapView.CorrectionParameters.CentralMeridian = centralMeridian;
            MapView.CorrectionParameters.ScaleFactor = scale;
            MapView.CorrectionParameters.FalseEasting = falseEasting;
            MapView.CorrectionParameters.FalseNorthing = falseNorthing;
            MapView.CorrectionParameters.Ellipsoid = selectedEllipsoid;

            MapView.InvalidateVisual();
        }
    }
}
