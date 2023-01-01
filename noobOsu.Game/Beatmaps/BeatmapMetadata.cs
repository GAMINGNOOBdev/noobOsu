namespace noobOsu.Game.Beatmaps
{
    public class BeatmapMetadata
    {
        // beatmap title in it's original language
        public string Title { get; set; }

        // romanized title
        public string TitleUnicode { get; set; }
        
        // song artist name in the original language
        public string Artist { get; set; }

        // romanized artist name
        public string ArtistUnicode { get; set; }

        // beatmap creator
        public string Author { get; set; }

        // difficulty name
        public string Version { get; set; }

        // source of the song
        public string Source { get; set; }

        // used for searching, tags
        public string[] Tags { get; set; }

        // beatmap id/beatmap set id for web access
        public int BeatmapID { get; set; }
        public int BeatmapSetID { get; set; }
    }
}