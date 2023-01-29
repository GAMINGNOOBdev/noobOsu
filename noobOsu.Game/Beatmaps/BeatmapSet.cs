using System.IO;
using osu.Framework.Audio;
using osu.Framework.Logging;
using osu.Framework.Threading;
using noobOsu.Game.HitObjects;
using osu.Framework.Audio.Track;
using System.Collections.Generic;
using osu.Framework.Graphics.Audio;
using noobOsu.Game.Beatmaps.Timing;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapSet
    {
        // id of the beatmap set
        long SetID { get; }

        // name of the beatmap
        string SetName { get; }
        
        // general info for all beatmaps and the entire set
        IBeatmapGeneral SetInfo { get; }

        // list of all beatmaps in this set
        IReadOnlyList<IBeatmapGeneral> GetBeatmaps();
    }

    public class BeatmapSet : IBeatmapSet
    {
        private readonly List<BeatmapGeneral> beatmaps = new List<BeatmapGeneral>();
        private BeatmapGeneral mapsetInfo = null;

        public long SetID { get; private set; }
        public string SetName { get; private set; }
        public IBeatmapGeneral SetInfo {
            get => mapsetInfo;
            set => throw new System.NotSupportedException("setting of the set info not supported, can only be read using ReadSetInfo");
        }
        public IReadOnlyList<IBeatmapGeneral> GetBeatmaps() => beatmaps;

        public void ReadSetInfo(string setDirectoryName)
        {
            string[] setInfo = setDirectoryName.Split(' ', 2);

            SetID = long.Parse(setInfo[0].Trim());
            SetName = setInfo[1].Trim();

            foreach (string s in Directory.EnumerateFiles("Songs/" + setDirectoryName, "*.osu", SearchOption.AllDirectories))
            {
                string filename = s.Substring("Songs/".Length + setDirectoryName.Length + 1);
                
                BeatmapGeneral g = new BeatmapGeneral(this);
                g.SetAttributes(filename);
                beatmaps.Add(g);
                if (mapsetInfo == null)
                    mapsetInfo = g;
            }

            if (mapsetInfo == null)
            {
                mapsetInfo = new BeatmapGeneral(this);
                mapsetInfo.FromMapset(SetName);
            }
        }
    }
}