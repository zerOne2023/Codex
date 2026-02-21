using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArcMapControl.Models;

namespace ArcMapControl.Layers
{
    public class TileLayer : MapLayerBase
    {
        private readonly Dictionary<string, BitmapImage> _cache = new Dictionary<string, BitmapImage>();

        public TileLayer(string name, string urlTemplate) : base(name)
        {
            UrlTemplate = urlTemplate;
            MinZoom = 1;
            MaxZoom = 18;
        }

        public string UrlTemplate { get; set; }
        public int MinZoom { get; set; }
        public int MaxZoom { get; set; }
        public int CurrentZoom { get; set; }

        public override MapEnvelope GetEnvelope()
        {
            return new MapEnvelope(-180, -85, 180, 85);
        }

        public override void Render(DrawingContext drawingContext, LayerRenderContext context)
        {
            if (string.IsNullOrWhiteSpace(UrlTemplate)) return;
            var zoom = CurrentZoom <= 0 ? CalculateZoom(context.Viewport.Extent.Width, context.Viewport.RenderSize.Width) : CurrentZoom;
            if (zoom < MinZoom) zoom = MinZoom;
            if (zoom > MaxZoom) zoom = MaxZoom;

            var minX = LonToTileX(context.Viewport.Extent.MinX, zoom);
            var maxX = LonToTileX(context.Viewport.Extent.MaxX, zoom);
            var minY = LatToTileY(context.Viewport.Extent.MaxY, zoom);
            var maxY = LatToTileY(context.Viewport.Extent.MinY, zoom);

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    var tile = LoadTile(zoom, x, y);
                    if (tile == null) continue;

                    var lonLeft = TileXToLon(x, zoom);
                    var lonRight = TileXToLon(x + 1, zoom);
                    var latTop = TileYToLat(y, zoom);
                    var latBottom = TileYToLat(y + 1, zoom);

                    var topLeft = context.Viewport.ToScreen(new MapPoint(lonLeft, latTop));
                    var bottomRight = context.Viewport.ToScreen(new MapPoint(lonRight, latBottom));
                    var rect = new Rect(topLeft, bottomRight);
                    drawingContext.DrawImage(tile, rect);
                }
            }
        }

        protected virtual BitmapImage LoadTile(int z, int x, int y)
        {
            var key = z.ToString(CultureInfo.InvariantCulture) + "_" + x.ToString(CultureInfo.InvariantCulture) + "_" + y.ToString(CultureInfo.InvariantCulture);
            BitmapImage cached;
            if (_cache.TryGetValue(key, out cached)) return cached;

            var url = UrlTemplate.Replace("{z}", z.ToString(CultureInfo.InvariantCulture))
                .Replace("{x}", x.ToString(CultureInfo.InvariantCulture))
                .Replace("{y}", y.ToString(CultureInfo.InvariantCulture));

            try
            {
                var request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null) return null;
                    var memory = new MemoryStream();
                    stream.CopyTo(memory);
                    memory.Position = 0;

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = memory;
                    image.EndInit();
                    image.Freeze();
                    _cache[key] = image;
                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        private static int CalculateZoom(double mapWidth, double pixelWidth)
        {
            if (mapWidth <= 0 || pixelWidth <= 0) return 2;
            var scale = 360.0 / mapWidth;
            var zoom = Math.Log(scale * pixelWidth / 256.0, 2);
            return (int)Math.Round(zoom);
        }

        private static int LonToTileX(double lon, int z) => (int)Math.Floor((lon + 180.0) / 360.0 * (1 << z));
        private static int LatToTileY(double lat, int z)
        {
            var rad = lat * Math.PI / 180.0;
            return (int)Math.Floor((1.0 - Math.Log(Math.Tan(rad) + 1.0 / Math.Cos(rad)) / Math.PI) / 2.0 * (1 << z));
        }
        private static double TileXToLon(int x, int z) => x / (double)(1 << z) * 360.0 - 180;
        private static double TileYToLat(int y, int z)
        {
            var n = Math.PI - 2.0 * Math.PI * y / (1 << z);
            return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
        }
    }
}
