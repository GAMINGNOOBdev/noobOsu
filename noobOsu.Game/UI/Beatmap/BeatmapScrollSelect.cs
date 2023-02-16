using noobOsu.Game.Beatmaps;
using noobOsu.Game.UI.Basic;

namespace noobOsu.Game.UI.Beatmap
{
    //public partial class BeatmapScrollSelect : BasicScrollSelect<BeatmapSet>
    public partial class BeatmapScrollSelect : BasicSearchableScrollSelect<BeatmapSet>
    {
        public void AddBeatmap(string dirPath)
        {
            AddItem(new BeatmapScrollSelectItem(dirPath, this));
        }
    }
}