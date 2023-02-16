using System.IO;
using osuTK.Input;
using noobOsu.Game.UI;
using noobOsu.Game.Beatmaps;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using noobOsu.Game.UI.Beatmap;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class MapSelectScreen : Screen
    {
        public static MapSelectScreen INSTANCE { get; private set; }

        private BeatmapScrollSelect beatmapSelect;
        private BasicButton settingsButton;
        private SettingsOverlay settingsScreen;

        public MapSelectScreen()
        {
            if (INSTANCE != null) return;   
            INSTANCE = this;
        }
        
        [BackgroundDependencyLoader]
        private void load()
        {
            settingsScreen = new SettingsOverlay();

            settingsButton = new BasicButton();
            settingsButton.Text = "Settings";
            settingsButton.Anchor = Anchor.BottomLeft;
            settingsButton.Origin = Anchor.BottomLeft;
            settingsButton.Size = new osuTK.Vector2(20 * "Settings".Length, 20);
            settingsButton.Scale = new osuTK.Vector2(2f);
            settingsButton.Action = () => {
                settingsScreen.Enter();
            };

            beatmapSelect = new BeatmapScrollSelect(){
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Size = new osuTK.Vector2(0.5f, 1f),
                X = 0.5f,
                Y = 0f,
            };
            foreach (string s in Directory.EnumerateDirectories("Songs/", "*", SearchOption.TopDirectoryOnly))
            {
                string songName = s.Substring("Songs/".Length);
                if (char.IsDigit(songName[0]))
                    beatmapSelect.AddBeatmap(songName);
            }
            beatmapSelect.FinishAdding();
            beatmapSelect.SelectRandom();

            AddInternal(settingsButton);
            AddInternal(settingsScreen);
            AddInternal(beatmapSelect);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        public void LoadMap(IBeatmapGeneral map, bool debug)
        {
            if (!debug)
            {
                BeatmapRenderScreen ms = new BeatmapRenderScreen();
                ms.SetMap(map);
                noobOsuGame.INSTANCE.ScreenStack.Push(ms);
            }
            else
            {
                BeatmapDebugInfoScreen ms = new BeatmapDebugInfoScreen();
                ms.SetMap(map);
                noobOsuGame.INSTANCE.ScreenStack.Push(ms);
            }
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.Key == Key.O && e.ControlPressed)
                settingsScreen.Enter();
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
        }

        private void ToThisScreen()
        {
            this.FadeInFromZero(500);
        }
    }
}