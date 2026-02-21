using ArcMapControl.Models;

namespace ArcMapControl.Layers
{
    public abstract class MapLayerBase : IMapLayer
    {
        protected MapLayerBase(string name)
        {
            Name = name;
            IsVisible = true;
        }

        public string Name { get; private set; }
        public bool IsVisible { get; set; }
        public int ZIndex { get; set; }

        public abstract MapEnvelope GetEnvelope();

        public abstract void Render(System.Windows.Media.DrawingContext drawingContext, LayerRenderContext context);
    }
}
