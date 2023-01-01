using noobOsu.Game.Beatmaps.Timing;

namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapInfo
    {
        // beatmap difficulty
        BeatmapDifficulty Difficulty { get; }

        // beatmap metadata
        BeatmapMetadata Metadata { get; }

        // beatmap colors
        BeatmapColors Colors { get; }

        // beatmap timing points
        BeatmapTiming Timing { get; }

        /////////////////////////////////
        //                             //
        // General beatmap information //
        //                             //
        /////////////////////////////////

        // filename of the audio file (relative to the beatmap directory)
        string AudioFilename { get; set; }

        // time (in ms) to spend before starting the audio
        float AudioLeadIn { get; set; }

        // deprecated audio hash
        string AudioHash { get; set; }

        // Preview time in ms (when the audio preview shoud start)
        int PreviewTime { get; set; }

        // Countdown speed (0=none, 1=normal, 2=half, 3=double)
        int Countdown { get; set; }

        // Sample set (available ones are "Normal", "Soft", "Drum")
        string SampleSet { get; set; }
    }
}