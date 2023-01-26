using osuTK;
using noobOsu.Game.Skins;
using noobOsu.Game.Stores;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class BeatmapRenderScreen : Screen
    {
        public static BeatmapRenderScreen INSTANCE { get; private set; }
        private readonly DrawSizePreservingFillContainer contents = new DrawSizePreservingFillContainer();
        private ISkin SelectedSkin;
        private Beatmap beatmap;
        private DrawableBeatmap drawableBeatmap;
        private BasicButton exitBeatmapButton;
        private string beatmapPath;

        public BeatmapRenderScreen()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;

            SelectedSkin = Settings.GameSettings.GetCurrentSkin();
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
            SelectedSkin = Settings.GameSettings.GetCurrentSkin();

            contents.Strategy = DrawSizePreservationStrategy.Minimum;
            contents.TargetDrawSize = new Vector2(640, 513);
            contents.BypassAutoSizeAxes = Axes.Y;
            AddInternal(contents);

            exitBeatmapButton = new BasicButton()
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
            AddInternal(exitBeatmapButton);

            SkinFontStore.SetCurrentSkin(SelectedSkin);

            beatmap = new Beatmap(beatmapPath);
            drawableBeatmap = new DrawableBeatmap(beatmap, SelectedSkin);
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
