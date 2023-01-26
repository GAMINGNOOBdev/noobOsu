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

        private BeatmapDropdown dropdown;
        private BasicButton startMapButton;
        private BasicButton settingsButton;
        private string SelectedSongDifficulty => SelectedSongFolder;
        private string SelectedSongFolder;

        public MapSelectScreen()
        {
            if (INSTANCE != null) return;   
            INSTANCE = this;
        }
        
        [BackgroundDependencyLoader]
        private void load()
        {
            startMapButton = new BasicButton();
            startMapButton.Text = "Start Beatmap";
            startMapButton.X = 200;
            startMapButton.Y = 20;
            startMapButton.Size = new osuTK.Vector2(20 * "Start Beatmap".Length, 20);
            startMapButton.Action = () => {
                SelectedSongFolder = dropdown.Current.Value;
                if (!string.IsNullOrEmpty(SelectedSongFolder))
                    LoadMap();
            };

            settingsButton = new BasicButton();
            settingsButton.Text = "Settings";
            settingsButton.X = 200;
            settingsButton.Y = 50;
            settingsButton.Size = new osuTK.Vector2(20 * "Settings".Length, 20);
            settingsButton.Action = () => {
                noobOsuGame.INSTANCE.ScreenStack.Push(new SettingsScreen());
            };

            dropdown = new BeatmapDropdown();
            dropdown.X = 500;
            dropdown.Y = 20;
            foreach (string s in Directory.EnumerateDirectories("Songs/", "*", SearchOption.TopDirectoryOnly))
            {
                string songName = s.Substring("Songs/".Length);
                dropdown.AddMap(songName);
            }

            AddInternal(startMapButton);
            AddInternal(settingsButton);
            AddInternal(dropdown);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        private void LoadMap()
        {
            BeatmapRenderScreen ms = new BeatmapRenderScreen();
            ms.SetBeatmapPath("Songs/" + SelectedSongFolder + "/" + SelectedSongDifficulty + ".osu");
            noobOsuGame.INSTANCE.ScreenStack.Push(ms);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            
            if (e.Key == Key.O && e.ControlPressed)
                noobOsuGame.INSTANCE.ScreenStack.Push(new SettingsScreen());
            
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