using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using ArcMapControl.Models;

namespace ArcMapControl.Layers
{
    public sealed class ShapefileLayer : MapLayerBase
    {
        private readonly List<List<MapPoint>> _parts = new List<List<MapPoint>>();
        private MapEnvelope _envelope;

        public ShapefileLayer(string name, string shpPath) : base(name)
        {
            if (string.IsNullOrWhiteSpace(shpPath)) throw new ArgumentNullException(nameof(shpPath));
            LoadFromShp(shpPath);
        }

        public override MapEnvelope GetEnvelope()
        {
            return _envelope;
        }

        public override void Render(DrawingContext drawingContext, LayerRenderContext context)
        {
            var pen = new Pen(context.DisplayOptions.VectorStroke, context.DisplayOptions.StrokeThickness);

            foreach (var part in _parts)
            {
                if (part.Count < 2) continue;

                var geometry = new StreamGeometry();
                using (var geometryContext = geometry.Open())
                {
                    var start = context.Viewport.ToScreen(context.ProjectionEngine.Project(part[0], context.CorrectionParameters));
                    geometryContext.BeginFigure(start, false, false);
                    for (var i = 1; i < part.Count; i++)
                    {
                        var screenPoint = context.Viewport.ToScreen(context.ProjectionEngine.Project(part[i], context.CorrectionParameters));
                        geometryContext.LineTo(screenPoint, true, false);
                    }
                }

                geometry.Freeze();
                drawingContext.DrawGeometry(null, pen, geometry);
            }
        }

        private void LoadFromShp(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var reader = new BinaryReader(stream))
            {
                stream.Seek(24, SeekOrigin.Begin);
                ReadBigEndianInt32(reader);
                var version = reader.ReadInt32();
                var shapeType = reader.ReadInt32();
                var minX = reader.ReadDouble();
                var minY = reader.ReadDouble();
                var maxX = reader.ReadDouble();
                var maxY = reader.ReadDouble();
                _envelope = new MapEnvelope(minX, minY, maxX, maxY);

                stream.Seek(100, SeekOrigin.Begin);
                while (stream.Position < stream.Length)
                {
                    if (stream.Length - stream.Position < 8) break;
                    ReadBigEndianInt32(reader);
                    var contentLengthWords = ReadBigEndianInt32(reader);
                    var recordStart = stream.Position;
                    if (contentLengthWords <= 0) break;

                    var recordShapeType = reader.ReadInt32();
                    if (recordShapeType == 3 || recordShapeType == 5)
                    {
                        ReadPolylineOrPolygon(reader);
                    }
                    else if (recordShapeType == 1)
                    {
                        var x = reader.ReadDouble();
                        var y = reader.ReadDouble();
                        _parts.Add(new List<MapPoint> { new MapPoint(x, y), new MapPoint(x + 0.00001, y + 0.00001) });
                    }

                    var target = recordStart + (contentLengthWords * 2L);
                    if (stream.Position < target)
                    {
                        stream.Seek(target, SeekOrigin.Begin);
                    }
                }
            }
        }

        private void ReadPolylineOrPolygon(BinaryReader reader)
        {
            reader.ReadDouble();
            reader.ReadDouble();
            reader.ReadDouble();
            reader.ReadDouble();

            var numParts = reader.ReadInt32();
            var numPoints = reader.ReadInt32();
            var partIndexes = new int[numParts];
            for (var i = 0; i < numParts; i++)
            {
                partIndexes[i] = reader.ReadInt32();
            }

            var points = new MapPoint[numPoints];
            for (var i = 0; i < numPoints; i++)
            {
                points[i] = new MapPoint(reader.ReadDouble(), reader.ReadDouble());
            }

            for (var i = 0; i < numParts; i++)
            {
                var start = partIndexes[i];
                var end = (i == numParts - 1 ? numPoints : partIndexes[i + 1]);
                var part = new List<MapPoint>();
                for (var p = start; p < end; p++)
                {
                    part.Add(points[p]);
                }

                _parts.Add(part);
            }
        }

        private static int ReadBigEndianInt32(BinaryReader reader)
        {
            var bytes = reader.ReadBytes(4);
            if (bytes.Length < 4) return 0;
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
