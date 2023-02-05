using noobOsu.Game.Beatmaps;

namespace noobOsu.Game.HitObjects
{
    public interface IHitObjectTiming
    {
        // the amount of time that this hitobject is visible
        double TotalVisibleTime { get; }

        // the amount of time it takes to fade in this hitobject
        double FadeTime { get; }

        // how much time there is for the user to interact with this hitobject (not used on slider bodies)
        double HitWindow { get; }
    }

    public class HitObjectTiming : IHitObjectTiming
    {
        public double TotalVisibleTime { get; private set; }
        public double FadeTime { get; private set; }
        public double HitWindow { get; private set; }

        public HitObjectTiming(BeatmapDifficulty difficulty)
        {
            TotalVisibleTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1800f, 1200f, 450f);
            FadeTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1200f, 800f, 300f);
            HitWindow = BeatmapDifficulty.ScaleWithRange(difficulty.OD, 80, 50, 20);
        }
    }
}