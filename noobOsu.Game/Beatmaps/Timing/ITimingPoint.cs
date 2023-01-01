namespace noobOsu.Game.Beatmaps.Timing
{
    public interface ITimingPoint
    {
        // time (in ms) of this current info point
        int TimeStamp { get; }

        // for uninherited points: BPM
        // for inherited points: inversed slider velocity multiplies of this timestamp until it is changed
        float BeatLength { get; }

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

        ITimingPoint Clone();
    }
}