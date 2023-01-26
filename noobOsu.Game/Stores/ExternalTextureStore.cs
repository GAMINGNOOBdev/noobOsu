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
        private readonly List<TextureUpload> textures = new List<TextureUpload>();

        public void Dispose() {
            foreach (TextureUpload texture in textures)
            {
                texture.Dispose();
            }
            foreach (Stream file in filestreams)
            {
                file.Dispose();
                file.Close();
            }
        }

        public virtual TextureUpload Get(string name)
        {
            if (filenames.Contains(name))
            {
                return textures[ filenames.IndexOf(name) ];
            }
            
            TextureUpload texture = new TextureUpload(GetStream(name));
            textures.Add(texture);

            return texture;
        }

        public Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default) => null;

        public IEnumerable<string> GetAvailableResources() => filenames.AsEnumerable();

        public Stream GetStream(string name)
        {
            if (filenames.Contains(name))
            {
                return filestreams[filenames.IndexOf(name)];
            }
            
            Stream file = File.OpenRead(name);
            
            filenames.Add(name);
            filestreams.Add(file);
            
            return file;
        }
    }
}