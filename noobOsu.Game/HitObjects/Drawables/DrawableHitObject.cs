using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Colour;
using noobOsu.Game.Beatmaps.Timing;
using noobOsu.Game.Beatmaps;

namespace noobOsu.Game.HitObjects.Drawables
{
    public partial class DrawableHitObject : CompositeDrawable
    {
        private TimingPoint timingPoint;
        protected HitObject HitObject { get; private set; } = null;
        protected ColourInfo Color { get; private set; } = new ColourInfo();
        protected readonly IBeatmap ParentMap;
        protected ITimingPoint TimingInfo => timingPoint;

        protected DrawableHitObject(HitObject hitObj, BeatmapColors colors, IBeatmap beatmap)
        {
            if (hitObj.isNewCombo()) colors.NextColor();
            HitObject = hitObj;
            colors.Skip(HitObject.ComboColorSkip);
            Color = colors.GetComboColor();

            timingPoint = (TimingPoint)beatmap.GetInfo().Timing.GetTimingPoint(HitObject.Time, false);
            if (timingPoint == null && beatmap.GetInfo().Timing.GetTimingPoint(HitObject.Time, true) != null)
            {
                timingPoint = (TimingPoint)beatmap.GetInfo().Timing.GetTimingPoint(HitObject.Time, true).Clone();
                timingPoint.BeatLength = 1;
            }
            else if (beatmap.GetInfo().Timing.GetTimingPoint(HitObject.Time, true) == null)
            {
                timingPoint = null;
            }
            else
                timingPoint = (TimingPoint)timingPoint.Clone();

            ParentMap = beatmap;

            this.Depth = HitObject.Time;
        }

        public virtual void End() {}

        public virtual void DisposeResources() {}
    }
}