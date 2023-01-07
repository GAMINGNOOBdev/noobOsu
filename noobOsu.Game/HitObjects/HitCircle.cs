using osuTK;
using noobOsu.Game.Skins;
using noobOsu.Game.Beatmaps;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.HitObjects
{
    public partial class HitCircle : DrawableHitObject
    {
        private HitObjectSprite approachCircle, hitcircleArea, hitcircleOverlay;
        private bool startApproach, approachEnded, started, circleEnded;
        private float totalVisibleTime, fadeTime, currentTime;
        private float waitingTime, currentDelayTime, hitWindow;
        private bool ending = false;

        public HitCircle(HitObject hitObj, IBeatmap beatmap, IBeatmapDifficulty difficulty, IColorStore colors) : base(hitObj, colors, beatmap)
        {
            Radius = 64f * ((1.0f - 0.7f * (difficulty.CS - 5f) / 5f) / 2f);

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(difficulty.OD, 80, 50, 20);
            waitingTime = hitObj.Time - hitWindow;

            currentDelayTime = 0f;
            currentTime = 0f;

            startApproach = false;
            approachEnded = false;
            circleEnded = false;
            started = false;

            RelativePositionAxes = Axes.Both;
            Position = hitObj.Position;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            approachCircle = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            approachCircle.Scale = new Vector2(Radius*2 * 4f);
            approachCircle.Colour = Color;
            AddProperty(new SkinnableTextureProperty(approachCircle, "approachcircle"));

            hitcircleArea = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleArea.Scale = new Vector2(Radius*2);
            hitcircleArea.Colour = Color;
            AddProperty(new SkinnableTextureProperty(hitcircleArea, "hitcircle", true));

            hitcircleOverlay = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleOverlay.Scale = new Vector2(Radius*2);
            AddProperty(new SkinnableTextureProperty(hitcircleOverlay, "hitcircleoverlay"));


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
            approachCircle.ScaleTo(Radius*2 * approachCircle.ScaleFactor, totalVisibleTime);

            approachCircle.FadeInFromZero(fadeTime);
            hitcircleArea.FadeInFromZero(fadeTime);
            hitcircleOverlay.FadeInFromZero(fadeTime);
        }

        public override void End()
        {
            if (ending) return;
            ending = true;
            
            hitcircleArea.FadeOutFromOne(200);
            hitcircleOverlay.FadeOutFromOne(200);
            hitcircleArea.ScaleTo(Radius*2 * 1.5f * hitcircleArea.ScaleFactor, 200);
            hitcircleOverlay.ScaleTo(Radius*2 * 1.5f * hitcircleOverlay.ScaleFactor, 200);
            approachCircle.Alpha = 0f;
        }

        public override void DisposeResources()
        {
            hitcircleArea.Dispose();
            hitcircleOverlay.Dispose();
            approachCircle.Dispose();
        }

        protected override void Update()
        {
            if (!ParentMap.Started) return;
            base.Update();

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

                if (currentTime >= totalVisibleTime)
                {
                    startApproach = false;
                    approachEnded = true;
                    currentTime = 0f;
                }
            }

            if (approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;
                if (currentTime >= hitWindow)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}