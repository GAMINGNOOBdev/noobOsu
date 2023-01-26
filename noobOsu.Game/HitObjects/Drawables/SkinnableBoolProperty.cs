using osuTK.Graphics;
using noobOsu.Game.Skins;
using osu.Framework.Logging;
using osu.Framework.Bindables;

namespace noobOsu.Game.HitObjects.Drawables
{
    public class SkinnableBoolProperty : SkinnableProperty
    {
        private Bindable<bool> BooleanValue;

        public SkinnableBoolProperty(Bindable<bool> col, bool defaultValue, string name)
        {
            PropertyType = ISkinnableProperty.Type.Bool;
            BooleanValue = new Bindable<bool>();
            BooleanValue.BindTo(col);
            BooleanValue.Value = defaultValue;
            Name = name;
        }

        public override void Resolve(object obj)
        {
            if (Resolved) return;
            base.Resolve(obj);

            if (obj != null)
                BooleanValue.Value = (bool)obj;
        }
    }
}