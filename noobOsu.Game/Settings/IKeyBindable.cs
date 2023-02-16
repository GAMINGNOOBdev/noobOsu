using System.Collections.Generic;

namespace noobOsu.Game.Settings
{
    public interface IKeyBindable
    {
        IReadOnlyList<IKeybind> Keybinds { get; }

        void AddKeybind(IKeybind binding);
        void ClearKeybinds();
    }
}