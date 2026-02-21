using System;
using ArcMapControl.Models;

namespace ArcMapControl.Projection
{
    public sealed class ProjectionEngine
    {
        public MapPoint Project(MapPoint input, GeodeticCorrectionParameters parameters)
        {
            if (parameters == null) return input;

            var x = input.X;
            var y = input.Y;

            if (parameters.Mode == CorrectionMode.Geographic)
            {
                return new MapPoint(
                    (x + parameters.FalseEasting) * parameters.ScaleFactor,
                    (y + parameters.FalseNorthing) * parameters.ScaleFactor);
            }

            var zoneWidth = parameters.Mode == CorrectionMode.ThreeDegree ? 3.0 : 6.0;
            var central = parameters.CentralMeridian;
            if (Math.Abs(central) < 0.0000001)
            {
                central = Math.Round(x / zoneWidth) * zoneWidth;
            }

            var radius = parameters.Ellipsoid?.SemiMajorAxis ?? ReferenceEllipsoid.Cgcs2000.SemiMajorAxis;
            var lon = DegreesToRadians(x);
            var lat = DegreesToRadians(y);
            var centralRad = DegreesToRadians(central);

            var delta = lon - centralRad;
            var projectedX = radius * delta * Math.Cos(lat);
            var projectedY = radius * lat;

            projectedX = (projectedX + parameters.FalseEasting) * parameters.ScaleFactor;
            projectedY = (projectedY + parameters.FalseNorthing) * parameters.ScaleFactor;

            return new MapPoint(projectedX, projectedY);
        }

        private static double DegreesToRadians(double value)
        {
            return value * Math.PI / 180.0;
        }
    }
}
