using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Skins.Drawables
{
    public partial class SkinSelectDropdown : BasicDropdown<ISkin>
    {
        private readonly List<string> skins = new List<string>();

        public SkinSelectDropdown() : base()
        {
            Width = 250;
            Depth = 0;
        }

        public void AddSkin(string name, ISkin skin)
        {
            if (skin == null || skins.Contains(name)) return;

            AddDropdownItem(name, skin);
            skins.Add(name);
        }
    }
}