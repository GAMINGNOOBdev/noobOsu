using osuTK;
using noobOsu.Game.Skins;
using osu.Framework.Audio;
using osu.Framework.Logging;
using osu.Framework.Threading;
using noobOsu.Game.HitObjects;
using osu.Framework.Audio.Track;
using System.Collections.Generic;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public partial class DrawableBeatmap : CompositeDrawable, IBeatmap
    {
        private readonly List<DrawableHitObject> drawableObjects = new List<DrawableHitObject>();
        private Container draw_container;
        private readonly Beatmap beatmap;
        private Scheduler ParentScheduler;
        private readonly ISkin CurrentSkin;

        public Beatmap Map => beatmap;
        public bool Started { get; set; } = false;

        public DrawableBeatmap(Beatmap beatmap, ISkin current)
        {
            this.beatmap = beatmap;
            this.CurrentSkin = current;
            if (this.beatmap == null) this.beatmap = new Beatmap(null);
        }

        public void LoadBeatmap(string path)
        {
            beatmap.LoadBeatmap(path);
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
            
            // delay the starting for 10 ms to make sure the beatmap starts properly with the audio playing at the same time
            Started = true;
            ParentScheduler.AddDelayed( () => {
                beatmap.MapAudio.Start();
            }, GetInfo().Timing.FirstBPM + BeatmapDifficulty.ScaleWithRange(GetInfo().Difficulty.OD, 80, 50, 20));
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
                    drawableObjects.Add(obj = new HitCircle(beatmap.HitObjects[i], this, beatmap.GetInfo().Difficulty, Colors));

                if (beatmap.HitObjects[i].isSlider())
                    drawableObjects.Add(obj = new Slider(beatmap.HitObjects[i], this, beatmap.GetInfo().Difficulty, Colors));

                if (beatmap.HitObjects[i].isSpinner())
                {
                    // TODO: --- add spinners ---
                }

                if (obj != null) draw_container.Add(obj);
            }

            CurrentSkin.ResolveSkinnables(drawableObjects, Textures);
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
        }
    }
}