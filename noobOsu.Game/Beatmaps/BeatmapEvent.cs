using System;
using osu.Framework.Logging;
using System.Collections.Generic;

namespace noobOsu.Game.Beatmaps
{
    public class BeatmapEvents
    {
        private readonly List<IBeatmapEvent> events = new List<IBeatmapEvent>();

        public void AddEvent(string eventString)
        {
            try
            {
                string[] object_values = eventString.Split(",");
                
                BeatmapEvent e = new BeatmapEvent();
                e.Type = IBeatmapEvent.ToEventType(object_values[0]);
                if (e.Type == BeatmapEventType.None)
                    throw new FormatException("invalid event string format \"" + eventString + "\"");
                e.StartTime = double.Parse(object_values[1], System.Globalization.CultureInfo.InvariantCulture);

                if (e.Type.Equals(BeatmapEventType.Background) && e.StartTime != 0) return;

                if (e.Type == BeatmapEventType.Background || e.Type == BeatmapEventType.Video)
                {
                    e.Filename = object_values[2];
                    e.xOffset = int.Parse(object_values[3]);
                    e.yOffset = int.Parse(object_values[4]);
                }
                else
                {
                    e.EndTime = double.Parse(object_values[2], System.Globalization.CultureInfo.InvariantCulture);
                }

                events.Add(e);
            }
            catch (Exception e)
            {
                Logger.Log("Exception thrown while adding event, skipping. Exception: " + e.ToString(), level: LogLevel.Error);
            }
        }

        public string GetBackgroundIfPresent() {
            foreach (IBeatmapEvent e in events)
            {
                if (e.Type == BeatmapEventType.Background) return Util.StringUtil.RemoveQuotes(e.Filename);
            }
            return string.Empty;
        }
    }

    public class BeatmapEvent : IBeatmapEvent
    {
        public BeatmapEventType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string Filename { get; set; }
        public int xOffset { get; set; }
        public int yOffset { get; set; }
    }
}