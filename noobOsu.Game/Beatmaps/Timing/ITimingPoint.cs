namespace noobOsu.Game.Beatmaps.Timing
{
    public interface ITimingPoint
    {
        // time (in ms) of this current info point
        int TimeStamp { get; }

        // for uninherited points: BPM
        // for inherited points: inversed slider velocity multiplies of this timestamp until it is changed
        double BeatLength { get; }

        // amount of beats in one measure, only used by uninherited points
        int Meter { get; }

        // sample set (0=default, 1=normal, 2=soft, 3=drum)
        int SampleSet { get; }
        
        // sample index (0=osu default, rest is used defined)
        int SampleIndex { get; }

        // volume in percentace
        int Volume { get; }

        // if object is uninherited, this should be one, else zero. no other values than zero and one allowed
        int Uninherited { get; }

        // effects bitfield (bit zero=kiai time enable)
        int Effects { get; }

        // return as a sample file name
        string GetHitsound(int hitsound);

        // get as a string
        string ToString();

        // to clone an existing timing point
        ITimingPoint Clone();

        // get the default timing hitsound
        static string GetDefaultHitsound(int hitsound)
        {
            return GetSampleSetName(0) + "-" + GetSampleSoundName(hitsound);
        }

        static string GetSampleSetName(int setIdx)
        {
            switch (setIdx)
            {
                case 2:
                    return "soft";

                case 3:
                    return "drum";

                case 0:
                case 1:
                default:
                    return "normal";
            }
        }

        static string GetSampleSoundName(int soundIdx)
        {
            if((soundIdx & (1 << 0)) > 0)
                return "hitnormal";
            if((soundIdx & (1 << 1)) > 0)
                return "hitwhistle";
            if((soundIdx & (1 << 2)) > 0)
                return "hitfinish";
            if((soundIdx & (1 << 3)) > 0)
                return "hitclap";

                
            return "hitnormal";
        }
    }
}