using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using System.Diagnostics;
using noobOsu.Game.Beatmaps;
using osu.Framework.Graphics;
using System.Collections.Generic;
using noobOsu.Game.Skins.Drawables;
using noobOsu.Game.Beatmaps.Timing;
using osu.Framework.Graphics.Sprites;
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
        public int ComboNumber { get; private set; } = 0;
        public readonly IBeatmap ParentMap;
        public SpriteText CircleNumbers { get; private set; }
        public ITimingPoint TimingInfo => timingPoint;
        public double Radius { get; protected set; }

        protected DrawableHitObject(HitObject hitObj, IColorStore colors, IBeatmap beatmap)
        {
            if (hitObj.isNewCombo()) colors.NextColor();
            HitObject = hitObj;
            colors.Skip(HitObject.ComboColorSkip);
            Debug.Assert(colors != null);
            Color = (Color4)colors.GetColor();
            ComboNumber = colors.GetComboNumber();

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
                if (timingPoint.BeatLength.Equals(double.NaN))
                    timingPoint = null;
                else
                    timingPoint = (TimingPoint)timingPoint.Clone();

            ParentMap = beatmap;

            if (HitObject.isSlider())
                HitObject.setSliderInfo(timingPoint);

            this.Depth = HitObject.Time;

            RelativePositionAxes = Axes.Both;
            Position = hitObj.Position;
        }

        public void AddNumbers(SpriteText objects) 
        {
            CircleNumbers = objects;
            CircleNumbers.Alpha = 0f;
            CircleNumbers.Origin = Anchor.Centre;
            CircleNumbers.RelativeAnchorPosition = new Vector2(0.5f);
            this.AddInternal(CircleNumbers);
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