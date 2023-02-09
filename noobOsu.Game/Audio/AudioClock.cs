using osu.Framework.Timing;
using osu.Framework.Audio.Track;

namespace noobOsu.Game.Audio
{
    public partial class AudioClock : ManualClock, IFrameBasedClock
    {
        private FrameTimeInfo AudioFrameTimeInfo = new FrameTimeInfo();
        private Track clock_timing_track;
        private double LastTime, GeneralOffset;
        
        public double MapOffset
        {
            get => GeneralOffset;
            set
            {
                if (GeneralOffset == value)
                    return;

                GeneralOffset = value;
            }
        }

        public double ElapsedFrameTime
        {
            get => CurrentTime - LastTime;
            set => throw new System.NotSupportedException("setting of the elapsed time not supported, can only be dynamically calculated by this class");
        }
        public double FramesPerSecond => 0;
        public FrameTimeInfo TimeInfo
        {
            get => AudioFrameTimeInfo;
            set
            {
                throw new System.NotSupportedException("this field can only be accessed!");
            }
        }

        public AudioClock() : base()
        {
        }

        public AudioClock(Track audio) : base()
        {
            clock_timing_track = audio;
        }

        public void SetTrack(Track audio)
        {
            clock_timing_track = audio;
        }

        public Track GetTrack() => clock_timing_track;

        public void ProcessFrame()
        {
            LastTime = CurrentTime;
            CurrentTime = clock_timing_track.CurrentTime + MapOffset;

            AudioFrameTimeInfo.Current = CurrentTime;
            AudioFrameTimeInfo.Elapsed = ElapsedFrameTime;
        }

        public override string ToString()
        {
            return "AudioClock ( " + System.Math.Truncate(CurrentTime) + "ms )";
        }
    }
}