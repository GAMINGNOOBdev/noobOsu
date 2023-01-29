using osuTK;
using System.Diagnostics;
using noobOsu.Game.Beatmaps;
using noobOsu.Game.Beatmaps.Timing;

namespace noobOsu.Game.HitObjects
{
    public class HitObject
    {
        private readonly Vector2 position = new Vector2(0);

        public Vector2 Position => position;
        public IBeatmap ParentMap { get; private set; }
        public Vector2 RawPosition { get; private set; }

        public int Time { get; private set; } = 0;
        public int EndTime { get; private set; } = 0;
        public int Type { get; private set; } = 0;
        public int HitSound { get; private set; } = 0;

        public Vector3 Color { get; } = new Vector3(1f);

        public int ComboColorSkip { get; private set; } = 0;
        public ITimingPoint Timing { get; private set; }

        private readonly SliderInfo sliderInfo = new SliderInfo();
        public ISliderInfo SliderInformation => sliderInfo;

        public readonly HitObjectPath Path;

        public HitObject(string expression, IBeatmap parentmap)
        {
            ParentMap = parentmap;

            string[] object_values = expression.Split(',');
            position.X = int.Parse(object_values[0]) / 640f + 0.1f;
            position.Y = int.Parse(object_values[1]) / 512f + 0.125f;
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

        public void setSliderInfo(ITimingPoint timing)
        {
            Timing = timing;

            if (Timing != null)
                if (Timing.BeatLength < 0)
                    EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier * (-100/Timing.BeatLength)) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
                else
                    EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
            else
                EndTime = Time + (int)((sliderInfo.SlideRepeat+1) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
            
            CalculateRepeatTimes(timing);
        }

        private void ParseSliderInfo(string[] object_values)
        {
            sliderInfo.SlideRepeat = int.Parse(object_values[6])-1;
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
                sliderInfo.TotalSliderSpan = (float)Path.GetLength() * sliderInfo.SlideRepeat;
            else
                sliderInfo.TotalSliderSpan = (float)Path.GetLength();
        }

        private void CalculateRepeatTimes(ITimingPoint timing)
        {
            int repeatTime = 0;
            for (int repeat = 1; repeat < sliderInfo.SlideRepeat+1; repeat++)
            {
                if (Timing != null)
                    if (Timing.BeatLength < 0)
                        repeatTime = (int)((repeat) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier * (-100/Timing.BeatLength)) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
                    else
                        repeatTime = (int)((repeat) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
                else
                    repeatTime = (int)((repeat) * Path.GetLength() / (ParentMap.GetInfo().Difficulty.SliderMultiplier) * (ParentMap.GetInfo().Timing.BPM_At(Time)/100));
                
                sliderInfo.RepeatTimingInfo.AddTimingPoint(repeatTime);
            }
        }

        public override string ToString()
        {
            string infoString = GetTypeString(this) + "(x: " + RawPosition.X + " y: " + RawPosition.Y  + " bpm: " + 1 / ParentMap.GetInfo().Timing.BPM_At(Time) * 1000 * 60 + " starttime: " + Time;
            if (isSlider())
            {
                infoString += " endtime: " + EndTime + " repeat: " + SliderInformation.SlideRepeat;
                infoString += " sv(map): " + ParentMap.GetInfo().Difficulty.SliderMultiplier +  " sv(timing): " + Timing.BeatLength;
                infoString += " slider_end_pos: " + Path.GetCurvePoints()[Path.GetCurvePoints().Count-1].ToString() + ")";
            }
            else
                infoString += ")";
            return infoString;
        }

        private static string GetTypeString(HitObject obj)
        {
            if (obj.isCircle())
                return "HitCircle";
            
            if (obj.isSlider())
                return "Slider";

            if (obj.isSpinner())
                return "Spinner";
            
            return "None";
        }
    }
}