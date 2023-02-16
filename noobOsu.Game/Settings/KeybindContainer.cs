using System;
using System.IO;
using osuTK.Input;
using osu.Framework.Logging;
using osu.Framework.Bindables;
using System.Collections.Generic;

namespace noobOsu.Game.Settings
{
    public class KeybindContainer
    {
        private readonly List<SerializableKeybind<Key>> KeybindList = new List<SerializableKeybind<Key>>();
        private readonly List<Keybind<Key>> ActiveKeybinds = new List<Keybind<Key>>();

        public IReadOnlyList<SerializableKeybind<Key>> Keybinds => KeybindList;

        public void BindTo(IKeyBindable boundObject)
        {
            ActiveKeybinds.Clear();
            foreach (ITypedKeybind<Key> bind in Keybinds)
            {
                Keybind<Key> keybind = new Keybind<Key>(bind, boundObject);
                boundObject.AddKeybind(keybind);
                ActiveKeybinds.Add(keybind);
            }
        }

        public void ReadKeybinds()
        {
            if (!File.Exists("keybinds.ini"))
                return;

            KeybindList.Clear();
            StreamReader KeybindsReader = new StreamReader("keybinds.ini");

            string line;
            string[] splitLine;
            SerializableKeybind<Key> keybind;
            while ((line = KeybindsReader.ReadLine()) != null)
            {
                line = line.TrimStart();
                if (line.Equals("") || line.StartsWith("//")) continue;

                splitLine = line.Split(" = ");

                if (splitLine.Length < 2) continue;

                if (splitLine[0].Length < 2)
                    splitLine[0] = splitLine[0].ToUpper();

                if (!Enum.IsDefined(typeof(Key), splitLine[0]))
                    continue;

                keybind = new SerializableKeybind<Key>();
                
                keybind.Key = (Key)Enum.Parse(typeof(Key), splitLine[0]);
                keybind.BoundAction = splitLine[1];

                Logger.Log("keybind: " + keybind.ToString(), level: LogLevel.Important);

                KeybindList.Add(keybind);
            }

            KeybindsReader.Dispose();
            KeybindsReader.Close();
        }

        public void SaveKeybinds()
        {
            StreamWriter KeybindsWriter = new StreamWriter("keybinds.ini");
            
            foreach (SerializableKeybind<Key> bind in KeybindList)
            {
                KeybindsWriter.WriteLine(bind.Key.ToString() + " = " + bind.BoundAction);
            }

            KeybindsWriter.Dispose();
            KeybindsWriter.Close();
        }
    }
}