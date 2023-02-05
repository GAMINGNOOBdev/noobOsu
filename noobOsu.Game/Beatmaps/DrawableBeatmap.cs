using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Skins;
using osu.Framework.Audio;
using osu.Framework.Timing;
using osu.Framework.Logging;
using noobOsu.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Threading;
using noobOsu.Game.HitObjects;
using osu.Framework.Audio.Track;
using System.Collections.Generic;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public partial class DrawableBeatmap : CompositeDrawable, IBeatmap
    {
        private readonly List<DrawableHitObject> drawableObjects = new List<DrawableHitObject>();
        private readonly IExternalCanAddChildren ParentContainer;
        private Sprite beatmapBackground;
        private Box beatmapBackgroundDim;
        private Container draw_container;
        private readonly Beatmap beatmap;
        private Scheduler ParentScheduler;
        private ISkin CurrentSkin {
            get
            {
                return Settings.GameSettings.GetCurrentSkin();
            }
            set { return; }
        }

        public Beatmap Map => beatmap;
        public bool Started { get; set; } = false;
        public IBeatmapGeneral CurrentMap => beatmap?.CurrentMap;

        public DrawableBeatmap(Beatmap beatmap, IExternalCanAddChildren parent)
        {
            ParentContainer = parent;
            this.beatmap = beatmap;
            if (this.beatmap == null) this.beatmap = new Beatmap(null);
        }

        public void LoadBeatmap(IBeatmapGeneral map)
        {
            beatmap.LoadBeatmap(map);
        }

        public void RemoveObject(DrawableHitObject obj)
        {
            obj.End();
            ParentScheduler.AddDelayed( () => {
                draw_container.Remove(obj, false);
                obj.DisposeResources();
            }, 200);
        }

        public new void Update()
        {
            /*if (GetAudio().HasCompleted)
            {
                this.Dispose();
            }*/
        }

        public Track GetAudio() => beatmap.GetAudio();

        public BeatmapInfo GetInfo() => beatmap.GetInfo();

        public void SetDrawContainer(Container container) => draw_container = container;

        public void StartBeatmap(Scheduler parentScheduler)
        {
            if (Started) return;
            ParentScheduler = parentScheduler;
            
            Started = true;
            ParentScheduler.AddDelayed( () => {
                beatmap.MapAudio.Start();    
            }, GetInfo().Timing.FirstBPM);
        }

        public new void Dispose()
        {
            foreach (DrawableHitObject d in drawableObjects)
            {
                d.DisposeResources();
            }
            if (beatmap.GetAudio() != null)
            {
                Track audio = beatmap.GetAudio();
                audio.Stop();
                audio.Dispose();
            }
            beatmap.Dispose();
        }

        public void Load(AudioManager audioManager, TextureStore Textures)
        {
            if (audioManager != null)
            {
                beatmap.Load(audioManager, null);
                if (beatmap.MapAudio != null)
                    AddInternal(beatmap.MapAudio);
            }

            if (!GetInfo().Events.GetBackgroundIfPresent().Equals(string.Empty))
            {
                beatmapBackground = new Sprite(){
                    Depth = float.MaxValue,
                    RelativeSizeAxes = Axes.Both,
                    Texture = Textures.Get("Songs/" + CurrentMap.ParentSet.SetID + " " + CurrentMap.ParentSet.SetName + "/" + GetInfo().Events.GetBackgroundIfPresent()),
                };
                ParentContainer.AddChild(beatmapBackground);

                beatmapBackgroundDim = new Box(){
                    Depth = float.MaxValue - 1,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                };
                Settings.GameSettings.INSTANCE.BeatmapBackgroundDim.BindValueChanged((val) => { beatmapBackgroundDim.Alpha = val.NewValue / 100f; }, true);
                ParentContainer.AddChild(beatmapBackgroundDim);
            }

            IColorStore Colors = CurrentSkin.Colors.SkinComboColors;
            Colors.RestartColor();
            if (Settings.GameSettings.INSTANCE.UseBeatmapColors.Value && !beatmap.GetInfo().Colors.IsEmpty())
            {
                Colors = beatmap.GetInfo().Colors;
            }

            DrawableHitObject obj;
            for (int i = 0; i < beatmap.HitObjects.Count; i++)
            {
                obj = null;

                if (beatmap.HitObjects[i].isCircle())
                    drawableObjects.Add(obj = new HitCircle(beatmap.HitObjects[i], this, Colors));

                if (beatmap.HitObjects[i].isSlider())
                    drawableObjects.Add(obj = new Slider(beatmap.HitObjects[i], this, Colors));

                if (beatmap.HitObjects[i].isSpinner())
                {
                    // TODO: --- add spinners ---
                }

                if (obj != null) draw_container.Add(obj);
            }

            CurrentSkin.ResolveSkinnables(drawableObjects, Textures);
        }

        public void ReadObjects(bool addImmediately)
        {
            IColorStore Colors = CurrentSkin.Colors.SkinComboColors;
            Colors.RestartColor();
            if (Settings.GameSettings.INSTANCE.UseBeatmapColors.Value && !beatmap.GetInfo().Colors.IsEmpty())
            {
                Colors = beatmap.GetInfo().Colors;
            }

            DrawableHitObject obj;
            for (int i = 0; i < beatmap.HitObjects.Count; i++)
            {
                obj = null;

                if (beatmap.HitObjects[i].isCircle())
                    drawableObjects.Add(obj = new HitCircle(beatmap.HitObjects[i], this, Colors));

                if (beatmap.HitObjects[i].isSlider())
                    drawableObjects.Add(obj = new Slider(beatmap.HitObjects[i], this, Colors));

                if (beatmap.HitObjects[i].isSpinner())
                {
                    // TODO: --- add spinners ---
                }

                if (obj != null && addImmediately) draw_container.Add(obj);
            }
        }

        public void ClearData()
        {
            ClearInternal(false);
            if (drawableObjects.Count > 0)
            {
                foreach (DrawableHitObject d in drawableObjects)
                {
                    d.DisposeResources();
                }
                draw_container.RemoveRange(drawableObjects, false);
                drawableObjects.Clear();
            }
            if (!GetInfo().Events.GetBackgroundIfPresent().Equals(string.Empty))
            {
                beatmapBackground.Dispose();
            }
        }
    }
}