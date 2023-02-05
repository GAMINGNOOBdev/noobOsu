using System.Collections.Generic;

namespace noobOsu.Game.Beatmaps.Timing
{
    public interface IBeatmapTiming
    {
        double BPM_average { get; }
        double BPM_min { get; }
        double BPM_max { get; }
        double FirstBPM { get; }
        
        ITimingPoint GetTimingPoint(int timestamp);
        ITimingPoint GetTimingPoint(int timestamp, bool uninherited);
        double BPM_At(int timestamp);
        double GetTimingOffset();
        IReadOnlyList<ITimingPoint> GetTimingPoints();
    }
}