namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapDifficulty
    {
        /// Approach Rate
        float AR { get; }

        // Circle Size
        float CS { get; }

        // Overall Difficulty
        float OD { get; }

        // Health Drain
        float HP { get; }

        // Slider velocity
        float SliderMultiplier { get; }

        // Slider tick rate
        float SliderTickRate { get; }
    }

    public class BeatmapDifficulty : IBeatmapDifficulty
    {
        public float AR { get; set; } = 5f;

        public float CS { get; set; } = 5f;

        public float OD { get; set; } = 5f;

        public float HP { get; set; } = 5f;

        public float SliderMultiplier { get; set; } = 1f;

        public float SliderTickRate { get; set; } = 1f;

        public static float ScaleWithRange(float value, float maxVal, float midVal, float minVal)
        {
            if (value < 5f) return midVal + (maxVal - minVal) * (5 - value) / 5;
            if (value > 5f) return midVal - (midVal - minVal) * (value - 5) / 5;

            return midVal;
        }
    }
}