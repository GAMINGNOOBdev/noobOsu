using osuTK;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using noobOsu.Game.Skins.Drawables;
using noobOsu.Game.Skins.Properties;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.HitObjects
{
    public partial class SliderStartCircle : CompositeDrawable
    {   
        private SkinnableSprite approachCircle, hitcircleArea, hitcircleOverlay, sliderEndCircle, reverseArrow;
        private double totalVisibleTime, fadeTime, CurrentTime = 0;
        private double waitingTime, hitWindow;
        private Slider ParentSlider;
        private bool HitcircleEnded = false, Ended = false, Started = false;
        private int RepeatMax, LastRepeatTime = 0;
        
        public bool HasReverseArrow { get; private set; }
 
        public SliderStartCircle(Slider parent)
        {
            ParentSlider = parent;

            // since this is the start of the slider, we subtract the repeat count by one
            RepeatMax = parent.HitObject.SliderInformation.SlideRepeat - 1;
            HasReverseArrow = RepeatMax > 0;

            if (HasReverseArrow)
                LastRepeatTime = parent.HitObject.SliderInformation.GetRepeatTimingInfo().ForSliderStart();

            totalVisibleTime = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.AR, 1800f, 1200f, 450f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.AR, 1200f, 800f, 300f);
            hitWindow = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.OD, 80, 50, 20);
            waitingTime = ParentSlider.HitObject.Time - hitWindow;

            RelativePositionAxes = Axes.None;
            Position = new Vector2(0);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            approachCircle = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            approachCircle.Scale = new Vector2((float)ParentSlider.Radius*2 * 4f);
            approachCircle.Colour = ParentSlider.Color;
            ParentSlider.AddProperty(new SkinnableTextureProperty(approachCircle, "approachcircle"));

            hitcircleArea = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleArea.Scale = new Vector2((float)ParentSlider.Radius*2);
            hitcircleArea.Colour = ParentSlider.Color;
            ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleArea, "hitcircle", true));
            

            hitcircleOverlay = new SkinnableSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            hitcircleOverlay.Scale = new Vector2((float)ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(hitcircleOverlay, "hitcircleoverlay"));

            ParentSlider.AddProperty(new SkinnableHitcircleNumberProperty(ParentSlider, ParentSlider.ComboNumber, "numbers"));

            hitcircleArea.Alpha = 0f;
            hitcircleOverlay.Alpha = 0f;
            approachCircle.Alpha = 0f;
            
            AddInternal(hitcircleArea);
            AddInternal(hitcircleOverlay);
            AddInternal(approachCircle);
            if (HasReverseArrow)
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

                Rotation = (float)ParentSlider.HitObject.Path.GetFirstAngle();

                AddInternal(sliderEndCircle);
                AddInternal(reverseArrow);
            }
        }

        public void Update(double delta)
        {
            if (HasReverseArrow)
            {
                CurrentTime += delta;
                if (CurrentTime >= LastRepeatTime)
                {
                    End();
                }
            }
        }

        public void Start()
        {
            if (Started) return;
            Started = true;
            approachCircle.ScaleTo((float)ParentSlider.Radius*2 * (float)approachCircle.ScaleFactor, totalVisibleTime);
            approachCircle.FadeInFromZero(fadeTime);
            hitcircleArea.FadeInFromZero(fadeTime);
            hitcircleOverlay.FadeInFromZero(fadeTime);
        }

        public void End()
        {
            if (Ended) return;
            Ended = true;
            
            if (HasReverseArrow)
            {
                sliderEndCircle.FadeOutFromOne(200);
                reverseArrow.FadeOutFromOne(200);
            }
        }

        public void DisposeResources()
        {
            hitcircleArea.Dispose();
            hitcircleOverlay.Dispose();
            approachCircle.Dispose();
            if (HasReverseArrow)
            {
                sliderEndCircle.Dispose();
                reverseArrow.Dispose();
            }
        }

        public void EndHitcircle()
        {
            if (HitcircleEnded) return;
            HitcircleEnded = true;
            hitcircleArea.FadeOutFromOne(200);
            hitcircleOverlay.FadeOutFromOne(200);
            hitcircleArea.ScaleTo((float)ParentSlider.Radius*2 * 1.5f * (float)hitcircleArea.ScaleFactor, 200);
            hitcircleOverlay.ScaleTo((float)ParentSlider.Radius*2 * 1.5f * (float)hitcircleOverlay.ScaleFactor, 200);
            approachCircle.Alpha = 0f;

            if (ParentSlider.Sample != null)
                ParentSlider.Sample.Play();
            
            if (HasReverseArrow)
            {
                sliderEndCircle.FadeInFromZero(fadeTime);
                reverseArrow.FadeInFromZero(fadeTime);
            }
        }
    }
}