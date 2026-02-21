namespace ArcMapControl.Models
{
    public struct MapEnvelope
    {
        public MapEnvelope(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public double MinX { get; }
        public double MinY { get; }
        public double MaxX { get; }
        public double MaxY { get; }

        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public static MapEnvelope Union(MapEnvelope left, MapEnvelope right)
        {
            if (left.IsEmpty) return right;
            if (right.IsEmpty) return left;
            return new MapEnvelope(
                left.MinX < right.MinX ? left.MinX : right.MinX,
                left.MinY < right.MinY ? left.MinY : right.MinY,
                left.MaxX > right.MaxX ? left.MaxX : right.MaxX,
                left.MaxY > right.MaxY ? left.MaxY : right.MaxY);
        }
    }
}
