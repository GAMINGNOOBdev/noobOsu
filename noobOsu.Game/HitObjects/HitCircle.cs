using osuTK;
using noobOsu.Game.Skins;
using osu.Framework.Audio;
using noobOsu.Game.Beatmaps;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using noobOsu.Game.Skins.Drawables;
using noobOsu.Game.Skins.Properties;
using noobOsu.Game.HitObjects.Drawables;
using osu.Framework.Input.Events;

namespace noobOsu.Game.HitObjects
{
    public partial class HitCircle : DrawableHitObject
    {
        private SkinnableSprite approachCircle, hitcircleArea, hitcircleOverlay;
        private bool startApproach, approachEnded, started, circleEnded;
        private double waitingTime, currentDelayTime, currentTime;
        private bool ending = false;

        public HitCircle(IHitObject hitObj, IBeatmap beatmap, IColorStore colors, ISkin skin, bool useBeatmapHitsound, AudioManager audioManager) : base(hitObj, colors, beatmap)
        {
            waitingTime = HitObject.Time - HitObject.ObjectTiming.HitWindow - HitObject.ObjectTiming.TotalVisibleTime;

            currentDelayTime = 0f;
            currentTime = 0f;

            startApproach = false;
            approachEnded = false;
            circleEnded = false;
            started = false;

            if (useBeatmapHitsound)
                base.LoadHitsound(audioManager, "Songs/" + beatmap.CurrentMap.ParentSet.AsDirectoryName());
            else
                base.LoadHitsound(audioManager, "Skins/" + (skin.DirectoryName.EndsWith("/") ? skin.DirectoryName : skin.DirectoryName + "/"));
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            approachCircle = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            approachCircle.Scale = new Vector2((float)Radius*2 * 4f);
            approachCircle.Colour = Color;
            AddProperty(new SkinnableTextureProperty(approachCircle, "approachcircle"));

            hitcircleArea = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleArea.Scale = new Vector2((float)Radius*2);
            hitcircleArea.Colour = Color;
            AddProperty(new SkinnableTextureProperty(hitcircleArea, "hitcircle", true));

            hitcircleOverlay = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleOverlay.Scale = new Vector2((float)Radius*2);
            AddProperty(new SkinnableTextureProperty(hitcircleOverlay, "hitcircleoverlay"));

            AddProperty(new SkinnableHitcircleNumberProperty(this, ComboNumber, "numbers"));

            hitcircleArea.Alpha = 0f;
            hitcircleOverlay.Alpha = 0f;
            approachCircle.Alpha = 0f;

            AddInternal(hitcircleArea);
            AddInternal(hitcircleOverlay);
            AddInternal(approachCircle);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        public void Start()
        {
            if (started) return;
            started = true;
            startApproach = true;
            approachCircle.ScaleTo((float)Radius*2 * (float)approachCircle.ScaleFactor, HitObject.ObjectTiming.TotalVisibleTime);

            approachCircle.FadeInFromZero(HitObject.ObjectTiming.FadeTime);
            hitcircleArea.FadeInFromZero(HitObject.ObjectTiming.FadeTime);
            hitcircleOverlay.FadeInFromZero(HitObject.ObjectTiming.FadeTime);
            CircleNumbers.FadeInFromZero(HitObject.ObjectTiming.FadeTime);
        }

        public override void End()
        {
            if (ending) return;
            ending = true;

            if (Sample != null)
                Sample.Play();
            
            hitcircleArea.FadeOutFromOne(200);
            hitcircleOverlay.FadeOutFromOne(200);
            CircleNumbers.ScaleTo(1.5f, 200);
            CircleNumbers.FadeOutFromOne(200);
            hitcircleArea.ScaleTo((float)Radius*2 * 1.5f * (float)hitcircleArea.ScaleFactor, 200);
            hitcircleOverlay.ScaleTo((float)Radius*2 * 1.5f * (float)hitcircleOverlay.ScaleFactor, 200);
            approachCircle.Alpha = 0f;
        }

        public override void DisposeResources()
        {
            base.DisposeResources();
            hitcircleArea.Dispose();
            hitcircleOverlay.Dispose();
            approachCircle.Dispose();
            CircleNumbers.Dispose();
        }

        protected override void Update()
        {
            base.Update();
            COOL = currentDelayTime;
            if (!ParentMap.Started) return;

            if (circleEnded) return;
            if (currentDelayTime < waitingTime)
            {
                currentDelayTime += (float)Clock.ElapsedFrameTime;
            }
            else
            {
                Start();
            }
            

            if (startApproach && !approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;

                if (currentTime >= HitObject.ObjectTiming.TotalVisibleTime)
                {
                    startApproach = false;
                    approachEnded = true;
                    currentTime = 0f;
                }
            }

            if (approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;
                if (currentTime >= HitObject.ObjectTiming.HitWindow)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}