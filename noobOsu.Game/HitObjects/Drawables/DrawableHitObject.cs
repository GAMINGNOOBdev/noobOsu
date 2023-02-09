using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using System.Diagnostics;
using osu.Framework.Audio;
using noobOsu.Game.Beatmaps;
using osu.Framework.Graphics;
using osu.Framework.Audio.Sample;
using System.Collections.Generic;
using noobOsu.Game.Beatmaps.Timing;
using osu.Framework.Graphics.Audio;
using noobOsu.Game.Skins.Properties;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Containers;
using Logger = osu.Framework.Logging.Logger;

namespace noobOsu.Game.HitObjects.Drawables
{
    public partial class DrawableHitObject : CompositeDrawable, ISkinnable
    {
        private TimingPoint timingPoint;
        private Sample hitsoundSample;
        protected Skinnable _skinnable = new Skinnable();
        public HitObject HitObject { get; private set; } = null;
        public Color4 Color { get; private set; } = new Color4();
        public int ComboNumber { get; private set; } = 0;
        public readonly IBeatmap ParentMap;
        public SpriteText CircleNumbers { get; private set; }
        public ITimingPoint TimingInfo => timingPoint;
        public double Radius { get; protected set; }
        public DrawableSample Sample { get; set; }
        public double COOL;

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

            RelativePositionAxes = Axes.None;
            Position = hitObj.Position;
            Radius = 64f * ((1.0f - 0.7f * (beatmap.GetInfo().Difficulty.CS - 5f) / 5f) / 2f);
            ProcessCustomClock = false;

            /*LifetimeStart = HitObject.Time - HitObject.ObjectTiming.TotalVisibleTime;
            LifetimeEnd = HitObject.EndTime + HitObject.ObjectTiming.HitWindow + 200;*/
        }

        public void LoadHitsound(AudioManager audioManager, string soundLocation)
        {
            if (audioManager != null)
            {
                if (TimingInfo != null)
                    hitsoundSample = audioManager.GetSampleStore().Get( soundLocation + TimingInfo.GetHitsound(HitObject.HitSound));
                else
                    hitsoundSample = audioManager.GetSampleStore().Get( soundLocation + ITimingPoint.GetDefaultHitsound(HitObject.HitSound));
                Sample = new DrawableSample(hitsoundSample, true);
                Sample.Volume.Value = Settings.GameSettings.GetEffectVolume() * Settings.GameSettings.GetMasterVolume();
                AddInternal(Sample);
            }
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

        public virtual void DisposeResources() {
            if (Sample != null)
                Sample.Dispose();
        }

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