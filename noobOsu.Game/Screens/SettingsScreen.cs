using osuTK;
using osuTK.Graphics;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;

namespace noobOsu.Game.Screens
{
    public partial class SettingsScreen : Screen
    {
        private Box box;

        public SettingsScreen() {}

        [BackgroundDependencyLoader]
        private void load()
        {
            box = new Box()
            {
                Colour = Color4.Red,
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(500, Size.Y)
            };
            AddInternal(box);

            Position = new Vector2(-box.Size.X, Position.Y);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            this.Exit();

            return base.OnMouseDown(e);
        }

        public override void OnSuspending(ScreenTransitionEvent e) => ToNextScreen(); 
        public override bool OnExiting(ScreenExitEvent e)
        {
            ToNextScreen();
            return base.OnExiting(e);
        }

        public override void OnResuming(ScreenTransitionEvent e) => ToThisScreen();
        public override void OnEntering(ScreenTransitionEvent e) => ToThisScreen();

        private void ToNextScreen()
        {
            this.FadeOutFromOne(500);
            this.MoveToX(-box.Size.X, 500);
        }

        private void ToThisScreen()
        {
            this.FadeInFromZero(500);
            this.MoveToX(0, 500);
        }
    }
}