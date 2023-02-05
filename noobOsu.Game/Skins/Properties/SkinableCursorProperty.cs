using osuTK;
using noobOsu.Game.Util;
using noobOsu.Game.Skins.Drawables;
using osu.Framework.Graphics.Textures;
using Logger = osu.Framework.Logging.Logger;

namespace noobOsu.Game.Skins.Properties
{
    public class SkinnableCursorProperty : SkinnableProperty
    {
        private SkinnableSprite Drawable;
        public bool Scaleable = true;

        public SkinnableCursorProperty(SkinnableSprite drawable, string name)
        {
            PropertyType = ISkinnableProperty.Type.Texture;
            Drawable = drawable;
            Name = name;
        }

        public override void SetScale(float scale)
        {
            Scale = scale;
            
            Drawable.ScaleFactor = Scale;
        }

        public override void Resolve(object obj)
        {
            if (Resolved) return;
            base.Resolve(obj);

            Drawable.Texture = (Texture)obj;

            if (Scale > 1)
            {
                if (Scaleable)
                    Drawable.Scale *= new Vector2(TextureUtil.GetScaleFor(Drawable.Texture)) * (1/Scale);
                Drawable.ScaleFactor = TextureUtil.GetScaleFor(Drawable.Texture) * (1/Scale);
            }
            else
            {
                if (Scaleable)
                    Drawable.Scale *= new Vector2(TextureUtil.GetScaleFor(Drawable.Texture));
                Drawable.ScaleFactor = TextureUtil.GetScaleFor(Drawable.Texture);
            }
        }
    }
}