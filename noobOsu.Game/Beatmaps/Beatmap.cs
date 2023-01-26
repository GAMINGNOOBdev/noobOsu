using System.IO;
using osu.Framework.Audio;
using osu.Framework.Logging;
using osu.Framework.Threading;
using noobOsu.Game.HitObjects;
using osu.Framework.Audio.Track;
using System.Collections.Generic;
using osu.Framework.Graphics.Audio;
using noobOsu.Game.Beatmaps.Timing;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Beatmaps
{
    public class Beatmap : IBeatmap
    {
        private BeatmapInfo info = new BeatmapInfo();
        private readonly List<HitObject> hitObjects = new List<HitObject>();
        private string relative_path = null;
        private DrawableTrack mapAudio;
        private Track mapTrack;

        public List<HitObject> HitObjects => hitObjects;
        public DrawableTrack MapAudio => mapAudio;
        public bool Started { get; set; } = false;

        public Beatmap(string path)
        {
            if (path == null) return;
            relative_path = path.Substring(0, path.LastIndexOf('/'));
            ReadBeatmap(path);
        }

        public BeatmapInfo GetInfo() => info;

        public void LoadBeatmap(string path)
        {
            ClearData();
            if (path == null) return;
            relative_path = path.Substring(0, path.LastIndexOf('/'));
            ReadBeatmap(path);
        }

        public void Load(AudioManager audioManager, TextureStore Textures)
        {
            if (relative_path == null) return;
            if (!relative_path.StartsWith("Songs/"))
                mapTrack = audioManager.GetTrackStore().GetVirtual(1000, relative_path + "/" + info.AudioFilename);
            else
                mapTrack = audioManager.GetTrackStore().Get(relative_path + "/" + info.AudioFilename);

            mapAudio = new DrawableTrack(mapTrack);
            info.AudioLength = (float)mapTrack.Length;
        }

        public void Dispose()
        {
            if (mapTrack != null)
            {
                mapTrack.Dispose();
                mapTrack = null;
            }
        }

        public Track GetAudio() => mapTrack;

        public void RemoveObject(DrawableHitObject obj) => throw new System.NotImplementedException();

        public void StartBeatmap(Scheduler ParentScheduler) => throw new System.NotImplementedException();

        public void ClearData()
        {
            hitObjects.Clear();
            if (mapTrack != null)
            {
                mapTrack.Dispose();
                mapTrack = null;
            }
            
            if (mapAudio != null)
            {
                mapAudio.Dispose();
                mapAudio = null;
            }
            info = new BeatmapInfo();
        }

        private void ReadBeatmap(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                string current_line;
                current_line = file.ReadLine();
                if (!current_line.StartsWith("osu file format v"))
                {
                    return;
                }
                while ((current_line = file.ReadLine()) != null)
                {
                    if (current_line.Equals("[General]"))
                    {
                        ReadGeneralInfo(file);
                        continue;
                    }

                    if (current_line.Equals("[Metadata]"))
                    {
                        ReadMetadata(file);
                        continue;
                    }

                    if (current_line.Equals("[Difficulty]"))
                    {
                        ReadDifficultyInfo(file);
                        continue;
                    }

                    if (current_line.Equals("[HitObjects]"))
                    {
                        ReadHitObjects(file);
                        continue;
                    }

                    if (current_line.Equals("[Colours]"))
                    {
                        ReadComboColors(file);
                        continue;
                    }

                    if (current_line.Equals("[TimingPoints]"))
                    {
                        ReadTimingInfo(file);
                        continue;
                    }

                    if (current_line.Equals("[Events]"))
                    {
                        ReadEvents(file);
                        continue;
                    }
                }
            }
        }

        private void ReadGeneralInfo(StreamReader file)
        {
            string line;
            string[] values;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                values = line.Split(": ");
                
                if (values[0].Equals("AudioFilename"))
                {
                    info.AudioFilename = values[1];
                }
                if (values[0].Equals("AudioLeadIn"))
                {
                    info.AudioLeadIn = float.Parse(values[1]);
                }
                if (values[0].Equals("AudioLength"))
                {
                    info.AudioLength = float.Parse(values[1]);
                }
            }
        }

        private void ReadMetadata(StreamReader file)
        {
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);
            }
        }

        private void ReadEvents(StreamReader file)
        {
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);
                info.Events.AddEvent(line);
            }
        }

        private void ReadDifficultyInfo(StreamReader file)
        {
            string line;
            string[] values;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                values = line.Split(":");

                if (values[0].Equals("HPDrainRate"))
                {
                    ((BeatmapDifficulty)info.Difficulty).HP = float.Parse(values[1]);
                }
                if (values[0].Equals("CircleSize"))
                {
                    ((BeatmapDifficulty)info.Difficulty).CS = float.Parse(values[1]);
                }
                if (values[0].Equals("OverallDifficulty"))
                {
                    ((BeatmapDifficulty)info.Difficulty).OD = float.Parse(values[1]);
                }
                if (values[0].Equals("ApproachRate"))
                {
                    ((BeatmapDifficulty)info.Difficulty).AR = float.Parse(values[1]);
                }
                if (values[0].Equals("SliderMultiplier"))
                {
                    ((BeatmapDifficulty)info.Difficulty).SliderMultiplier = float.Parse(values[1]);
                }
                if (values[0].Equals("SliderTickRate"))
                {
                    ((BeatmapDifficulty)info.Difficulty).SliderTickRate = float.Parse(values[1]);
                }
            }
        }

        private void ReadTimingInfo(StreamReader file)
        {
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                info.Timing.AddTimingPoint(new TimingPoint(line));
            }
            info.Timing.CalculateBPM();
        }

        private void ReadComboColors(StreamReader file)
        {
            string line;
            string[] values;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                values = line.Split(" : ");
                if (values[0].StartsWith("Combo"))
                {
                    info.Colors.AddColor(values[1]);
                }
            }
        }

        private void ReadHitObjects(StreamReader file)
        {
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Equals(string.Empty)) break;
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                hitObjects.Add( new HitObject(line) );
            }
        }
    }
}