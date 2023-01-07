using osuTK.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Lines;

namespace noobOsu.Game.HitObjects
{
    public partial class ColoredPath : SmoothPath
    {
        public const float BORDER = 0.128f;
        public const float GRADIENT = 1 - BORDER;

        private const float border_max = 8f;
        private const float border_min = 0f;
        private const float opacity_centre = 0.9f;
        private const float opacity_edge = 0.8f;

        private float borderSize = 1f;

        public Bindable<Color4> BorderColor;

        public Bindable<Color4> AccentColor;

        public ColoredPath() : base()
        {
            BorderColor = new Bindable<Color4>(Color4.White);
            AccentColor = new Bindable<Color4>(Color4.White);

            BorderColor.BindValueChanged((val) => {
                InvalidateTexture();
            });
            AccentColor.BindValueChanged((val) => {
                InvalidateTexture();
            });
        }

        public float BorderSize
        {
            get => borderSize;
            set {
                if (borderSize == value) return;
                if (border_min > value || border_max < value) return;

                borderSize = value;

                InvalidateTexture();
            }
        }

        protected float CalculatedBorder => BorderSize * BORDER;

        protected override Color4 ColourAt(float position)
        {
            if (CalculatedBorder != 0f && position <= CalculatedBorder)
                return BorderColor.Value;
            
            position -= CalculatedBorder;
            return new Color4( AccentColor.Value.R, AccentColor.Value.G, AccentColor.Value.B, (opacity_edge - (opacity_edge - opacity_centre) * position / GRADIENT) * AccentColor.Value.A );
        }
    }
}