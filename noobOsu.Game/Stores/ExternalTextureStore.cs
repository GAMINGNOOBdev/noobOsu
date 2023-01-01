using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace noobOsu.Game.Stores
{
    public partial class ExternalTextureStore : IResourceStore<TextureUpload>
    {
        private readonly List<Stream> filestreams = new List<Stream>();
        private readonly List<string> filenames = new List<string>();

        public void Dispose() {
            foreach (Stream file in filestreams)
            {
                file.Close();
            }
        }

        public TextureUpload Get(string name)
        {
            Stream file = GetStream(name);
            filestreams.Add(file);
            filenames.Add(name);
            return new TextureUpload(file);
        }

        public Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default) => null;

        public IEnumerable<string> GetAvailableResources() => filenames.AsEnumerable();

        public Stream GetStream(string name) => File.OpenRead(name);
    }
}