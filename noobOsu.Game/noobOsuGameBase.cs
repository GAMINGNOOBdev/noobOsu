using osuTK;
using noobOsu.Game.UI;
using noobOsu.Resources;
using noobOsu.Game.Stores;
using osu.Framework.Timing;
using noobOsu.Game.Settings;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Allocation;
using noobOsu.Game.Skins.Drawables;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game
{
    public partial class noobOsuGameBase : osu.Framework.Game
    {
        protected override Container<Drawable> Content { get; }

        private ExternalAudioStore ExternalSongTracks, ExternalSongSamples;
        private ResourceStore<byte[]> tracksContianer, samplesContainer;
        private noobOsuAudioManager clientAudioManager;

        public static GameCursorContainer GlobalGameCursor;
        public static SkinCursorContainer GlobalSkinCursor;

        protected noobOsuGameBase()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768)
            });

            GameSettings.Init();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(noobOsuResources).Assembly));

            ExternalSongSamples = new ExternalAudioStore();
            ExternalSongTracks = new ExternalAudioStore();

            tracksContianer = new ResourceStore<byte[]>();
            samplesContainer = new ResourceStore<byte[]>();

            tracksContianer.AddStore(ExternalSongTracks);
            samplesContainer.AddStore(ExternalSongSamples);

            clientAudioManager = new noobOsuAudioManager(Host.AudioThread, tracksContianer, samplesContainer);

            Textures.AddTextureSource(new ExternalTextureStore());
            Fonts.AddTextureSource(new SkinFontStore());

            base.Content.Add(
                GlobalGameCursor = new GameCursorContainer(){
                    RelativeSizeAxes = Axes.Both,
                }
            );
            base.Content.Add(
                GlobalSkinCursor = new SkinCursorContainer(){
                    RelativeSizeAxes = Axes.Both,
                }
            );

            Host.Window.Title = "noobOsu";
            Host.Window.CursorState |= osu.Framework.Platform.CursorState.Hidden;
        }

        protected override bool OnExiting()
        {
            GameSettings.Dispose();
            return base.OnExiting();
        }
    }
}
