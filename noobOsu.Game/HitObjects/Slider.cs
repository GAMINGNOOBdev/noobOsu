using osuTK;
using System;
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
        private SliderBall Ball;
        private ColoredPath path;
        private bool startApproach, approachEnded, started, circleEnded;
        private float totalVisibleTime, fadeTime, currentTime;
        private float waitingTime, currentDelayTime, hitWindow;
        private bool ending = false;
        public float Duration { get; private set; }

        public Slider(HitObject hitObj, IBeatmap beatmap, IBeatmapDifficulty difficulty, IColorStore colors) : base(hitObj, colors, beatmap)
        {
            Radius = 64f * ((1.0f - 0.7f * (difficulty.CS - 5f) / 5f) / 2f);

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(difficulty.OD, 80, 50, 20);
            waitingTime = HitObject.Time - hitWindow;

            // add the time it takes to complete the slider to the total visible time
            Duration = HitObject.EndTime - HitObject.Time;

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
            path.PathRadius = Radius;
            path.Vertices = HitObject.Path.GetCurvePoints();
            path.OriginPosition = path.PositionInBoundingBox(Vector2.Zero);
            path.Alpha = 0f;
            AddProperty(new SkinnableColorProperty(path.AccentColor, Color, "SliderTrack"));
            AddProperty(new SkinnableColorProperty(path.BorderColor, Color4.White, "SliderBorder"));

            sliderStart = new SliderStartCircle(this, HitObject.Position);
            if (HitObject.SliderInformation.SlideRepeat > 0)
                sliderEnd = new SliderEndCircle(this, HitObject.Path.GetLastPoint());
            Ball = new SliderBall(this);

            AddInternal(path);
            AddInternal(sliderStart);
            
            if (sliderEnd != null)
                AddInternal(sliderEnd);
            
            AddInternal(Ball);
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
            Ball.End();
            
            // Logger.Log("slider ============================== timestamp --> " + HitObject.Time + " | waitingTime --> " + waitingTime + " | totalVisibleTime --> " + totalVisibleTime + " | fadeTime -->" + fadeTime + " | hitWindow --> " + hitWindow + " ==============================");
            // Logger.Log("path: " + HitObject.Path.ToString());
            // Logger.Log("AccentColor.Value: " + path.AccentColor.Value);
        }

        public override void DisposeResources()
        {
            path.Dispose();
            sliderStart.DisposeResources();
            Ball.DisposeResources();
            if (sliderEnd != null)
                sliderEnd.DisposeResources();
        }

        public void Start() {
            if (started) return;
            started = true;
            startApproach = true;
            
            path.FadeInFromZero(fadeTime);

            sliderStart.Start();
            if (sliderEnd != null)
                sliderEnd.Start();

            /*
            Logger.Log(" ======== slider info start ======== ");
            if (HitObject.Timing != null)
            {
                Logger.Log("EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * (float)Path.GetLength() / (beatmap.GetInfo().Difficulty.SliderMultiplier * (-100/HitObject.Timing.BeatLength)))");
                Logger.Log(HitObject.EndTime + " = " + HitObject.Time + " + (int)((" + HitObject.SliderInformation.SlideRepeat + "+1) * (float)" + HitObject.Path.GetLength() + " / (" + ParentMap.GetInfo().Difficulty.SliderMultiplier + " * (-100/" + HitObject.Timing.BeatLength + ")))");
            }
            else
            {
                Logger.Log("EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * (float)Path.GetLength() / beatmap.GetInfo().Difficulty.SliderMultiplier)");
                Logger.Log(HitObject.EndTime + " = " + HitObject.Time + " + (int)((" + HitObject.SliderInformation.SlideRepeat + "+1) * (float)" + HitObject.Path.GetLength() + " / " + ParentMap.GetInfo().Difficulty.SliderMultiplier + ")");
            }
            Logger.Log(" ======== slider info end ======== ");
            */
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
                    Ball.Start();
                    if (sliderEnd != null)
                        sliderEnd.End();
                }
            }

            if (approachEnded)
            {
                currentTime += (float)Clock.ElapsedFrameTime;
                double progress = currentTime / (Duration);
                progress = Math.Clamp(progress, 0, 1);
                Ball.Update(progress);
                if (currentTime >= Duration)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}