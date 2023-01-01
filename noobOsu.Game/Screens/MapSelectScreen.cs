using System.IO;
using osuTK.Input;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using System.Collections.Generic;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class MapSelectScreen : Screen
    {
        public static MapSelectScreen INSTANCE { get; private set; }

        private readonly List<Button> beatmapsSelect = new List<Button>();
        private string SelectedSongDifficulty => SelectedSongFolder;
        private string SelectedSongFolder = "mymap";

        public MapSelectScreen()
        {
            if (INSTANCE != null) return;   
            INSTANCE = this;
        }
        
        [BackgroundDependencyLoader]
        private void load()
        {
            int yOffset = 20;

            foreach (string s in Directory.EnumerateDirectories("Songs/", "*", SearchOption.TopDirectoryOnly))
            {
                string songName = s.Substring("Songs/".Length);
                BasicButton b = new BasicButton();
                b.Text = songName;
                b.Y = yOffset;
                b.X = 200;
                b.Size = new osuTK.Vector2(20 * songName.Length, 20);
                b.Action = () => {
                    SelectedSongFolder = songName;
                    LoadMap();
                };
                yOffset += 30;
                beatmapsSelect.Add(b);
            }

            foreach (Button b in beatmapsSelect)
            {
                AddInternal(b);
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        private void LoadMap()
        {
            MainScreen ms = new MainScreen();
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