using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Primitives;

namespace noobOsu.Game.Util
{
    public static class TextureUtil
    {
        public static Texture CropIfNeeded(Texture t)
        {
            if (t.Size.X > 120 || t.Size.Y > 120)
            {
                int xAmount = (int)t.Size.X - 120;
                int yAmount = (int)t.Size.Y - 120;
                RectangleF cropArea = new RectangleF(xAmount/2f, yAmount/2f, 120f, 120f);
                
                return t.Crop( cropArea );
            }
            return t;
        }

        public static float GetScaleFor(Texture t)
        {
            return t.Size.X / 120f;
        }
    }
}