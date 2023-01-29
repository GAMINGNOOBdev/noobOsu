namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapDifficulty
    {
        /// Approach Rate
        double AR { get; }

        // Circle Size
        double CS { get; }

        // Overall Difficulty
        double OD { get; }

        // Health Drain
        double HP { get; }

        // Slider velocity
        double SliderMultiplier { get; }

        // Slider tick rate
        double SliderTickRate { get; }
    }

    public class BeatmapDifficulty : IBeatmapDifficulty
    {
        public double AR { get; set; } = 5f;

        public double CS { get; set; } = 5f;

        public double OD { get; set; } = 5f;

        public double HP { get; set; } = 5f;

        public double SliderMultiplier { get; set; } = 1f;

        public double SliderTickRate { get; set; } = 1f;

        public static double ScaleWithRange(double value, double maxVal, double midVal, double minVal)
        {
            if (value < 5f) return midVal + (maxVal - minVal) * (5 - value) / 5;
            if (value > 5f) return midVal - (midVal - minVal) * (value - 5) / 5;

            return midVal;
        }
    }
}