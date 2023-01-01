using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Threading;

namespace noobOsu.Game
{
    public class noobOsuAudioManager : AudioManager
    {
        public static noobOsuAudioManager INSTANCE;

        public noobOsuAudioManager(AudioThread audioThread, ResourceStore<byte[]> trackStore, ResourceStore<byte[]> sampleStore)
            : base(audioThread, trackStore, sampleStore)
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }
    }
}