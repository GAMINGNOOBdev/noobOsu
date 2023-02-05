using osuTK;
using osuTK.Input;
using noobOsu.Game.UI.Cursor;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using noobOsu.Game.Skins.Properties;

namespace noobOsu.Game.Skins.Drawables
{
    public partial class SkinCursorTrail : CompositeDrawable, ISkinnableProperty
    {
        private const int MAX_TRAIL_LENGTH = 512;

        private SkinnableSprite[] sprites = new SkinnableSprite[MAX_TRAIL_LENGTH];
        private float skinnableScale;

        public ISkinnableProperty.Type PropertyType => ISkinnableProperty.Type.Texture;

        string ISkinnableProperty.Name => "cursortrail";

        public bool Resolved { get; set; }

        float ISkinnableProperty.Scale => skinnableScale;
        
        public void Resolve(object obj)
        {
            Resolved = true;
            foreach (SkinnableSprite s in sprites)
            {
                s.Texture = (Texture)obj;
                s.ScaleFactor = skinnableScale;
            }
        }

        public void SetScale(float scale)
        {
            skinnableScale = scale;
        }
    }
}