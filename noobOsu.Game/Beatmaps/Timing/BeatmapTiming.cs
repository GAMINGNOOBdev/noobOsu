using System.Collections.Generic;

namespace noobOsu.Game.Beatmaps.Timing
{
    public class BeatmapTiming : IBeatmapTiming
    {
        private readonly List<ITimingPoint> timingPoints = new List<ITimingPoint>();
        private readonly List<double> bpmPoints = new List<double>();

        public double BPM_average { get; set; } = 180;
        public double BPM_min { get; set; } = 0;
        public double BPM_max { get; set; } = 0;
        public double FirstBPM { get; set; } = 0f;

        public void AddTimingPoint(ITimingPoint timingPoint)
        {
            timingPoints.Add( timingPoint );

            if (timingPoint.Uninherited == 1)
                bpmPoints.Add(timingPoint.BeatLength);

            if (FirstBPM == 0)
                FirstBPM = timingPoint.BeatLength;
        }

        public void CalculateBPM()
        {
            foreach (double bpm in bpmPoints)
            {
                if (bpm > BPM_average || bpm > BPM_max)
                {
                    BPM_max = 1 / bpm * 1000 * 60;
                }

                if (bpm > BPM_average)
                {
                    BPM_min = 1 / bpm * 1000 * 60;
                }
            }

            if (BPM_max == BPM_min)
            {
                BPM_average = BPM_max;
            }
        }

        public ITimingPoint GetTimingPoint(int timestamp) => FindClosestTimingPoint(timestamp, false);

        public ITimingPoint GetTimingPoint(int timestamp, bool uninherited) => FindClosestTimingPoint(timestamp, uninherited);

        public double BPM_At(int timestamp)
        {
            double lastBPM = 0;
            foreach (ITimingPoint t in timingPoints)
            {
                if (t.TimeStamp > timestamp) break;
                if (t.Uninherited == 1) lastBPM = t.BeatLength;
            }
            if (lastBPM == 0)
                return FirstBPM;
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