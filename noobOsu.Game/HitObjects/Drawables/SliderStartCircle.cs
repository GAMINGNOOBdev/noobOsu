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
        private Sprite approachCircle, hitcircleArea, hitcircleOverlay, sliderEndCircle, reverseArrow;
        private float totalVisibleTime, fadeTime, radius;
        private float waitingTime, hitWindow;
        private Slider ParentSlider;
        
        public bool IsReverseArrow { get; private set; }
 
        public SliderStartCircle(Slider parent, Vector2 circlePosition, bool reverseArrow = false)
        {
            ParentSlider = parent;
            IsReverseArrow = reverseArrow;
            
            radius = 64f * ((1.0f - 0.7f * (ParentSlider.ParentMap.GetInfo().Difficulty.CS - 5f) / 5f) / 2f);

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
                approachCircle = new Sprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                approachCircle.Scale = new Vector2(radius*2 * 4f);
                approachCircle.Colour = ParentSlider.Color;
                ParentSlider.AddProperty(new SkinnableTextureProperty(approachCircle, "approachcircle.png"));

                hitcircleArea = new Sprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                hitcircleArea.Scale = new Vector2(radius*2);
                hitcircleArea.Colour = ParentSlider.Color;
                ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleArea, "hitcircle.png"));
                

                hitcircleOverlay = new Sprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                hitcircleOverlay.Scale = new Vector2(radius*2);
                ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleOverlay, "hitcircleoverlay.png"));


                hitcircleArea.Alpha = 0f;
                hitcircleOverlay.Alpha = 0f;
                approachCircle.Alpha = 0f;
                
                AddInternal(hitcircleArea);
                AddInternal(hitcircleOverlay);
                AddInternal(approachCircle);
            }
            else
            {
                sliderEndCircle = new Sprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                sliderEndCircle.Scale = new Vector2(radius*2);
                ParentSlider.AddProperty(new SkinnableTextureProperty(sliderEndCircle, "sliderendcircle.png"));

                reverseArrow = new Sprite(){
                    RelativeSizeAxes = Axes.None,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(1f),
                };
                reverseArrow.Scale = new Vector2(radius*2);
                ParentSlider.AddProperty(new SkinnableTextureProperty(reverseArrow, "reversearrow.png"));

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
                approachCircle.ScaleTo(radius*2, totalVisibleTime);
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
                hitcircleArea.ScaleTo(radius*2 * 1.5f, 200);
                hitcircleOverlay.ScaleTo(radius*2 * 1.5f, 200);
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