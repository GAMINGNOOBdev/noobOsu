using osuTK;
using noobOsu.Game.Beatmaps;

namespace noobOsu.Game.HitObjects
{
    public interface IHitObject
    {
        Vector2 Position { get; }
        IBeatmap ParentMap { get; }
        IHitObjectTiming ObjectTiming { get; }

        int Time { get; }
        int EndTime { get; }
        int Type { get; }
        int HitSound { get; }

        bool IsCircle();
        bool IsSlider();
        bool IsSpinner();
        bool IsNewCombo();
    }
}