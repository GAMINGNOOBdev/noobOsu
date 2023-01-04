using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.HitObjects
{
    public partial class Slider : DrawableHitObject
    {
        private SliderStartCircle sliderStart;
        private SliderEndCircle sliderEnd;
        //private SliderBall Ball;
        private ColoredPath path;
        private bool startApproach, approachEnded, started, circleEnded;
        private float totalVisibleTime, fadeTime, currentTime, radius;
        private float waitingTime, currentDelayTime, hitWindow;
        private bool ending = false;
        
        public float SliderCompletionTime;

        public Slider(HitObject hitObj, IBeatmap beatmap, IBeatmapDifficulty difficulty, IColorStore colors) : base(hitObj, colors, beatmap)
        {
            radius = 64f * ((1.0f - 0.7f * (difficulty.CS - 5f) / 5f) / 2f);

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(difficulty.OD, 80, 50, 20);
            waitingTime = HitObject.Time - hitWindow;

            // add the time it takes to complete the slider to the total visible time
            if (TimingInfo != null)
                SliderCompletionTime = HitObject.SliderInformation.TotalSliderSpan / ( difficulty.SliderMultiplier * 100 * (-100/TimingInfo.BeatLength) ) * beatmap.GetInfo().Timing.BPM_At(HitObject.Time);
            else
                SliderCompletionTime = HitObject.SliderInformation.TotalSliderSpan / ( difficulty.SliderMultiplier * 100 ) * beatmap.GetInfo().Timing.BPM_At(HitObject.Time);

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
            path = new ColoredPath();
            path.RelativeSizeAxes = Axes.None;
            path.PathRadius = radius;
            path.Vertices = HitObject.Path.GetCurvePoints();
            path.OriginPosition = path.PositionInBoundingBox(Vector2.Zero);
            path.Alpha = 0f;
            AddProperty(new SkinnableColorProperty(path.AccentColor, Color, "SliderTrack"));
            AddProperty(new SkinnableColorProperty(path.BorderColor, Color4.White, "SliderBorder"));

            sliderStart = new SliderStartCircle(this, HitObject.Position);
            sliderEnd = new SliderEndCircle(this, HitObject.Path.GetLastPoint());

            AddInternal(path);
            AddInternal(sliderStart);
            AddInternal(sliderEnd);
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
            
            // Logger.Log("slider ============================== timestamp --> " + HitObject.Time + " | waitingTime --> " + waitingTime + " | totalVisibleTime --> " + totalVisibleTime + " | fadeTime -->" + fadeTime + " | hitWindow --> " + hitWindow + " ==============================");
            // Logger.Log("path: " + HitObject.Path.ToString());
            // Logger.Log("AccentColor.Value: " + path.AccentColor.Value);
        }

        public override void DisposeResources()
        {
            path.Dispose();
            sliderStart.DisposeResources();
            sliderEnd.DisposeResources();
        }

        public void Start() {
            if (started) return;
            started = true;
            startApproach = true;
            
            path.FadeInFromZero(fadeTime);

            sliderStart.Start();
            sliderEnd.Start();

            //Logger.Log("sliderCompletionTime = HitObject.SliderInformation.TotalSliderSpan / ( difficulty.SliderMultiplier * 100 * (-100/TimingInfo.BeatLength) ) * beatmap.GetInfo().Timing.BPM_At(HitObject.Time)");
            //Logger.Log(sliderCompletionTime + " = " + HitObject.SliderInformation.TotalSliderSpan + " / (" + ParentMap.GetInfo().Difficulty.SliderMultiplier + " * 100 * (-100/" + TimingInfo.BeatLength + ") ) * " + ParentMap.GetInfo().Timing.BPM_At(HitObject.Time));
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

                    sliderStart.End();
                    sliderEnd.End();
                }

                //Ball.Update();
            }

            if (approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;
                if (currentTime >= hitWindow + SliderCompletionTime - fadeTime)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}