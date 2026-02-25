namespace ArcMapControl.Models
{
    public struct MapPoint
    {
        public MapPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }
    }
}
