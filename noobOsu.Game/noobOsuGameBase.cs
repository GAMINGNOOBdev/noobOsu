using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using noobOsu.Resources;
using noobOsu.Game.Stores;

namespace noobOsu.Game
{
    public partial class noobOsuGameBase : osu.Framework.Game
    {
        protected override Container<Drawable> Content { get; }

        private ExternalAssetStore ExternalSongTracks, ExternalSongSamples;
        private ResourceStore<byte[]> tracksContianer, samplesContainer;
        private noobOsuAudioManager clientAudioManager;

        protected noobOsuGameBase()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768)
            });
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

            Host.Window.Title = "noobOsu!";
        }
    }
}
