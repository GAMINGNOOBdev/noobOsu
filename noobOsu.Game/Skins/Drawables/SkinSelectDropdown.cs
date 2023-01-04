using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Skins.Drawables
{
    public partial class SkinSelectDropdown : BasicDropdown<ISkin>
    {
        public SkinSelectDropdown() : base()
        {
            Width = 250;
            Depth = 0;
        }

        public void AddSkin(string name, ISkin skin)
        {
            if (skin == null) return;

            AddDropdownItem(name, skin);
        }
    }
}