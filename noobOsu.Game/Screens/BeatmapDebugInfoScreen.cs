using osuTK;
using noobOsu.Game.Skins;
using noobOsu.Game.Stores;
using noobOsu.Game.Graphics;
using noobOsu.Game.Beatmaps;
using osu.Framework.Screens;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.Screens
{
    public partial class BeatmapDebugInfoScreen : Screen, IExternalCanAddChildren
    {
        public static BeatmapDebugInfoScreen INSTANCE { get; private set; }
        private readonly DrawSizePreservingFillContainer contents = new DrawSizePreservingFillContainer();
        private BasicScrollContainer<Drawable> timingContents;
        
        private Beatmap beatmap;
        private DrawableBeatmap drawableBeatmap;
        private BasicButton showTimings, showHitobjects, backbutton;
        private SpriteText beatmapClockText;
        private IBeatmapGeneral beatmapInfo;

        public BeatmapDebugInfoScreen()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;
        }

        public void AddChild(Drawable child)
        {
            AddInternal(child);
        }

        public void RemoveChild(Drawable child)
        {
            RemoveInternal(child, false);
        }

        public void SetMap(IBeatmapGeneral map)
        {
            beatmapInfo = map;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            showTimings = new BasicButton()
            {
                Origin = Anchor.TopCentre,
                Anchor = Anchor.TopCentre,
                Text = "Timings",
                Size = new Vector2(20 * "Timings".Length, 20),
                Scale = new Vector2(2f),
                Depth = 0,
                Action = () => {
                    ShowTimings();
                },
            };
            AddInternal(showTimings);

            showHitobjects = new BasicButton()
            {
                Origin = Anchor.TopRight,
                Anchor = Anchor.TopRight,
                Text = "Objects",
                Size = new Vector2(20 * "Objects".Length, 20),
                Scale = new Vector2(2f),
                Depth = 0,
                Action = () => {
                    ShowHitobjects();
                },
            };
            AddInternal(showHitobjects);

            backbutton = new BasicButton()
            {
                Origin = Anchor.TopLeft,
                Anchor = Anchor.TopLeft,
                Text = "Back",
                Size = new Vector2(20 * "Back".Length, 20),
                Scale = new Vector2(2f),
                Action = () => {
                    this.Exit();
                },
            };
            AddInternal(backbutton);

            SkinFontStore.SetCurrentSkin(null);

            beatmap = new Beatmap(beatmapInfo);
            drawableBeatmap = new DrawableBeatmap(beatmap, this);
            drawableBeatmap.SetDrawContainer(contents);
            drawableBeatmap.ReadObjects(false);

            beatmapClockText = new SpriteText()
            {
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Colour = Colour4.LightBlue,
                Font = FontUsage.Default.With(size: 20),
            };
            AddInternal(beatmapClockText);

            beatmap.Load(noobOsuAudioManager.INSTANCE, null);
            if (beatmap.MapAudio != null)
            {
                AddInternal(beatmap.MapAudio);
                drawableBeatmap.beatmapClock.SetTrack(beatmap.GetAudio());
                beatmap.MapAudio.Looping = true;
                beatmap.MapAudio.Start();
            }

            timingContents = new BasicScrollContainer<Drawable>(){
                RelativeSizeAxes = Axes.Both,
                Depth = -1,
            };

            AddInternal(timingContents);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            drawableBeatmap?.beatmapClock.ProcessFrame();
            beatmapClockText.Text = "Map Clock: " + drawableBeatmap?.beatmapClock.ToString();
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.ControlPressed && e.Key == osuTK.Input.Key.Q)
            {
                this.Exit();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            beatmap.Dispose();
            SkinFontStore.SetCurrentSkin(null);
        }

        private void ShowHitobjects()
        {
            timingContents.Clear();
            int y = 0;
            foreach (HitObjects.HitObject obj in beatmap.HitObjects)
            {
                timingContents.Add(
                    new SpriteText(){
                        RelativeSizeAxes=Axes.X,
                        Text=obj.ToString(),
                        Colour=Colour4.White,
                        Font=FontUsage.Default,
                        Y = y,
                        Height=20
                    }
                );
                y += 30;
            }
        }

        private void ShowTimings()
        {
            timingContents.Clear();
            int y = 0;
            foreach (Beatmaps.Timing.ITimingPoint t in beatmap.GetInfo().Timing.GetTimingPoints())
            {
                timingContents.Add(
                    new SpriteText(){
                        RelativeSizeAxes=Axes.X,
                        Text=t.ToString(),
                        Colour=Colour4.White,
                        Font=FontUsage.Default,
                        Y = y,
                        Height=20
                    }
                );
                y += 30;
            }
        }
    }
}
