using osuTK.Graphics;
using noobOsu.Game.Skins;
using osu.Framework.Logging;
using osu.Framework.Bindables;

namespace noobOsu.Game.Skins.Properties
{
    public class SkinnableColorProperty : SkinnableProperty
    {
        private Bindable<Color4> Color;

        public SkinnableColorProperty(Bindable<Color4> col, Color4 defaultValue, string name)
        {
            PropertyType = ISkinnableProperty.Type.Color;
            Color = new Bindable<Color4>();
            Color.BindTo(col);
            Color.Value = defaultValue;
            Name = name;
        }

        public override void Resolve(object obj)
        {
            if (Resolved) return;
            base.Resolve(obj);

            if (obj != null)
                Color.Value = (Color4)obj;
        }
    }
}