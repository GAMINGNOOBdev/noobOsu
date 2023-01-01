using osuTK;
using osu.Framework.Audio;
using osu.Framework.Logging;
using osu.Framework.Threading;
using noobOsu.Game.HitObjects;
using osu.Framework.Audio.Track;
using System.Collections.Generic;
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

        public Beatmap Map => beatmap;
        public bool Started { get; set; } = false;

        public DrawableBeatmap(Beatmap beatmap)
        {
            this.beatmap = beatmap;
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
            beatmap.Dispose();
        }

        public void Load(AudioManager audioManager)
        {
            Logger.Log("loading beatmap objects");
            if (audioManager != null)
            {
                beatmap.Load(audioManager);
                AddInternal(beatmap.MapAudio);
            }

            DrawableHitObject obj;
            for (int i = 0; i < beatmap.HitObjects.Count; i++)
            {
                obj = null;
                if (beatmap.HitObjects[i].isCircle())
                {
                    drawableObjects.Add(obj = new HitCircle(beatmap.HitObjects[i], this, beatmap.GetInfo().Difficulty, beatmap.GetInfo().Colors));
                }
                if (beatmap.HitObjects[i].isSlider())
                {
                    drawableObjects.Add(obj = new Slider(beatmap.HitObjects[i], this, beatmap.GetInfo().Difficulty, beatmap.GetInfo().Colors));
                    //drawableObjects.Add(obj = new HitCircle(beatmap.HitObjects[i], this, beatmap.GetInfo().Difficulty, beatmap.GetInfo().Colors));
                }

                if (obj != null) draw_container.Add(obj);
            }

            Logger.Log("added " + drawableObjects.Count + " objects to the scene");
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