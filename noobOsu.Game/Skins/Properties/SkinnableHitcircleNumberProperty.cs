using osuTK;
using noobOsu.Game.Skins;
using osu.Framework.Logging;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Skins.Properties
{
    public class SkinnableHitcircleNumberProperty : SkinnableProperty
    {
        private DrawableHitObject hitObject;
        private int ComboNumber = 0;

        public SkinnableHitcircleNumberProperty(DrawableHitObject obj, int comboNum, string name)
        {
            PropertyType = ISkinnableProperty.Type.Font;
            hitObject = obj;
            ComboNumber = comboNum;
            Name = name;
        }

        public override void Resolve(object obj)
        {
            if (Resolved) return;
            base.Resolve(obj);

            if (obj != null && obj is object[])
            {
                object[] objs = (object[])obj;
                ISkin skin = (ISkin)objs[0];
                hitObject.AddNumbers( skin.GetHitobjectNumber(ComboNumber, hitObject, (TextureStore)objs[1]) );
            }
        }
    }
}