using osu.Framework.Testing;

namespace noobOsu.Game.Tests.Visual
{
    public partial class noobOsuTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new noobOsuTestSceneTestRunner();

        private partial class noobOsuTestSceneTestRunner : noobOsuGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
