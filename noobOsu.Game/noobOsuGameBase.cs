using osuTK;
using noobOsu.Game.UI;
using noobOsu.Resources;
using noobOsu.Game.Stores;
using noobOsu.Game.Settings;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game
{
    public partial class noobOsuGameBase : osu.Framework.Game
    {
        protected override Container<Drawable> Content { get; }

        private ExternalAssetStore ExternalSongTracks, ExternalSongSamples;
        private ResourceStore<byte[]> tracksContianer, samplesContainer;
        private noobOsuAudioManager clientAudioManager;
        private GameSettings Settings;

        public static GameCursorContainer GlobalGameCursor;

        protected noobOsuGameBase()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768)
            });

            Settings = new GameSettings();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(noobOsuResources).Assembly));

            ExternalSongSamples = new ExternalAssetStore();
            ExternalSongTracks = new ExternalAssetStore();

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

            Host.Window.Title = "noobOsu";
            Host.Window.CursorState |= osu.Framework.Platform.CursorState.Hidden;
        }

        protected override bool OnExiting()
        {
            Settings.Dispose();
            return base.OnExiting();
        }
    }
}
