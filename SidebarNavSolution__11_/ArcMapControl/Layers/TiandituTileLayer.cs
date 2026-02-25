namespace ArcMapControl.Layers
{
    public sealed class TiandituTileLayer : TileLayer
    {
        public TiandituTileLayer(string name, string token, string style) : base(name, BuildTemplate(token, style))
        {
        }

        private static string BuildTemplate(string token, string style)
        {
            var layerStyle = string.IsNullOrWhiteSpace(style) ? "vec_w" : style;
            return "https://t0.tianditu.gov.cn/" + layerStyle + "/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER="
                   + layerStyle + "&STYLE=default&TILEMATRIXSET=w&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}&FORMAT=tiles&tk=" + token;
        }
    }
}
