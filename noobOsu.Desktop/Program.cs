using osu.Framework.Platform;
using osu.Framework;
using noobOsu.Game;

namespace noobOsu.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"noobOsu"))
            using (osu.Framework.Game game = new noobOsuGame())
                host.Run(game);
        }
    }
}
