using System.Windows.Media;

namespace ArcMapControl.Configuration
{
    public sealed class MapDisplayOptions
    {
        public Brush Background { get; set; }
        public Brush VectorStroke { get; set; }
        public Brush VectorFill { get; set; }
        public double StrokeThickness { get; set; }
        public int MaxTileConcurrency { get; set; }

        public static MapDisplayOptions CreateDefault()
        {
            return new MapDisplayOptions
            {
                Background = Brushes.White,
                VectorStroke = Brushes.DarkSlateGray,
                VectorFill = Brushes.LightBlue,
                StrokeThickness = 1.0,
                MaxTileConcurrency = 4
            };
        }
    }
}
