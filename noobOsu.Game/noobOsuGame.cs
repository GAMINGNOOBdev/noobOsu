using osuTK;
using noobOsu.Game.Screens;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Allocation;

namespace noobOsu.Game
{
    public partial class noobOsuGame : noobOsuGameBase
    {
        public static new noobOsuGame INSTANCE { get; private set; }
        public static DiscordRichPresence UserRichPresence { get; private set; }
        public ScreenStack ScreenStack { get; private set; }

        public static void UpdateRichPresence(IBeatmapGeneral map)
        {
            if (map != null)
            {
                UserRichPresence.Presence.Details = "Playing " + map.InterpretName + " - " + map.SongName + " (" + map.MapperName + ") [" + map.DifficultyName + "]";
                UserRichPresence.Presence.State = "Clicking circles";
            }
            else
            {
                UserRichPresence.Presence.Details = null;
                UserRichPresence.Presence.State = null;
            }
            UserRichPresence.UpdatePresence();
        }

        public noobOsuGame() : base()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(ScreenStack = new ScreenStack { RelativeSizeAxes = Axes.Both });
            Add(UserRichPresence = new DiscordRichPresence());
            
            GlobalGameCursor.DrawCursor = true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            // start off with the map select, the next screens will be created and pushed after finishing then exited and disposed by the map select screen
            ScreenStack.Push(new MapSelectScreen());
        }

        protected override void Dispose(bool isDisposing)
        {
            UserRichPresence.DisposePresence();
            UserRichPresence.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
