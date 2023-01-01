using osuTK;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.Screens
{
    public partial class MainScreen : Screen
    {
        public static MainScreen INSTANCE { get; private set; }
        private readonly DrawSizePreservingFillContainer contents = new DrawSizePreservingFillContainer();
        private Beatmap beatmap;
        private DrawableBeatmap drawableBeatmap;
        private string beatmapPath;

        public MainScreen(string path = "Songs/mymap/mymap.osu")
        {
            if (INSTANCE != null) return;
            INSTANCE = this;

            beatmapPath = path;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            contents.Strategy = DrawSizePreservationStrategy.Minimum;
            contents.TargetDrawSize = new Vector2(640, 513);
            AddInternal(contents);

            beatmap = new Beatmap(beatmapPath);
            drawableBeatmap = new DrawableBeatmap(beatmap);
            drawableBeatmap.SetDrawContainer(contents);
            drawableBeatmap.Load(noobOsuAudioManager.INSTANCE);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            drawableBeatmap?.Update();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            drawableBeatmap.Dispose();
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            drawableBeatmap.StartBeatmap(Scheduler);
        }

        private void ChangeBeatmap(string beatmapName)
        {
            drawableBeatmap.LoadBeatmap(beatmapName);
            drawableBeatmap.Load(noobOsuAudioManager.INSTANCE);
        }
    }
}
