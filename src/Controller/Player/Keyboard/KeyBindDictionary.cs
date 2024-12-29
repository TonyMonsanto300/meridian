using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Service;

public class KeyBindDictionary {
    private readonly Dictionary<Keys, KeyBinding> keyBindings;

    public KeyBindDictionary() {
        keyBindings = new Dictionary<Keys, KeyBinding>();
    }

    // Method to get the binding for a specific key
    public bool TryGetBinding(Keys key, out KeyBinding binding) {
        return keyBindings.TryGetValue(key, out binding);
    }

    // Method to seed default bindings
    public void SeedDefaultBindings() {
        // Escape key binding
        keyBindings.Add(Keys.Escape, new KeyBinding(Keys.Escape, () => {
            PlayerManager.Controller.ResetCasting();
            PlayerManager.Controller.ExitCurrentMode();
        }));

        // Weapon cycle key binding (W key)
        keyBindings.Add(Keys.W, new KeyBinding(Keys.W, () => {
            WeaponService.CycleEquippedWeapon(PlayerManager.Controller.Puppet);
        }));

        // Mining key binding (M key)
        keyBindings.Add(Keys.M, new KeyBinding(Keys.M, () => {
            if (PlayerManager.Controller.CurrentMode == InteractionMode.Mine) {
                if (InteractionService.PerformInteraction(PlayerManager.Controller.CurrentMode)) {
                    PlayerManager.Controller.TakeTurn();
                }
                PlayerManager.Controller.ExitCurrentMode(); // Exit mining mode after action
            } else if (PlayerManager.Controller.CurrentMode == InteractionMode.None) {
                PlayerManager.Controller.EnterMode(InteractionMode.Mine); // Enter mining mode
            }
        }));

        // Attack key binding (A key)
        keyBindings.Add(Keys.A, new KeyBinding(Keys.A, () => {
            if (PlayerManager.Controller.CurrentMode == InteractionMode.Attack) {
                if (InteractionService.PerformInteraction(PlayerManager.Controller.CurrentMode)) {
                    PlayerManager.Controller.TakeTurn();
                }
                PlayerManager.Controller.ExitCurrentMode(); // Exit attack mode after action
            } else if (PlayerManager.Controller.CurrentMode == InteractionMode.None) {
                PlayerManager.Controller.EnterMode(InteractionMode.Attack); // Enter attack mode
            }
        }));

        // Scan key binding (S key)
        keyBindings.Add(Keys.S, new KeyBinding(Keys.S, () => {
            if (PlayerManager.Controller.CurrentMode == InteractionMode.Scan) {
                if (InteractionService.PerformInteraction(PlayerManager.Controller.CurrentMode)) {
                    PlayerManager.Controller.TakeTurn();
                }
                PlayerManager.Controller.ExitCurrentMode(); // Exit scan mode after action
            } else if (PlayerManager.Controller.CurrentMode == InteractionMode.None) {
                PlayerManager.Controller.EnterMode(InteractionMode.Scan); // Enter scan mode
            }
        }));

        // Casting key binding (C key)
        keyBindings.Add(Keys.C, new KeyBinding(Keys.C, () => {
            if (!PlayerManager.Controller.IsCasting && !PlayerManager.Controller.IsChoosingClass
                && PlayerManager.Controller.CurrentMode == InteractionMode.None) {
                PlayerManager.Controller.CurrentMode = InteractionMode.Cast;
                PlayerManager.Controller.IsChoosingClass = true; // Start AbilityClass selection phase
            }
        }));

        // Confirm interaction key binding (Enter key)
        keyBindings.Add(Keys.Enter, new KeyBinding(Keys.Enter, () => {
            if (PlayerManager.Controller.CurrentMode != InteractionMode.None) {
                if (InteractionService.PerformInteraction(PlayerManager.Controller.CurrentMode)) {
                    PlayerManager.Controller.TakeTurn();
                }
                PlayerManager.Controller.ExitCurrentMode();
            }
        }));
    }

    // Optional methods to add or remove bindings dynamically
    public void AddBinding(Keys key, Action action) {
        if (!keyBindings.ContainsKey(key)) {
            keyBindings.Add(key, new KeyBinding(key, action));
        }
    }

    public void RemoveBinding(Keys key) {
        keyBindings.Remove(key);
    }
}