using osuTK;

namespace noobOsu.Game.HitObjects
{
    public class HitObject
    {
        private readonly Vector2 position = new Vector2(0);

        public Vector2 Position => position;

        public Vector2 RawPosition { get; private set; }

        public int Time { get; private set; } = 0;
        public int Type { get; private set; } = 0;
        public int HitSound { get; private set; } = 0;

        public Vector3 Color { get; } = new Vector3(1f);

        public int ComboColorSkip { get; private set; } = 0;

        private readonly SliderInfo sliderInfo = new SliderInfo();
        public ISliderInfo SliderInformation => sliderInfo;

        public readonly HitObjectPath Path;

        public HitObject(string expression)
        {
            string[] object_values = expression.Split(',');
            position.X = int.Parse(object_values[0]) / 640f + 0.1f;
            position.Y = int.Parse(object_values[1]) / 513f + 0.124f;
            Time = int.Parse(object_values[2]);
            Type = int.Parse(object_values[3]);
            HitSound = int.Parse(object_values[4]);

            ComboColorSkip = (Type >> 4) & 0b111;

            // read additional info if this is a slider
            if (this.isSlider())
            {
                Path = new HitObjectPath(this);
                ParseSliderInfo(object_values);
            }
        }

        public bool isCircle()
        {
            return (Type & 1) > 0;
        }

        public bool isSlider()
        {
            return (Type & (1 << 1)) > 0;
        }

        public bool isSpinner()
        {
            return (Type & (1 << 3)) > 0;
        }

        public bool isNewCombo()
        {
            return (Type & (1 << 2)) > 0;
        }

        private void ParseSliderInfo(string[] object_values)
        {
            sliderInfo.SlideRepeat = int.Parse(object_values[6]);
            sliderInfo.Length = float.Parse(object_values[7]);

            string[] curveInfo = object_values[5].Split('|');
            sliderInfo.CurveType = SliderInfo.StringToType(curveInfo[0]);

            RawPosition = new Vector2(int.Parse(object_values[0]), int.Parse(object_values[1]));
            Path.SetStartPosition(RawPosition);
            Path.AddAnchorPoint(RawPosition);
            for (int i = 1; i < curveInfo.Length; i++) {
                Path.AddAnchorPoint(curveInfo[i]);
            }
            Path.FinishPath();

            if (sliderInfo.SlideRepeat > 0)
                sliderInfo.TotalSliderSpan = sliderInfo.Length * sliderInfo.SlideRepeat;
            else
                sliderInfo.TotalSliderSpan = sliderInfo.Length;
        }
    }
}