using System.Collections.Generic;

namespace noobOsu.Game.Beatmaps.Timing
{
    public class BeatmapTiming : IBeatmapTiming
    {
        private readonly List<ITimingPoint> timingPoints = new List<ITimingPoint>();

        public float BPM_average { get; set; } = 180;
        public float BPM_min { get; set; } = 0;
        public float BPM_max { get; set; } = 0;
        public float FirstBPM { get; set; } = 0f;

        public void AddTimingPoint(ITimingPoint timingPoint)
        {
            timingPoints.Add( timingPoint );
            if (FirstBPM == 0 && timingPoint.Uninherited == 1) FirstBPM = timingPoint.BeatLength;
        }

        public void CalculateBPM()
        {
            foreach (ITimingPoint t in timingPoints)
            {
                if (t.Uninherited == 1)
                {
                    if (t.BeatLength > BPM_average || t.BeatLength > BPM_max)
                    {
                        BPM_max = t.BeatLength;
                    }

                    if (t.BeatLength > BPM_average)
                    {
                        BPM_min = t.BeatLength;
                    }
                }
            }

            if (BPM_max == BPM_min)
            {
                BPM_average = BPM_max;
            }
        }

        public ITimingPoint GetTimingPoint(int timestamp) => FindClosestTimingPoint(timestamp, false);

        public ITimingPoint GetTimingPoint(int timestamp, bool uninherited) => FindClosestTimingPoint(timestamp, uninherited);

        public float BPM_At(int timestamp)
        {
            float lastBPM = 0;
            foreach (ITimingPoint t in timingPoints)
            {
                if (t.TimeStamp > timestamp) break;
                if (t.Uninherited == 1) lastBPM = t.BeatLength;
            }
            return lastBPM;
        }

        public IReadOnlyList<ITimingPoint> GetTimingPoints() => timingPoints;

        private ITimingPoint FindClosestTimingPoint(int timestamp, bool uninherited)
        {
            ITimingPoint result = null;
            foreach (ITimingPoint t in timingPoints)
            {
                if (t.TimeStamp > timestamp) break;

                if (t.TimeStamp <= timestamp && t.Uninherited == (uninherited ? 1 : 0))
                {
                    result = t;
                }
            }

            return result;
        }
    }
}