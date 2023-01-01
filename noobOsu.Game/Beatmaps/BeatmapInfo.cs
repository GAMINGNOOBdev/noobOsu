using noobOsu.Game.Beatmaps.Timing;

namespace noobOsu.Game.Beatmaps
{
    public class BeatmapInfo : IBeatmapInfo
    {
        // beatmap difficulty
        public BeatmapDifficulty Difficulty { get; private set; } = new BeatmapDifficulty();

        // beatmap metadata
        public BeatmapMetadata Metadata { get; private set; } = new BeatmapMetadata();

        // beatmap events (video, breaks, storyboard, background)
        public BeatmapEvents Events { get; private set; } = new BeatmapEvents();

        // beatmap colors
        public BeatmapColors Colors { get; private set; } = new BeatmapColors();

        // beatmap timing points
        public BeatmapTiming Timing { get; private set; } = new BeatmapTiming();

        /////////////////////////////////
        //                             //
        // General beatmap information //
        //                             //
        /////////////////////////////////

        // filename of the audio file (relative to the beatmap directory)
        public string AudioFilename { get; set; } = "audio.mp3";

        // time (in ms) to spend before starting the audio
        public float AudioLeadIn { get; set; } = 0f;

        // deprecated audio hash
        public string AudioHash { get; set; } = null;

        // Preview time in ms (when the audio preview shoud start)
        public int PreviewTime { get; set; } = -1;

        // Countdown speed (0=none, 1=normal, 2=half, 3=double)
        public int Countdown { get; set; } = 1;

        // Sample set (available ones are "Normal", "Soft", "Drum")
        public string SampleSet { get; set; } = "Normal";

        // Length of the audio file in ms, needs to be set after getting the audio file
        public float AudioLength { get; set; } = -1;

        public float GetAudioLength() => AudioLength;
    }
}