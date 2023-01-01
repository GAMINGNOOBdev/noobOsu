using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using noobOsu.Game.Screens;

namespace noobOsu.Game
{
    public partial class noobOsuGame : noobOsuGameBase
    {
        public static noobOsuGame INSTANCE { get; private set; }
        public ScreenStack ScreenStack { get; private set; }

        public noobOsuGame() : base()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = ScreenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            // start off with the map select, the next screens will be created and pushed after finishing then exited and disposed by the map select screen
            ScreenStack.Push(new MapSelectScreen());
        }
    }
}
