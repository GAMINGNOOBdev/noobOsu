using osuTK;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.HitObjects
{
    public partial class SliderEndCircle : CompositeDrawable
    {   
        private HitObjectSprite sliderEndCircle, reverseArrow;
        private float totalVisibleTime, fadeTime;
        private float waitingTime, hitWindow;
        private Slider ParentSlider;

        public SliderEndCircle(Slider parent, Vector2 circlePosition)
        {
            ParentSlider = parent;

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
            sliderEndCircle = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            sliderEndCircle.Scale = new Vector2(ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(sliderEndCircle, "sliderendcircle"));

            reverseArrow = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            reverseArrow.Scale = new Vector2(ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(reverseArrow, "reversearrow"));

            sliderEndCircle.Alpha = 0f;
            reverseArrow.Alpha = 0f;

            Rotation = ParentSlider.HitObject.Path.GetLastAngle();

            AddInternal(sliderEndCircle);
            AddInternal(reverseArrow);
        }

        public void Start()
        {
            sliderEndCircle.FadeInFromZero(fadeTime);
            reverseArrow.FadeInFromZero(fadeTime);
        }

        public void End()
        {
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