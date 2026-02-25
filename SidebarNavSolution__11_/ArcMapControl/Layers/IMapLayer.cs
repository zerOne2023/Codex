using System.Windows.Media;
using ArcMapControl.Models;

namespace ArcMapControl.Layers
{
    public interface IMapLayer
    {
        string Name { get; }
        bool IsVisible { get; set; }
        int ZIndex { get; set; }
        MapEnvelope GetEnvelope();
        void Render(DrawingContext drawingContext, LayerRenderContext context);
    }
}
