using osuTK;
using noobOsu.Game.Util;
using noobOsu.Game.Skins;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace noobOsu.Game.HitObjects.Drawables
{
    public class SkinnableTextureProperty : SkinnableProperty
    {
        private Sprite Drawable;

        public SkinnableTextureProperty(Sprite drawable, string name)
        {
            PropertyType = ISkinnableProperty.Type.Texture;
            Drawable = drawable;
            Name = name;
        }

        public override void Resolve(object obj)
        {
            if (Resolved) return;
            base.Resolve(obj);

            Drawable.Texture = (Texture)obj;
            if (Name.Equals("hitcircle.png"))
                Drawable.Texture = TextureUtil.CropIfNeeded(Drawable.Texture);
            
            Drawable.Scale *= new Vector2(TextureUtil.GetScaleFor(Drawable.Texture));
        }
    }
}