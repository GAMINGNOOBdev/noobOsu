using osu.Framework.Audio;
using osu.Framework.Threading;
using osu.Framework.Audio.Track;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmap
    {
        bool Started { get; set; }

        void RemoveObject(DrawableHitObject obj);

        void StartBeatmap(Scheduler ParentScheduler);

        BeatmapInfo GetInfo();

        void LoadBeatmap(string path);
        void Load(AudioManager audioManager);

        Track GetAudio();
        void ClearData();
    }
}