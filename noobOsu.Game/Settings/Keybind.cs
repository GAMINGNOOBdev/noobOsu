using System;
using osuTK.Input;
using osu.Framework.Bindables;

namespace noobOsu.Game.Settings
{
    public interface IKeybind
    {
        object GetKey();
        object GetAction();
    }

    public interface ITypedKeybind<T> : IKeybind
    {
        T Key { get; }
    }

    public class KeybindAdapter<T> : ITypedKeybind<T>
    {
        public virtual T Key { get; set; }

        public KeybindAdapter()
        {
        }

        public KeybindAdapter(IKeybind bind)
        {
            Key = (T)bind.GetKey();
        }

        public virtual object GetKey() => Key;
        public virtual object GetAction() => null;
    }

    public class Keybind<T> : KeybindAdapter<T>
    {
        private SerializableKeybind<T> serializable;

        public Bindable<Action> BoundAction { get; set; }
        public override object GetAction() => BoundAction;

        public Keybind(T key, Action action)
        {
            Key = key;
            BoundAction = new Bindable<Action>();
            BoundAction.Value = action;

            serializable = new SerializableKeybind<T>();
            serializable.Key = key;
            serializable.BoundAction = action.Method.Name;
        }

        public Keybind(IKeybind keybind, IKeyBindable boundClass) : base(keybind)
        {
            BoundAction = new Bindable<Action>();
            if (keybind.GetAction() != null)
            {
                if (keybind.GetAction() is Action)
                    BoundAction.Value = (Action)keybind.GetAction();
                else
                    BoundAction.Value = Util.ClassUtil.GetAction(boundClass, (string)keybind.GetAction());
            }

            serializable = new SerializableKeybind<T>(keybind);
        }

        public void Rebind(IKeyBindable boundClass)
        {
            BoundAction.Value = Util.ClassUtil.GetAction(boundClass, serializable.BoundAction);
        }
        
        public override string ToString()
        {
            return "Keybind<" + typeof(T).ToString() + "> ( Key: " + Key.ToString() + " BoundAction: " + BoundAction.ToString() + " )";
        }
    }

    public class SerializableKeybind<T> : KeybindAdapter<T>
    {
        public string BoundAction { get; set; }
        public override object GetAction() => BoundAction;

        public SerializableKeybind()
        {
        }

        public SerializableKeybind(IKeybind bind) : base(bind)
        {
        }

        public override string ToString()
        {
            return "SerializableKeybind<" + typeof(T).ToString() + "> ( Key: " + Key.ToString() + " BoundAction: " + BoundAction.ToString() + " )";
        }
    }
}