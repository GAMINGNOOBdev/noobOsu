namespace noobOsu.Game.Beatmaps
{
    public interface IBeatmapGeneral
    {
        // always has to have a reference to it's parent mapset
        IBeatmapSet ParentSet { get; }

        // name of the mapper
        string MapperName { get; }

        // difficulty name
        string DifficultyName { get; }

        // song name
        string SongName { get; }

        // interpret/musician name
        string InterpretName { get; }

        // to get this beatmaps filename
        string AsFilename();
    }

    public class BeatmapGeneral : IBeatmapGeneral
    {
        public IBeatmapSet ParentSet { get; set; }
        public string MapperName { get; set; }
        public string DifficultyName { get; set; }
        public string SongName { get; set; }
        public string InterpretName { get; set; }

        public BeatmapGeneral(IBeatmapSet parent)
        {
            ParentSet = parent;
        }

        public void SetAttributes(string attribute)
        {
            if (string.IsNullOrEmpty(attribute) || string.IsNullOrWhiteSpace(attribute))
                return;

            if (attribute.EndsWith(".osu"))
                attribute = attribute.Substring(0, attribute.Length-4);

            string[] songAndInterpret = attribute.Substring(0, attribute.LastIndexOf('(')-1).Split('-');

            SongName = songAndInterpret[1].Trim();
            InterpretName = songAndInterpret[0].Trim();

            DifficultyName = attribute.Substring(attribute.IndexOf('[')+1, attribute.IndexOf(']')-attribute.IndexOf('[')-1);
            MapperName = attribute.Substring(attribute.LastIndexOf('(')+1, attribute.LastIndexOf(')')-attribute.LastIndexOf('(')-1);
        }

        public void FromMapset(string mapsetName)
        {
            string[] songAndInterpret = mapsetName.Split('-', 2);

            SongName = songAndInterpret[1].Trim();
            InterpretName = songAndInterpret[0].Trim();
        }

        public string AsFilename()
        {
            return InterpretName + " - " + SongName + " (" + MapperName + ") [" + DifficultyName + "].osu";
        }
    }
}