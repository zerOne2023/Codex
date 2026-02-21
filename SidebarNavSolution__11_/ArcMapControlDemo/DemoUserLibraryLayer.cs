using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ArcMapControl.Layers;
using ArcMapControl.Models;

namespace ArcMapControlDemo
{
    public sealed class DemoUserLibraryLayer : MapLayerBase
    {
        private readonly List<MapPoint> _points;

        public DemoUserLibraryLayer(string name) : base(name)
        {
            _points = new List<MapPoint>
            {
                new MapPoint(116.4074, 39.9042), // 北京
                new MapPoint(121.4737, 31.2304), // 上海
                new MapPoint(113.2644, 23.1291), // 广州
                new MapPoint(104.0665, 30.5728), // 成都
                new MapPoint(114.3055, 30.5928)  // 武汉
            };
        }

        public override MapEnvelope GetEnvelope()
        {
            return new MapEnvelope(103, 22, 122, 41);
        }

        public override void Render(DrawingContext drawingContext, LayerRenderContext context)
        {
            var markerBrush = Brushes.OrangeRed;
            var borderPen = new Pen(Brushes.White, 1);
            var textBrush = Brushes.Black;

            foreach (var point in _points)
            {
                var projected = context.ProjectionEngine.Project(point, context.CorrectionParameters);
                var screen = context.Viewport.ToScreen(projected);
                drawingContext.DrawEllipse(markerBrush, borderPen, screen, 5, 5);
                drawingContext.DrawRectangle(Brushes.WhiteSmoke, new Pen(Brushes.Gray, 0.8), new Rect(screen.X + 8, screen.Y - 10, 62, 16));
                var text = new FormattedText(
                    "企业站点",
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Microsoft YaHei"),
                    10,
                    textBrush,
                    1.0);
                drawingContext.DrawText(text, new Point(screen.X + 12, screen.Y - 8));
            }
        }
    }
}
