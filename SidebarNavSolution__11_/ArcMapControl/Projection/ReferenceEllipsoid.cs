namespace ArcMapControl.Projection
{
    public sealed class ReferenceEllipsoid
    {
        public static ReferenceEllipsoid Wgs84 => new ReferenceEllipsoid("WGS84", 6378137.0, 298.257223563);
        public static ReferenceEllipsoid Cgcs2000 => new ReferenceEllipsoid("CGCS2000", 6378137.0, 298.257222101);

        public ReferenceEllipsoid(string name, double semiMajorAxis, double inverseFlattening)
        {
            Name = name;
            SemiMajorAxis = semiMajorAxis;
            InverseFlattening = inverseFlattening;
        }

        public string Name { get; }
        public double SemiMajorAxis { get; }
        public double InverseFlattening { get; }
    }
}
