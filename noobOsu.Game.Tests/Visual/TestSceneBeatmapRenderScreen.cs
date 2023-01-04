using osu.Framework.Graphics;
using osu.Framework.Screens;
using noobOsu.Game.Screens;
using NUnit.Framework;

namespace noobOsu.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneBeatmapRenderScreen : noobOsuTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneBeatmapRenderScreen()
        {
            BeatmapRenderScreen ms = new BeatmapRenderScreen(){ RelativeSizeAxes = Axes.Both };
            ms.SetBeatmapPath("Songs/mytestmap/mytestmap.osu");
            Add(new ScreenStack( ms ));
        }
    }
}
