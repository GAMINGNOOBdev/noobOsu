using System.Collections.Generic;

namespace noobOsu.Game.Beatmaps.Timing
{
    public interface IBeatmapTiming
    {
        float BPM_average { get; }
        float BPM_min { get; }
        float BPM_max { get; }
        float FirstBPM { get; }
        
        ITimingPoint GetTimingPoint(int timestamp);
        ITimingPoint GetTimingPoint(int timestamp, bool uninherited);
        float BPM_At(int timestamp);
        IReadOnlyList<ITimingPoint> GetTimingPoints();
    }
}