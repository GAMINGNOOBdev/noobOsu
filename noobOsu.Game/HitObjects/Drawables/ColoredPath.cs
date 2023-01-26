using osuTK.Graphics;
using noobOsu.Game.Util;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Lines;
using osu.Framework.Extensions.Color4Extensions;

namespace noobOsu.Game.HitObjects.Drawables
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

        protected float CalculatedBorder => BorderSize * BORDER * 0.77f;

        protected override Color4 ColourAt(float position)
        {
            if (CalculatedBorder != 0f && position <= CalculatedBorder)
                return BorderColor.Value;
            
            position -= CalculatedBorder;
            Color4 accent = AccentColor.Value;

            Color4 color = ColorUtil.Interpolate(accent.Darken(0.1f), ColorUtil.Lighten(accent, 0.5f), position / GRADIENT);
            color.A = 0.5f; // always has to be somewhat transparent
            return color;
        }
    }
}