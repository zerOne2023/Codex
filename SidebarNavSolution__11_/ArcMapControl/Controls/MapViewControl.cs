using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArcMapControl.Configuration;
using ArcMapControl.Layers;
using ArcMapControl.Models;
using ArcMapControl.Projection;
using ArcMapControl.Rendering;

namespace ArcMapControl.Controls
{
    public class MapViewControl : Control
    {
        private Point _dragStart;
        private bool _dragging;

        static MapViewControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapViewControl), new FrameworkPropertyMetadata(typeof(MapViewControl)));
        }

        public MapViewControl()
        {
            Layers = new ObservableCollection<IMapLayer>();
            Layers.CollectionChanged += OnLayersCollectionChanged;
            DisplayOptions = MapDisplayOptions.CreateDefault();
            CorrectionParameters = GeodeticCorrectionParameters.CreateDefault();
            ProjectionEngine = new ProjectionEngine();
            Extent = new MapEnvelope(100, 20, 125, 45);
            Focusable = true;
        }

        public ObservableCollection<IMapLayer> Layers { get; private set; }

        public MapDisplayOptions DisplayOptions { get; set; }

        public GeodeticCorrectionParameters CorrectionParameters { get; set; }

        public ProjectionEngine ProjectionEngine { get; private set; }

        public MapEnvelope Extent
        {
            get { return (MapEnvelope)GetValue(ExtentProperty); }
            set { SetValue(ExtentProperty, value); }
        }

        public static readonly DependencyProperty ExtentProperty = DependencyProperty.Register(
            "Extent", typeof(MapEnvelope), typeof(MapViewControl), new FrameworkPropertyMetadata(default(MapEnvelope), FrameworkPropertyMetadataOptions.AffectsRender));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(DisplayOptions.Background, null, new Rect(new Point(0, 0), RenderSize));

            var viewport = new MapViewport(RenderSize, Extent);
            var context = new LayerRenderContext(viewport, ProjectionEngine, CorrectionParameters, DisplayOptions);

            foreach (var layer in Layers.Where(l => l.IsVisible).OrderBy(l => l.ZIndex))
            {
                layer.Render(drawingContext, context);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var factor = e.Delta > 0 ? 0.85 : 1.15;
            var centerX = (Extent.MinX + Extent.MaxX) * 0.5;
            var centerY = (Extent.MinY + Extent.MaxY) * 0.5;
            var width = Extent.Width * factor;
            var height = Extent.Height * factor;
            Extent = new MapEnvelope(centerX - width / 2.0, centerY - height / 2.0, centerX + width / 2.0, centerY + height / 2.0);
            InvalidateVisual();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _dragStart = e.GetPosition(this);
            _dragging = true;
            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _dragging = false;
            ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_dragging) return;

            var current = e.GetPosition(this);
            var dx = current.X - _dragStart.X;
            var dy = current.Y - _dragStart.Y;

            if (RenderSize.Width <= 0 || RenderSize.Height <= 0) return;

            var mapDx = dx / RenderSize.Width * Extent.Width;
            var mapDy = dy / RenderSize.Height * Extent.Height;
            Extent = new MapEnvelope(Extent.MinX - mapDx, Extent.MinY + mapDy, Extent.MaxX - mapDx, Extent.MaxY + mapDy);
            _dragStart = current;
            InvalidateVisual();
        }

        public void ZoomToFullExtent()
        {
            var envelope = default(MapEnvelope);
            var hasValue = false;
            foreach (var layer in Layers)
            {
                var layerExtent = layer.GetEnvelope();
                if (layerExtent.IsEmpty) continue;
                envelope = hasValue ? MapEnvelope.Union(envelope, layerExtent) : layerExtent;
                hasValue = true;
            }

            if (hasValue)
            {
                Extent = envelope;
                InvalidateVisual();
            }
        }

        private void OnLayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
        }
    }
}
