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
    public partial class SliderStartCircle : CompositeDrawable
    {   
        private HitObjectSprite approachCircle, hitcircleArea, hitcircleOverlay, sliderEndCircle, reverseArrow;
        private float totalVisibleTime, fadeTime;
        private float waitingTime, hitWindow;
        private Slider ParentSlider;

        public float Radius { get; private set; }
        
        public bool IsReverseArrow { get; private set; }
 
        public SliderStartCircle(Slider parent, Vector2 circlePosition, bool reverseArrow = false)
        {
            ParentSlider = parent;
            IsReverseArrow = reverseArrow;

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
            if (!IsReverseArrow)
            {
                approachCircle = new HitObjectSprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                approachCircle.Scale = new Vector2(ParentSlider.Radius*2 * 4f);
                approachCircle.Colour = ParentSlider.Color;
                ParentSlider.AddProperty(new SkinnableTextureProperty(approachCircle, "approachcircle"));

                hitcircleArea = new HitObjectSprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                hitcircleArea.Scale = new Vector2(ParentSlider.Radius*2);
                hitcircleArea.Colour = ParentSlider.Color;
                ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleArea, "hitcircle", true));
                

                hitcircleOverlay = new HitObjectSprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                hitcircleOverlay.Scale = new Vector2(ParentSlider.Radius*2);
                ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleOverlay, "hitcircleoverlay"));


                hitcircleArea.Alpha = 0f;
                hitcircleOverlay.Alpha = 0f;
                approachCircle.Alpha = 0f;
                
                AddInternal(hitcircleArea);
                AddInternal(hitcircleOverlay);
                AddInternal(approachCircle);
            }
            else
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

                //Logger.Log("ParentSlider.HitObject.Path.GetLastAngle() = " + ParentSlider.HitObject.Path.GetLastAngle());
                Rotation = ParentSlider.HitObject.Path.GetLastAngle();

                AddInternal(sliderEndCircle);
                AddInternal(reverseArrow);
            }
        }

        public void Start()
        {
            if (!IsReverseArrow)
            {
                approachCircle.ScaleTo(ParentSlider.Radius*2 * approachCircle.ScaleFactor, totalVisibleTime);
                approachCircle.FadeInFromZero(fadeTime);
                hitcircleArea.FadeInFromZero(fadeTime);
                hitcircleOverlay.FadeInFromZero(fadeTime);
            }
            else
            {
                sliderEndCircle.FadeInFromZero(fadeTime);
                reverseArrow.FadeInFromZero(fadeTime);
            }
        }

        public void End()
        {
            if (!IsReverseArrow)
            {
                hitcircleArea.FadeOutFromOne(200);
                hitcircleOverlay.FadeOutFromOne(200);
                hitcircleArea.ScaleTo(ParentSlider.Radius*2 * 1.5f * hitcircleArea.ScaleFactor, 200);
                hitcircleOverlay.ScaleTo(ParentSlider.Radius*2 * 1.5f * hitcircleOverlay.ScaleFactor, 200);
                approachCircle.Alpha = 0f;
            }
            else
            {
                sliderEndCircle.FadeOutFromOne(200);
                reverseArrow.FadeOutFromOne(200);
            }
        }

        public void DisposeResources()
        {
            if (!IsReverseArrow)
            {
                hitcircleArea.Dispose();
                hitcircleOverlay.Dispose();
                approachCircle.Dispose();
            }
            else
            {
                sliderEndCircle.Dispose();
                reverseArrow.Dispose();
            }
        }
    }
}