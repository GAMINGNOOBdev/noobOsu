using osu.Framework.Logging;

namespace noobOsu.Game.Beatmaps.Timing
{
    public class TimingPoint : ITimingPoint
    {
        // used for cloning objects
        private readonly string ParsedExpression;

        // time (in ms) of this current info point
        public int TimeStamp { get; set; }

        // for inherited points: inversed slider velocity multiplies of this timestamp until it is changed
        public float BeatLength { get; set; }

        // amount of beats in one measure, only used by uninherited points
        public int Meter { get; set; }

        // sample set (0=default, 1=normal, 2=soft, 3=drum)
        public int SampleSet { get; set; }
        
        // sample index (0=osu default, rest is used defined)
        public int SampleIndex { get; set; }

        // volume in percentace
        public int Volume { get; set; }

        // if object is uninherited, this should be one, else zero. no other values than zero and one allowed
        public int Uninherited { get; set; }

        // effects bitfield (bit zero=kiai time enable)
        public int Effects { get; set; }

        public TimingPoint(string expression)
        {
            ParsedExpression = expression;
            string[] object_values = expression.Split(',');
            if (object_values.Length < 8)
            {
                ParsedExpression = null;
                return;
            }
            
            TimeStamp = (int)float.Parse(object_values[0]);
            BeatLength = float.Parse(object_values[1]);
            
            Meter = int.Parse(object_values[2]);
            SampleSet = int.Parse(object_values[3]);
            SampleIndex = int.Parse(object_values[4]);
            Volume = int.Parse(object_values[5]);
            Uninherited = int.Parse(object_values[6]);
            Effects = int.Parse(object_values[7]);
        }

        public ITimingPoint Clone() => new TimingPoint(ParsedExpression);
    }
}