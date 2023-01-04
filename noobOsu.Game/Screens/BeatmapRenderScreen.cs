using osuTK;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Logging;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.Screens
{
    public partial class BeatmapRenderScreen : Screen
    {
        public static BeatmapRenderScreen INSTANCE { get; private set; }
        private readonly DrawSizePreservingFillContainer contents = new DrawSizePreservingFillContainer();
        private Beatmap beatmap;
        private DrawableBeatmap drawableBeatmap;
        private string beatmapPath;

        public BeatmapRenderScreen()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }

        public void SetBeatmapPath(string path)
        {
            if (path != null)
            {
                beatmapPath = path;
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (beatmapPath == null)
            {
                throw new System.Exception("beatmap path not specified");
            }
            contents.Strategy = DrawSizePreservationStrategy.Minimum;
            contents.TargetDrawSize = new Vector2(640, 513);
            AddInternal(contents);

            beatmap = new Beatmap(beatmapPath);
            drawableBeatmap = new DrawableBeatmap(beatmap);
            drawableBeatmap.SetDrawContainer(contents);
            drawableBeatmap.Load(noobOsuAudioManager.INSTANCE, textures);

            drawableBeatmap.StartBeatmap(Scheduler);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            drawableBeatmap?.Update();
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.ControlPressed && e.Key == osuTK.Input.Key.Q)
            {
                this.Exit();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            drawableBeatmap.Dispose();
        }
    }
}