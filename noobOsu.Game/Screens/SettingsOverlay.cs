using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using noobOsu.Game.Settings;
using osu.Framework.Logging;
using osu.Framework.Screens;
using noobOsu.Game.UI.Basic;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using noobOsu.Game.Skins.Drawables;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class SettingsOverlay : CompositeDrawable
    {
        private InteractableBox exitScreenBox, box;
        private Container settingsFlyout;
        private SkinSelectDropdown SkinSelect;
        private BasicCheckbox useBeatmapSkin, useBeatmapHitsound, useBeatmapColor;
        private BasicSliderBar<int> beatmapBgDim;
        private BasicSliderBar<int> masterVolume;
        private BasicSliderBar<int> effectVolume;
        private BasicSliderBar<int> musicVolume;
        private SpriteText mapbgdimInfo;
        private BasicButton backButton;

        public SettingsOverlay()
        {
            exitScreenBox = new InteractableBox()
            {
                Colour = new Color4(0.2f, 0.2f, 0.2f, 0.3f),
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(1),
                Depth = 10,
            };
            exitScreenBox.ClickAction = (e) => {
                this.Exit();
            };

            settingsFlyout = new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Depth = 0,
            };

            box = new InteractableBox()
            {
                Colour = new Color4(0.1f, 0.1f, 0.1f, 1f),
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(500, 1)
            };

            SkinSelect = new SkinSelectDropdown()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 10),
            };
            SkinSelect.Current.BindTo(GameSettings.CurrentSkin);

            foreach (ISkin skin in GameSettings.Skins)
            {
                SkinSelect.AddSkin(skin.DirectoryName, skin);
                if (skin.DirectoryName.Equals(GameSettings.LastSkinName))
                    SkinSelect.Current.Value = skin;
            }

            useBeatmapSkin = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 40),
            };
            useBeatmapSkin.LabelText = "Use Beatmap Skins";
            useBeatmapSkin.Current.BindTo(GameSettings.UseBeatmapSkins);

            useBeatmapColor = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 70),
            };
            useBeatmapColor.LabelText = "Use Beatmap Colors";
            useBeatmapColor.Current.BindTo(GameSettings.UseBeatmapColors);

            useBeatmapHitsound = new BasicCheckbox()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 100),
            };
            useBeatmapHitsound.LabelText = "Use Beatmap Hitsounds";
            useBeatmapHitsound.Current.BindTo(GameSettings.UseBeatmapHitsounds);

            mapbgdimInfo = new SpriteText()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 130),
                Font = FontUsage.Default.With(size: 30),
            };

            beatmapBgDim = new BasicSliderBar<int>()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 160),
                Size = new Vector2(500, 30),
            };
            beatmapBgDim.Current.BindTo(GameSettings.BeatmapBackgroundDim);
            beatmapBgDim.Current.BindValueChanged((val) => {
                mapbgdimInfo.Text = "Background dim: " + val.NewValue + "%";
            }, true);
            
            masterVolume = new BasicSliderBar<int>()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 200),
                Size = new Vector2(500, 30),
            };
            masterVolume.Current.BindTo(GameSettings.MasterVolume);

            musicVolume = new BasicSliderBar<int>()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 240),
                Size = new Vector2(500, 30),
            };
            musicVolume.Current.BindTo(GameSettings.MusicVolume);

            effectVolume = new BasicSliderBar<int>()
            {
                Origin = Anchor.TopLeft,
                Position = new Vector2(0, 280),
                Size = new Vector2(500, 30),
            };
            effectVolume.Current.BindTo(GameSettings.EffectVolume);

            backButton = new BasicButton()
            {
                Origin = Anchor.BottomLeft,
                RelativePositionAxes = Axes.Y,
                Y = 1f,
                Text = "Back",
                Size = new Vector2(20 * "Back".Length, 20),
                Scale = new Vector2(2f),
                Action = () => {
                    this.Exit();
                },
            };

            settingsFlyout.Add(box);
            settingsFlyout.Add(useBeatmapSkin);
            settingsFlyout.Add(useBeatmapColor);
            settingsFlyout.Add(useBeatmapHitsound);
            settingsFlyout.Add(mapbgdimInfo);
            settingsFlyout.Add(beatmapBgDim);
            settingsFlyout.Add(masterVolume);
            settingsFlyout.Add(musicVolume);
            settingsFlyout.Add(effectVolume);
            settingsFlyout.Add(backButton);
            settingsFlyout.Add(SkinSelect);
            AddInternal(exitScreenBox);
            AddInternal(settingsFlyout);

            settingsFlyout.Position = new Vector2(-box.Size.X, Position.Y);
            RelativeSizeAxes = Axes.Both;
            Depth = -2;

            exitScreenBox.Hide();
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
                this.Exit();
        }

        public void Exit()
        {
            this.FadeOutFromOne(200);
            settingsFlyout.MoveToX(-box.Size.X, 200);
            exitScreenBox.Hide();
        }

        public void Enter()
        {
            this.FadeInFromZero(200);
            settingsFlyout.MoveToX(0, 200);
            exitScreenBox.Show();
        }
    }
}