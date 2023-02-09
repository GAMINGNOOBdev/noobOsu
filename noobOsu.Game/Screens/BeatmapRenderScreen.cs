using osuTK;
using noobOsu.Game.Skins;
using noobOsu.Game.Stores;
using osu.Framework.Timing;
using noobOsu.Game.Graphics;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class BeatmapRenderScreen : Screen, IExternalCanAddChildren
    {
        public static BeatmapRenderScreen INSTANCE { get; private set; }
        private readonly DrawSizePreservingFillContainer contents = new DrawSizePreservingFillContainer();
        private ISkin SelectedSkin;
        private Beatmap beatmap;
        private DrawableBeatmap drawableBeatmap;
        private BasicButton exitBeatmapButton;
        private IBeatmapGeneral beatmapInfo;

        public BeatmapRenderScreen()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }

        public void AddChild(Drawable child)
        {
            AddInternal(child);
        }

        public void RemoveChild(Drawable child)
        {
            RemoveInternal(child, false);
        }

        public void SetMap(IBeatmapGeneral map)
        {
            beatmapInfo = map;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            SelectedSkin = Settings.GameSettings.GetCurrentSkin();

            contents.Strategy = DrawSizePreservationStrategy.Minimum;
            contents.TargetDrawSize = new Vector2(640, 512);
            contents.FillMode = FillMode.Fit;
            contents.FillAspectRatio = 4f/3f;
            contents.Origin = Anchor.Centre;
            contents.Anchor = Anchor.Centre;
            contents.RelativeSizeAxes = Axes.Both;
            AddInternal(contents);

            exitBeatmapButton = new BasicButton()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Text = "Back",
                Size = new Vector2(20 * "Back".Length, 20),
                Scale = new Vector2(2f),
                Action = () => {
                    this.Exit();
                },
            };
            AddInternal(exitBeatmapButton);

            SkinFontStore.SetCurrentSkin(SelectedSkin);

            noobOsuGame.GlobalGameCursor.DrawCursor = false;
            noobOsuGame.GlobalSkinCursor.SwitchSkin(SelectedSkin, textures);
            noobOsuGame.GlobalSkinCursor.DrawCursor = true;

            beatmap = new Beatmap(beatmapInfo);
            drawableBeatmap = new DrawableBeatmap(beatmap, this);
            drawableBeatmap.SetDrawContainer(contents);
            drawableBeatmap.Load(noobOsuAudioManager.INSTANCE, textures);

            drawableBeatmap.StartBeatmap(Scheduler);

            AddInternal(drawableBeatmap);

            Logger.Log("drawsize: " + contents.DrawSize + " size: " + contents.Size);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            noobOsuGame.UpdateRichPresence(beatmapInfo);
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
            SkinFontStore.SetCurrentSkin(null);
            noobOsuGame.UpdateRichPresence(null);
            noobOsuGame.GlobalGameCursor.DrawCursor = true;
            noobOsuGame.GlobalSkinCursor.DrawCursor = false;
        }
    }
}
