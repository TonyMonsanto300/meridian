using Microsoft.Xna.Framework.Input;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model;
using XenWorld.src.Service;
using XenWorld.src.Manager;
using System.Linq;
using Microsoft.Xna.Framework;
using System;

namespace XenWorld.src.Controller.Player {
    public static class KeyScanner {
        public static KeyboardState LastState;
        private static readonly KeyBindDictionary keyBindDictionary;

        static KeyScanner() {
            keyBindDictionary = new KeyBindDictionary();
            keyBindDictionary.SeedDefaultBindings();
        }

        public static void InputScan(GameTime gameTime) {
            KeyboardState currentState = Keyboard.GetState();
            bool moved = false;

            // Get currently pressed keys
            var pressedKeys = currentState.GetPressedKeys();

            // Ensure that only new key presses are processed
            var newKeys = pressedKeys.Except(LastState.GetPressedKeys());

            if (newKeys != null && newKeys.Any()) {
                foreach (var key in newKeys) {
                    if (keyBindDictionary.TryGetBinding(key, out KeyBinding binding)) {
                        binding.Execute();
                    } else {
                        // Handle keys not covered by KeyBindings
                        HandleUnboundKeys(currentState, key, gameTime, ref moved);
                    }
                }
            }

            // Update previous keyboard state for next cycle
            LastState = currentState;
        }

        private static void HandleUnboundKeys(KeyboardState currentState, Keys key, GameTime gameTime, ref bool moved) {
            // Handle action keys for selecting AbilityClass or casting Ability
            if (PlayerManager.Controller.IsChoosingClass || PlayerManager.Controller.IsCasting) {
                HandleCastingKeys(key);
            } else if (PlayerManager.Controller.CurrentMode != InteractionMode.None) {
                // Handle cursor movement within interaction mode
                HandleCursorMovement(currentState, LastState, gameTime);
            } else {
                // If we're not in an interaction mode, handle normal movement
                moved = HandleMovement(key);
                if (moved) {
                    PlayerManager.Controller.TakeTurn();
                }
            }
        }

        private static void HandleCastingKeys(Keys key) {
            var isActionKey = KeyValidator.IsValidActionKey(key, PlayerManager.Controller.CurrentMode);

            if (isActionKey) {
                var number = KeyMapper.NumberKeys[key]; // Extract the number from the key press
                if (number > 0) {
                    if (PlayerManager.Controller.IsChoosingClass) {
                        PlayerManager.Controller.SelectAbilityClass(number);
                    } else if (PlayerManager.Controller.IsCasting) {
                        PlayerManager.Controller.SelectAbilityFromClass(number);
                    }
                }
            }
        }
        private static TimeSpan lastInputTime = TimeSpan.Zero;
        private static TimeSpan inputCooldown = TimeSpan.FromMilliseconds(100);  // Cooldown before accepting new input
        private static void HandleCursorMovement(KeyboardState currentState, KeyboardState lastState, GameTime gameTime) {
            // Check if the current game time is less than last input time plus cooldown
            if (gameTime.TotalGameTime < lastInputTime + inputCooldown) {
                return;  // Do not process input if the cooldown has not elapsed
            }

            bool up = IsNewKeyPress(Keys.Up, currentState, lastState);
            bool down = IsNewKeyPress(Keys.Down, currentState, lastState);
            bool left = IsNewKeyPress(Keys.Left, currentState, lastState);
            bool right = IsNewKeyPress(Keys.Right, currentState, lastState);

            if (up && right) {
                MapCursorService.MoveCursor(PuppetDirection.NorthEast);
            } else if (up && left) {
                MapCursorService.MoveCursor(PuppetDirection.NorthWest);
            } else if (down && right) {
                MapCursorService.MoveCursor(PuppetDirection.SouthEast);
            } else if (down && left) {
                MapCursorService.MoveCursor(PuppetDirection.SouthWest);
            } else if (up) {
                MapCursorService.MoveCursor(PuppetDirection.North);
            } else if (down) {
                MapCursorService.MoveCursor(PuppetDirection.South);
            } else if (left) {
                MapCursorService.MoveCursor(PuppetDirection.West);
            } else if (right) {
                MapCursorService.MoveCursor(PuppetDirection.East);
            }

            // Update the last input time to the current game time
            lastInputTime = gameTime.TotalGameTime;
        }

        private static bool IsNewKeyPress(Keys key, KeyboardState currentState, KeyboardState lastState) {
            return currentState.IsKeyDown(key) && lastState.IsKeyUp(key);
        }

        private static bool HandleMovement(Keys key) {
            int moveX = 0;
            int moveY = 0;

            switch (key) {
                case Keys.Up:
                    moveY -= 1;
                    break;
                case Keys.Down:
                    moveY += 1;
                    break;
                case Keys.Left:
                    moveX -= 1;
                    break;
                case Keys.Right:
                    moveX += 1;
                    break;
            }

            if (moveX != 0 || moveY != 0) {
                return MoveService.MovePlayer(new Coordinate(
                    PlayerManager.Controller.Puppet.Location.X + moveX,
                    PlayerManager.Controller.Puppet.Location.Y + moveY));
            }

            return false;
        }
    }
}