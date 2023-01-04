using osuTK.Graphics;
using noobOsu.Game.Skins;
using noobOsu.Game.Beatmaps;
using System.Collections.Generic;
using noobOsu.Game.Beatmaps.Timing;
using osu.Framework.Graphics.Containers;
using Logger = osu.Framework.Logging.Logger;

namespace noobOsu.Game.HitObjects.Drawables
{
    public partial class DrawableHitObject : CompositeDrawable, ISkinnable
    {
        private TimingPoint timingPoint;
        protected Skinnable _skinnable = new Skinnable();
        public HitObject HitObject { get; private set; } = null;
        public Color4 Color { get; private set; } = new Color4();
        public readonly IBeatmap ParentMap;
        public ITimingPoint TimingInfo => timingPoint;

        protected DrawableHitObject(HitObject hitObj, IColorStore colors, IBeatmap beatmap)
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
                if (timingPoint.BeatLength.Equals(float.NaN))
                    timingPoint = null;
                else
                    timingPoint = (TimingPoint)timingPoint.Clone();

            ParentMap = beatmap;

            this.Depth = HitObject.Time;
        }

        public virtual void End() {}

        public virtual void DisposeResources() {}

        public void AddProperty(ISkinnableProperty property)
        {
            _skinnable.AddProperty(property);
        }

        public List<ISkinnableProperty> GetProperties()
        {
            return _skinnable.GetProperties();
        }

        public void ResetResolvedProperties()
        {
            _skinnable.ResetResolvedProperties();
        }
    }
}