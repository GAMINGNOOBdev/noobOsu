using System.IO;
using osu.Framework.Bindables;
using noobOsu.Game.Skins;
using System.Collections.Generic;

namespace noobOsu.Game.Settings
{
    public class GameSettings
    {
        public static GameSettings INSTANCE { get; private set; }
        public static List<ISkin> Skins { get; private set; } = new List<ISkin>();
        
        public static ISkin GetCurrentSkin()
        {
            if (INSTANCE == null)
                return null;

            return INSTANCE.CurrentSkin.Value;
        }

        public Bindable<ISkin> CurrentSkin = new Bindable<ISkin>();
        public string LastSkinName = "default";
        public BindableBool UseBeatmapSkins = new BindableBool(true);
        public BindableBool UseBeatmapHitsounds = new BindableBool(true);
        public BindableBool UseBeatmapColors = new BindableBool(true);
        
        public GameSettings()
        {
            if (INSTANCE != null) return;
            INSTANCE = this;

            ReadSettings();

            CurrentSkin.BindValueChanged(
                (value) => {
                    if (value != null)
                        INSTANCE.LastSkinName = value.NewValue.General.Name;
                }
            );

            foreach (string s in ISkin.GetAvailableSkinFolders())
            {
                Skin skin = new Skin();
                skin.DirectoryName = s;
                skin.LoadSkin("Skins/" + s + "/skin.ini");
                Skins.Add(skin);

                if (skin.General.Name.Equals(LastSkinName))
                {
                    CurrentSkin.Value = skin;
                }
            }
            if (CurrentSkin.Value == null)
                CurrentSkin.Value = FindSkin("default");
        }

        public void Dispose()
        {
            SaveSettings();
        }

        private void ReadSettings()
        {
            if (!File.Exists("settings.ini"))
                return;

            StreamReader SettingsReader = new StreamReader("settings.ini");

            string line;
            string[] splitLine;
            while ((line = SettingsReader.ReadLine()) != null)
            {
                line = line.TrimStart();
                if (line.Equals("") || line.StartsWith("//")) continue;

                splitLine = line.Split(" = ");

                if (splitLine.Length < 2) continue;

                if (splitLine[0].Equals("LastSkinName"))
                {
                    LastSkinName = splitLine[1];
                }
                if (splitLine[0].Equals("UseBeatmapSkins"))
                {
                    UseBeatmapSkins.Value = bool.Parse(splitLine[1]);
                }
                if (splitLine[0].Equals("UseBeatmapHitsounds"))
                {
                    UseBeatmapHitsounds.Value = bool.Parse(splitLine[1]);
                }
                if (splitLine[0].Equals("UseBeatmapColors"))
                {
                    UseBeatmapColors.Value = bool.Parse(splitLine[1]);
                }
            }

            SettingsReader.Dispose();
            SettingsReader.Close();
        }

        private void SaveSettings()
        {
            // go back to the beginning of the file to overwrite everything
            StreamWriter SettingsWriter = new StreamWriter("settings.ini");
            
            SettingsWriter.WriteLine("LastSkinName = " + LastSkinName);
            SettingsWriter.WriteLine("UseBeatmapSkins = " + UseBeatmapSkins.Value);
            SettingsWriter.WriteLine("UseBeatmapHitsounds = " + UseBeatmapHitsounds.Value);
            SettingsWriter.WriteLine("UseBeatmapColors = " + UseBeatmapColors.Value);

            SettingsWriter.Dispose();
            SettingsWriter.Close();
        }

        private ISkin FindSkin(string name)
        {
            foreach (ISkin skin in Skins)
            {
                if (skin.General.Name.Equals(name))
                    return skin;
            }
            return null;
        }
    }
}