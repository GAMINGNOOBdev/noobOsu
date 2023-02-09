using System.IO;
using osu.Framework.Bindables;
using noobOsu.Game.Skins;
using System.Collections.Generic;

namespace noobOsu.Game.Settings
{
    public static class GameSettings
    {
        public static List<ISkin> Skins { get; private set; } = new List<ISkin>();
        
        public static ISkin GetCurrentSkin()
        {
            return CurrentSkin.Value;
        }
        public static float GetMasterVolume() => (float)(MasterVolume.Value / 100f);
        public static float GetMusicVolume() => (float)(MusicVolume.Value / 100f);
        public static float GetEffectVolume() => (float)(EffectVolume.Value / 100f);

        public static Bindable<ISkin> CurrentSkin = new Bindable<ISkin>();
        public static string LastSkinName = "default";
        public static BindableBool UseBeatmapSkins = new BindableBool(true);
        public static BindableBool UseBeatmapHitsounds = new BindableBool(true);
        public static BindableBool UseBeatmapColors = new BindableBool(true);
        public static BindableInt BeatmapBackgroundDim = new BindableInt(0){ MinValue = 0, MaxValue = 100, Default = 90 };
        public static BindableInt MasterVolume = new BindableInt(0){ MinValue = 0, MaxValue = 100, Default = 100 };
        public static BindableInt MusicVolume = new BindableInt(0){ MinValue = 0, MaxValue = 100, Default = 100 };
        public static BindableInt EffectVolume = new BindableInt(0){ MinValue = 0, MaxValue = 100, Default = 100 };
        public static KeybindContainer GeneralKeybinds = new KeybindContainer();
        
        public static void Init()
        {
            ReadSettings();
            GeneralKeybinds.ReadKeybinds();

            CurrentSkin.BindValueChanged(
                (value) => {
                    if (value != null)
                        LastSkinName = value.NewValue.General.Name;
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

        public static void Dispose()
        {
            SaveSettings();
            GeneralKeybinds.SaveKeybinds();
        }

        private static void ReadSettings()
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
                if (splitLine[0].Equals("BeatmapBackgroundDim"))
                {
                    BeatmapBackgroundDim.Value = int.Parse(splitLine[1]);
                }
                if (splitLine[0].Equals("MasterVolume"))
                {
                    MasterVolume.Value = int.Parse(splitLine[1]);
                }
                if (splitLine[0].Equals("MusicVolume"))
                {
                    MusicVolume.Value = int.Parse(splitLine[1]);
                }
                if (splitLine[0].Equals("EffectVolume"))
                {
                    EffectVolume.Value = int.Parse(splitLine[1]);
                }
            }

            SettingsReader.Dispose();
            SettingsReader.Close();
        }

        private static void SaveSettings()
        {
            // go back to the beginning of the file to overwrite everything
            StreamWriter SettingsWriter = new StreamWriter("settings.ini");
            
            SettingsWriter.WriteLine("LastSkinName = " + LastSkinName);
            SettingsWriter.WriteLine("UseBeatmapSkins = " + UseBeatmapSkins.Value);
            SettingsWriter.WriteLine("UseBeatmapHitsounds = " + UseBeatmapHitsounds.Value);
            SettingsWriter.WriteLine("UseBeatmapColors = " + UseBeatmapColors.Value);
            SettingsWriter.WriteLine("BeatmapBackgroundDim = " + BeatmapBackgroundDim.Value);
            SettingsWriter.WriteLine("MasterVolume = " + MasterVolume.Value);
            SettingsWriter.WriteLine("MusicVolume = " + MusicVolume.Value);
            SettingsWriter.WriteLine("EffectVolume = " + EffectVolume.Value);

            SettingsWriter.Dispose();
            SettingsWriter.Close();
        }

        private static ISkin FindSkin(string name)
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