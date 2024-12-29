using Microsoft.Xna.Framework.Input;
using System;

public class KeyBinding {
    public Keys Key { get; }
    public Action Execute { get; }

    public KeyBinding(Keys key, Action execute) {
        Key = key;
        Execute = execute;
    }
}