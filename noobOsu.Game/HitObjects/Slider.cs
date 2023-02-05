using osuTK;
using System;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using noobOsu.Game.Skins.Properties;
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
        private double waitingTime, currentDelayTime, currentTime;
        private bool ending = false;
        public double Duration { get; private set; }

        public Slider(HitObject hitObj, IBeatmap beatmap, IColorStore colors) : base(hitObj, colors, beatmap)
        {
            waitingTime = HitObject.Time - HitObject.ObjectTiming.HitWindow;

            // add the time it takes to complete the slider to the total visible time
            Duration = HitObject.EndTime - HitObject.Time;

            currentTime = 0f;
            currentDelayTime = 0f;

            startApproach = false;
            approachEnded = false;
            circleEnded = false;
            started = false;
        }
        
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            path = new ColoredPath();
            path.RelativeSizeAxes = Axes.None;
            path.PathRadius = (float)Radius;
            path.Vertices = HitObject.Path.GetCurvePoints();
            path.OriginPosition = path.PositionInBoundingBox(Vector2.Zero);
            path.Alpha = 0f;
            AddProperty(new SkinnableColorProperty(path.AccentColor, Color, "SliderTrack"));
            AddProperty(new SkinnableColorProperty(path.BorderColor, Color4.White, "SliderBorder"));

            sliderStart = new SliderStartCircle(this);
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
            sliderStart.End();
            if (sliderEnd != null)
                sliderEnd.End();
        }

        public override void DisposeResources()
        {
            path.Dispose();
            sliderStart.DisposeResources();
            Ball.DisposeResources();
            CircleNumbers.Dispose();
            if (sliderEnd != null)
                sliderEnd.DisposeResources();
        }

        public void Start() {
            if (started) return;
            started = true;
            startApproach = true;
            
            path.FadeInFromZero(HitObject.ObjectTiming.FadeTime);

            sliderStart.Start();
            CircleNumbers.FadeInFromZero(HitObject.ObjectTiming.FadeTime);

            if (sliderEnd != null)
                sliderEnd.Start();

            /*
            Logger.Log(" ======== slider info start ======== ");
            if (HitObject.Timing != null)
            {
                Logger.Log("EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * (float)Path.GetLength() / (beatmap.GetInfo().Difficulty.SliderMultiplier * (-100/HitObject.Timing.BeatLength))) * (beatmap.GetInfo().Timing.BPM_At(Time)/100))");
                Logger.Log(HitObject.EndTime + " = " + HitObject.Time + " + (int)((" + HitObject.SliderInformation.SlideRepeat + "+1) * (float)" + HitObject.Path.GetLength() + " / (" + ParentMap.GetInfo().Difficulty.SliderMultiplier + " * (-100/" + HitObject.Timing.BeatLength + "))) * (" + ParentMap.GetInfo().Timing.BPM_At(HitObject.Time) + "/100))");
            }
            else
            {
                Logger.Log("EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * (float)Path.GetLength() / beatmap.GetInfo().Difficulty.SliderMultiplier) * (beatmap.GetInfo().Timing.BPM_At(Time)/100))");
                Logger.Log(HitObject.EndTime + " = " + HitObject.Time + " + (int)((" + HitObject.SliderInformation.SlideRepeat + "+1) * (float)" + HitObject.Path.GetLength() + " / " + ParentMap.GetInfo().Difficulty.SliderMultiplier + ") * (" + ParentMap.GetInfo().Timing.BPM_At(HitObject.Time) + "/100))");
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
                currentDelayTime += Clock.ElapsedFrameTime;
            }
            else
            {
                Start();
            }
            

            if (startApproach && !approachEnded)
            {
                currentTime += Clock.ElapsedFrameTime;

                if (currentTime >= HitObject.ObjectTiming.TotalVisibleTime)
                {
                    startApproach = false;
                    approachEnded = true;
                    currentTime = 0f;

                    sliderStart.EndHitcircle();
                    CircleNumbers.Alpha = 0;
                    Ball.Start();
                }
            }

            if (approachEnded)
            {
                currentTime += Clock.ElapsedFrameTime;
                double progress = currentTime / (Duration);
                progress = Math.Clamp(progress, 0, 1);
                
                Ball.Update(progress);

                sliderStart.Update(Clock.ElapsedFrameTime);
                if (sliderEnd != null)
                    sliderEnd.Update(Clock.ElapsedFrameTime);
                
                if (currentTime >= Duration)
                {
                    circleEnded = true;
                    ParentMap.RemoveObject(this);
                }
            }
        }
    }
}