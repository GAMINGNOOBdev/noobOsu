using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.UI
{
    public partial class BeatmapDropdown : BasicDropdown<string>
    {
        public BeatmapDropdown() : base()
        {
            Width = 250;
            Depth = 0;
        }

        public void AddMap(string name)
        {
            if (name == null) return;

            AddDropdownItem(name, name);
        }
    }
}