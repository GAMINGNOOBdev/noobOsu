using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using noobOsu.Game.Settings;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using noobOsu.Game.Skins.Drawables;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class SettingsScreen : Screen
    {
        private Box box;
        private SkinSelectDropdown SkinSelect;
        private BasicCheckbox useBeatmapSkin, useBeatmapHitsound, useBeatmapColor;
        private BasicButton backButton;

        public SettingsScreen() {}

        [BackgroundDependencyLoader]
        private void load()
        {
            if (GameSettings.INSTANCE == null) new GameSettings();

            box = new Box()
            {
                Colour = Color4.Red,
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(500, Size.Y)
            };

            SkinSelect = new SkinSelectDropdown()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 10),
            };
            SkinSelect.Current.BindTo(GameSettings.INSTANCE.CurrentSkin);

            foreach (ISkin skin in GameSettings.Skins)
            {
                SkinSelect.AddSkin(skin.DirectoryName, skin);
                if (skin.DirectoryName.Equals(GameSettings.INSTANCE.LastSkinName))
                    SkinSelect.Current.Value = skin;
            }

            useBeatmapSkin = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 40),
            };
            useBeatmapSkin.LabelText = "Use Beatmap Skins";
            useBeatmapSkin.Current.BindTo(GameSettings.INSTANCE.UseBeatmapSkins);

            useBeatmapColor = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 70),
            };
            useBeatmapColor.LabelText = "Use Beatmap Colors";
            useBeatmapColor.Current.BindTo(GameSettings.INSTANCE.UseBeatmapColors);

            useBeatmapHitsound = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 100),
            };
            useBeatmapHitsound.LabelText = "Use Beatmap Hitsounds";
            useBeatmapHitsound.Current.BindTo(GameSettings.INSTANCE.UseBeatmapHitsounds);

            backButton = new BasicButton()
            {
                Origin = Anchor.BottomLeft,
                RelativePositionAxes = Axes.Y,
                Y = 1f,
                Text = "Back",
                Size = new Vector2(20 * "Back".Length, 20),
                Action = () => {
                    this.Exit();
                },
            };

            AddInternal(box);
            AddInternal(useBeatmapSkin);
            AddInternal(useBeatmapColor);
            AddInternal(useBeatmapHitsound);
            AddInternal(backButton);
            AddInternal(SkinSelect);

            Position = new Vector2(-box.Size.X, Position.Y);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
                this.Exit();
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