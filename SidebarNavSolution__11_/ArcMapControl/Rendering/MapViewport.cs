using System.Windows;
using ArcMapControl.Models;

namespace ArcMapControl.Rendering
{
    public sealed class MapViewport
    {
        public MapViewport(Size renderSize, MapEnvelope extent)
        {
            RenderSize = renderSize;
            Extent = extent;
        }

        public Size RenderSize { get; }
        public MapEnvelope Extent { get; }

        public Point ToScreen(MapPoint point)
        {
            if (Extent.IsEmpty || RenderSize.Width <= 0 || RenderSize.Height <= 0)
            {
                return new Point();
            }

            var x = (point.X - Extent.MinX) / Extent.Width * RenderSize.Width;
            var y = RenderSize.Height - ((point.Y - Extent.MinY) / Extent.Height * RenderSize.Height);
            return new Point(x, y);
        }
    }
}
