namespace ArcMapControl.Projection
{
    public sealed class GeodeticCorrectionParameters
    {
        public CorrectionMode Mode { get; set; }
        public double CentralMeridian { get; set; }
        public double FalseEasting { get; set; }
        public double FalseNorthing { get; set; }
        public double ScaleFactor { get; set; }
        public ReferenceEllipsoid Ellipsoid { get; set; }

        public static GeodeticCorrectionParameters CreateDefault()
        {
            return new GeodeticCorrectionParameters
            {
                Mode = CorrectionMode.Geographic,
                CentralMeridian = 0,
                FalseEasting = 0,
                FalseNorthing = 0,
                ScaleFactor = 1,
                Ellipsoid = ReferenceEllipsoid.Cgcs2000
            };
        }
    }
}
