namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapEvent
    {
        BeatmapEventType Type { get; }
        float StartTime { get; }
        float EndTime { get; }
        string Filename { get; }
        int xOffset { get; }
        int yOffset { get; }

        static BeatmapEventType ToEventType(string typeStr)
        {
            if (typeStr.Equals("0")) return BeatmapEventType.Background;
            if (typeStr.Equals("1") || typeStr.Equals("Video")) return BeatmapEventType.Video;
            if (typeStr.Equals("2")) return BeatmapEventType.Break;

            return BeatmapEventType.None;
        }
    }

    public enum BeatmapEventType
    {
        Background,
        Video,
        Break,
        None,
    }
}