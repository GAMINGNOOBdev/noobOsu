using osu.Framework.IO.Stores;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace noobOsu.Game.Stores
{
    public class ExternalAssetStore : IResourceStore<byte[]>
    {
        public void Dispose() {}

        public byte[] Get(string name)
        {
            using (Stream file = GetStream(name))
            {
                byte[] result = new byte[file.Length];
                file.Read(result, 0, (int)file.Length);
                return result;
            }
        }

        public Task<byte[]> GetAsync(string name, CancellationToken cancellationToken = default) => null;

        public IEnumerable<string> GetAvailableResources() => Enumerable.Empty<string>();

        public Stream GetStream(string name) => File.OpenRead(name);
    }
}