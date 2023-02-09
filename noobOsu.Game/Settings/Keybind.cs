using System;
using osuTK.Input;
using osu.Framework.Bindables;

namespace noobOsu.Game.Settings
{
    public interface IKeybind<T>
    {
        T Key { get; }
    }

    public struct Keybind<T> : IKeybind<T>
    {
        public T Key { get; set; }
        public Bindable<Action> BoundAction { get; set; }
        
        public override string ToString()
        {
            return "Keybind<" + typeof(T).ToString() + "> ( Key: " + Key.ToString() + " BoundAction: " + BoundAction.ToString() + " )";
        }
    }

    public struct SerializableKeybind<T> : IKeybind<T>
    {
        public T Key { get; set; }
        public string BoundAction { get; set; }

        public override string ToString()
        {
            return "SerializableKeybind<" + typeof(T).ToString() + "> ( Key: " + Key.ToString() + " BoundAction: " + BoundAction.ToString() + " )";
        }
    }
}