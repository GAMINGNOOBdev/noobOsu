using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using noobOsu.Game.Beatmaps;
using noobOsu.Game.HitObjects.Drawables;
using osuTK.Graphics;

namespace noobOsu.Game.HitObjects
{
    public partial class Slider : DrawableHitObject
    {
        private Sprite approachCircle, hitcircleArea, hitcircleOverlay;
        private SmoothPath path;
        private bool startApproach, approachEnded, started, circleEnded;
        private float totalVisibleTime, fadeTime, currentTime, radius;
        private float waitingTime, currentDelayTime, hitWindow;
        private float sliderCompletionTime;
        private bool ending = false;

        public Slider(HitObject hitObj, IBeatmap beatmap, IBeatmapDifficulty difficulty, BeatmapColors colors) : base(hitObj, colors, beatmap)
        {
            radius = 64f * ((1.0f - 0.7f * (difficulty.CS - 5f) / 5f) / 2f);

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(difficulty.OD, 80, 50, 20);
            waitingTime = HitObject.Time - hitWindow;

            // add the time it takes to complete the slider to the total visible time
            sliderCompletionTime = HitObject.SliderInformation.TotalSliderSpan / ( difficulty.SliderMultiplier * 100 * (-100/TimingInfo.BeatLength) ) * beatmap.GetInfo().Timing.BPM_At(HitObject.Time);

            currentTime = 0f;
            currentDelayTime = 0f;

            startApproach = false;
            approachEnded = false;
            circleEnded = false;
            started = false;

            RelativePositionAxes = Axes.Both;
            Position = HitObject.Position;
        }
        
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            path = new SmoothPath();
            path.RelativeSizeAxes = Axes.None;
            path.PathRadius = radius;
            path.Vertices = HitObject.Path.GetCurvePoints();
            path.OriginPosition = path.PositionInBoundingBox(Vector2.Zero);

            Vector4 color = Color.BottomLeft.ToVector();

            path.Colour = new Color4(color.X, color.Y, color.Z, color.W);
            path.Alpha = 0f;

            approachCircle = new Sprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(4f),
                Texture = textures.Get("Skins/default/approachcircle.png"),
            };
            approachCircle.Size = new Vector2(radius*2);
            approachCircle.Colour = Color;

            hitcircleArea = new Sprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(1f),
                Texture = textures.Get("Skins/default/hitcircle.png"),
            };
            hitcircleArea.Size = new Vector2(radius*2);
            hitcircleArea.Colour = Color;
            

            hitcircleOverlay = new Sprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(1f),
                Texture = textures.Get("Skins/default/hitcircleoverlay.png"),
            };
            hitcircleOverlay.Size = new Vector2(radius*2);


            hitcircleArea.Alpha = 0f;
            hitcircleOverlay.Alpha = 0f;
            approachCircle.Alpha = 0f;

            AddInternal(path);
            AddInternal(hitcircleArea);
            AddInternal(hitcircleOverlay);
            AddInternal(approachCircle);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        public override void End()
        {
            if (ending) return;
            ending = true;
            
            path.FadeOutFromOne(200);
            /*
            Logger.Log("sliderCompletionTime = HitObject.SliderInformation.TotalSliderSpan / ( difficulty.SliderMultiplier * 100 * (-100/TimingInfo.BeatLength) ) * beatmap.GetInfo().Timing.BPM_At(HitObject.Time)");
            Logger.Log(sliderCompletionTime + " = " + HitObject.SliderInformation.TotalSliderSpan + " / (" + ParentMap.GetInfo().Difficulty.SliderMultiplier + " * 100 * (-100/" + TimingInfo.BeatLength + ") ) * " + ParentMap.GetInfo().Timing.BPM_At(HitObject.Time));
            Logger.Log("slider ============================== timestamp --> " + HitObject.Time + " | waitingTime --> " + waitingTime + " | totalVisibleTime --> " + totalVisibleTime + " | fadeTime -->" + fadeTime + " | hitWindow --> " + hitWindow + " ==============================");
            Logger.Log("path: " + HitObject.Path.ToString());
            */
        }

        public override void DisposeResources()
        {
            path.Dispose();
            hitcircleArea.Dispose();
            hitcircleOverlay.Dispose();
            approachCircle.Dispose();
        }

        public void Start() {
            if (started) return;
            started = true;
            startApproach = true;
            
            path.FadeInFromZero(fadeTime);

            approachCircle.ScaleTo(1, totalVisibleTime);
            approachCircle.FadeInFromZero(fadeTime);
            hitcircleArea.FadeInFromZero(fadeTime);
            hitcircleOverlay.FadeInFromZero(fadeTime);
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

                    hitcircleArea.FadeOutFromOne(200);
                    hitcircleOverlay.FadeOutFromOne(200);
                    hitcircleOverlay.ScaleTo(1.5f, 200);
                    approachCircle.Alpha = 0f;
                }
            }

            if (approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;
                if (currentTime >= hitWindow + sliderCompletionTime - fadeTime)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}