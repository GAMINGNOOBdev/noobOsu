using noobOsu.Game.Skins;
using osu.Framework.Audio;
using osu.Framework.Threading;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmap
    {
        bool Started { get; set; }
        IBeatmapGeneral CurrentMap { get; }

        void RemoveObject(DrawableHitObject obj);

        void StartBeatmap(Scheduler ParentScheduler);

        BeatmapInfo GetInfo();

        void LoadBeatmap(IBeatmapGeneral map);
        void Load(AudioManager audioManager, TextureStore Textures);

        Track GetAudio();
        void ClearData();
    }
}