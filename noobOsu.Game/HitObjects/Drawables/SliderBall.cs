using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Util;
using osu.Framework.Utils;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.HitObjects
{
    public partial class SliderBall : CompositeDrawable
    {
        private readonly Slider ParentSlider;
        private HitObjectSprite Ball, FollowCircle;
        private Bindable<Color4> BallColor = new Bindable<Color4>();
        private Bindable<bool> RotateFollowBall = new Bindable<bool>(false);
        private double CurrentProgress;
        private int RepeatMax;
        private bool Started = false, Ended = false;
        private double fadeTime;
        private Vector2 LastPosition;

        public SliderBall(Slider parent)
        {
            ParentSlider = parent;
            RepeatMax = ParentSlider.HitObject.SliderInformation.SlideRepeat + 1;
            RelativePositionAxes = Axes.None;
            LastPosition = new Vector2(0.0f);
            fadeTime = BeatmapDifficulty.ScaleWithRange(ParentSlider.ParentMap.GetInfo().Difficulty.AR, 1200f, 800f, 300f);
        }
        
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Ball = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            Ball.Scale = new Vector2((float)ParentSlider.Radius*2);
            BallColor.BindValueChanged( (val) => {
                Ball.Colour = val.NewValue;
            });
            ParentSlider.AddProperty(new SkinnableColorProperty(BallColor, ParentSlider.Color, "SliderBall"));
            ParentSlider.AddProperty(new SkinnableTextureProperty(Ball, "sliderb"));

            FollowCircle = new HitObjectSprite(){
                RelativeSizeAxes = Axes.None,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(1f),
            };
            FollowCircle.Scale = new Vector2((float)ParentSlider.Radius*2);
            ParentSlider.AddProperty(new SkinnableTextureProperty(FollowCircle, "sliderfollowcircle"));

            ParentSlider.AddProperty(new SkinnableBoolProperty(RotateFollowBall, true, "SliderBallFlip"));

            Ball.Alpha = 0f;
            FollowCircle.Alpha = 0f;

            AddInternal(Ball);
            AddInternal(FollowCircle);
        }

        public void Start()
        {
            if (Started) return;
            Started = true;
            Ball.Alpha = 1f;
            FollowCircle.Alpha = 1f;
            FollowCircle.ScaleTo((float)ParentSlider.Radius*2 * 0.75f * (float)FollowCircle.ScaleFactor);
            FollowCircle.ScaleTo((float)ParentSlider.Radius*2 * (float)FollowCircle.ScaleFactor, fadeTime);
        }

        public void End()
        {
            if (Ended) return;
            Ended = true;
            
            Ball.Alpha = 0f;
            FollowCircle.FadeOutFromOne(200f);
            FollowCircle.ScaleTo((float)ParentSlider.Radius*2 * 0.75f * (float)FollowCircle.ScaleFactor, 200f);
        }

        public void DisposeResources()
        {
            Ball.Dispose();
            FollowCircle.Dispose();
        }

        public void Update(double progress)
        {   
            CurrentProgress = progress * RepeatMax % 1;

            if ((int)(progress * RepeatMax) % 2 == 1)
                CurrentProgress = 1 - CurrentProgress;

            Position = ParentSlider.HitObject.Path.GetProgressPoint( CurrentProgress );
            if (RotateFollowBall.Value)
                Ball.Rotation = (float)VectorUtil.GetAngleBetween(LastPosition, Position);
            LastPosition = Position;
        }
    }
}