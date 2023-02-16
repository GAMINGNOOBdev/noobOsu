using System.IO;
using System.Linq;
using System.Threading;
using osu.Framework.Logging;
using System.Threading.Tasks;
using osu.Framework.IO.Stores;
using System.Collections.Generic;
using osu.Framework.Graphics.Textures;

namespace noobOsu.Game.Stores
{
    public partial class ExternalTextureStore : IResourceStore<TextureUpload>
    {
        private static string[] Extensions = new string[]{
            ".png",
            ".jpg",
            ".jpeg"
        };

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
            if (Util.StringUtil.GetExtension(name).Equals(string.Empty))
            {
                bool found = false;
                string newName = string.Empty;
                foreach (string extension in Extensions)
                {
                    newName = name + extension;
                    found = File.Exists(newName);

                    if (found) break;
                }
                if (!found)
                {
                    Logger.Log("Could not load image \"" + name + "\", maybe it doesn't exist", level: LogLevel.Error);
                    return null;
                }
                name = newName;
            }
            
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
            if (Util.StringUtil.GetExtension(name).Equals(string.Empty))
            {
                bool found = false;
                string newName = string.Empty;
                foreach (string extension in Extensions)
                {
                    newName = name + extension;
                    found = File.Exists(newName);

                    if (found) break;
                }
                if (!found)
                    return null;
                name = newName;
            }

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