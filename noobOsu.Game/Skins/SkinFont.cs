namespace noobOsu.Game.Skins
{
    public interface ISkinFont
    {
        // prefix path of the hitcircle numbers
        string HitCirclePrefix { get; }

        // overlap of the hitcircle numbers in pixels (negatives add a gap)
        int HitCircleOverlap { get; }

        // prefix path of the score numbers
        string ScorePrefix { get; }

        // overlap of the score numbers in pixels (negatives add a gap)
        int ScoreOverlap { get; }

        // prefix path of the combo numbers
        string ComboPrefix { get; }

        // overlap of the combo numbers in pixels (negatives add a gap)
        int ComboOverlap { get; }

        // parse font info from string
        void AddFontInfo(string info);
    }

    public class SkinFont : ISkinFont
    {
        public string HitCirclePrefix { get; private set; } = "default";

        public int HitCircleOverlap { get; private set; } = -2;

        public string ScorePrefix { get; private set; } = "score";

        public int ScoreOverlap { get; private set; } = 0;

        public string ComboPrefix { get; private set; } = "score";

        public int ComboOverlap { get; private set; } = 0;

        private Skin Parent;

        public SkinFont(Skin p)
        {
            Parent = p;
        }

        public void AddFontInfo(string info)
        {
            string[] splitInfo = info.Split(": ");
            
            if (splitInfo.Length < 2) return;

            if (splitInfo[0].Equals(HitCirclePrefix))
            {
                HitCirclePrefix = splitInfo[1];
            }
            if (splitInfo[0].Equals(HitCirclePrefix))
            {
                HitCircleOverlap = int.Parse(splitInfo[1]);
            }
            
            if (splitInfo[0].Equals(ScorePrefix))
            {
                ScorePrefix = splitInfo[1];
            }
            if (splitInfo[0].Equals(ScorePrefix))
            {
                ScoreOverlap = int.Parse(splitInfo[1]);
            }
            
            if (splitInfo[0].Equals(ComboPrefix))
            {
                ComboPrefix = splitInfo[1];
            }
            if (splitInfo[0].Equals(ComboPrefix))
            {
                ComboOverlap = int.Parse(splitInfo[1]);
            }
        }
    }
}