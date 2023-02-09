using osuTK;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using noobOsu.Game.Skins.Drawables;
using noobOsu.Game.Skins.Properties;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.HitObjects
{
    public partial class SliderEndCircle : CompositeDrawable
    {   
        private SkinnableSprite sliderEndCircle, reverseArrow;
        private double totalVisibleTime, fadeTime, CurrentTime = 0;
        private double waitingTime, hitWindow;
        private int RepeatMax, LastRepeatTime = 0;
        private bool Started, Ended;
        private Slider ParentSlider;

        public SliderEndCircle(Slider parent, Vector2 circlePosition)
        {
            ParentSlider = parent;
            RepeatMax = parent.HitObject.SliderInformation.SlideRepeat;
            LastRepeatTime = parent.HitObject.SliderInformation.GetRepeatTimingInfo().ForSliderEnd();

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.OD, 80, 50, 20);
            waitingTime = ParentSlider.HitObject.Time - hitWindow;

            RelativePositionAxes = Axes.None;
            Position = circlePosition;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sliderEndCircle = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            sliderEndCircle.Scale = new Vector2((float)ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(sliderEndCircle, "sliderendcircle"));

            reverseArrow = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            reverseArrow.Scale = new Vector2((float)ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(reverseArrow, "reversearrow"));

            sliderEndCircle.Alpha = 0f;
            reverseArrow.Alpha = 0f;

            Rotation = (float)ParentSlider.HitObject.Path.GetLastAngle();

            AddInternal(sliderEndCircle);
            AddInternal(reverseArrow);
        }

        public void Update(double delta)
        {
            CurrentTime += delta;
            if (CurrentTime >= LastRepeatTime)
            {
                End();
            }
        }

        public void Start()
        {
            if (Started) return;
            Started = true;
            sliderEndCircle.FadeInFromZero(fadeTime);
            reverseArrow.FadeInFromZero(fadeTime);
        }

        public void End()
        {
            if (Ended) return;
            Ended = true;
            if (ParentSlider.Sample != null)
                ParentSlider.Sample.Play();
            sliderEndCircle.FadeOutFromOne(200);
            reverseArrow.FadeOutFromOne(200);
        }

        public void DisposeResources()
        {
            sliderEndCircle.Dispose();
            reverseArrow.Dispose();
        }
    }
}