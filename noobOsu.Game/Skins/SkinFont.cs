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

        // only the last (or only) part of the hitcircle, score and combo prefix
        string HitCircle { get; }
        string Score { get; }
        string Combo { get; }

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

        public string HitCircle { get; private set; } = "default";
        public string Score { get; private set; } = "score";
        public string Combo { get; private set; } = "score";

        private ISkin Parent;

        public SkinFont(ISkin p)
        {
            Parent = p;
        }

        public void AddFontInfo(string info)
        {
            string[] splitInfo = info.Split(": ");
            
            if (splitInfo.Length < 2) return;

            if (splitInfo[1].Contains('\\'))
                splitInfo[1] = splitInfo[1].Replace('\\','/');

            if (splitInfo[0].Equals("HitCirclePrefix"))
            {
                HitCirclePrefix = splitInfo[1];
                if (HitCirclePrefix.Contains("/"))
                {
                    string[] prefixes = HitCirclePrefix.Split('/');
                    HitCircle = prefixes[prefixes.Length-1];
                }
                else
                    HitCircle = HitCirclePrefix;
            }
            if (splitInfo[0].Equals("HitCircleOverlap"))
            {
                HitCircleOverlap = int.Parse(splitInfo[1]);
            }
            
            if (splitInfo[0].Equals("ScorePrefix"))
            {
                ScorePrefix = splitInfo[1];
                if (ScorePrefix.Contains("/"))
                {
                    string[] prefixes = ScorePrefix.Split('/');
                    HitCircle = prefixes[prefixes.Length-1];
                }
                else
                    HitCircle = ScorePrefix;
            }
            if (splitInfo[0].Equals("ScoreOverlap"))
            {
                ScoreOverlap = int.Parse(splitInfo[1]);
            }
            
            if (splitInfo[0].Equals("ComboPrefix"))
            {
                ComboPrefix = splitInfo[1];
                if (ComboPrefix.Contains("/"))
                {
                    string[] prefixes = ComboPrefix.Split('/');
                    HitCircle = prefixes[prefixes.Length-1];
                }
                else
                    HitCircle = ComboPrefix;
            }
            if (splitInfo[0].Equals("ComboOverlap"))
            {
                ComboOverlap = int.Parse(splitInfo[1]);
            }
        }
    }
}