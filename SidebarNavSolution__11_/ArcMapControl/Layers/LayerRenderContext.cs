using ArcMapControl.Configuration;
using ArcMapControl.Projection;
using ArcMapControl.Rendering;

namespace ArcMapControl.Layers
{
    public sealed class LayerRenderContext
    {
        public LayerRenderContext(MapViewport viewport, ProjectionEngine projectionEngine, GeodeticCorrectionParameters correctionParameters, MapDisplayOptions displayOptions)
        {
            Viewport = viewport;
            ProjectionEngine = projectionEngine;
            CorrectionParameters = correctionParameters;
            DisplayOptions = displayOptions;
        }

        public MapViewport Viewport { get; }
        public ProjectionEngine ProjectionEngine { get; }
        public GeodeticCorrectionParameters CorrectionParameters { get; }
        public MapDisplayOptions DisplayOptions { get; }
    }
}
