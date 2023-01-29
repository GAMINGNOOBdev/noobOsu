using osuTK;
using noobOsu.Game.Util;
using noobOsu.Game.Skins;
using osu.Framework.Graphics.Textures;
using Logger = osu.Framework.Logging.Logger;

namespace noobOsu.Game.HitObjects.Drawables
{
    public class SkinnableTextureProperty : SkinnableProperty
    {
        private HitObjectSprite Drawable;
        private bool AutoCrop;

        public SkinnableTextureProperty(HitObjectSprite drawable, string name, bool CropIfNeeded = false)
        {
            PropertyType = ISkinnableProperty.Type.Texture;
            AutoCrop = CropIfNeeded;
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
                if (AutoCrop)
                    Drawable.Texture = TextureUtil.CropHitCircleIfNeeded(Drawable.Texture, Scale);
            
                Drawable.Scale *= new Vector2(TextureUtil.GetScaleFor(Drawable.Texture)) * (1/Scale);
                Drawable.ScaleFactor = TextureUtil.GetScaleFor(Drawable.Texture) * (1/Scale);
            }
            else
            {
                if (AutoCrop)
                    Drawable.Texture = TextureUtil.CropHitCircleIfNeeded(Drawable.Texture);
            
                Drawable.Scale *= new Vector2(TextureUtil.GetScaleFor(Drawable.Texture));
                Drawable.ScaleFactor = TextureUtil.GetScaleFor(Drawable.Texture);
            }
        }
    }
}