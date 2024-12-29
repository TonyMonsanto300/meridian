using Microsoft.Xna.Framework.Input;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model;
using XenWorld.src.Service;
using XenWorld.src.Manager;
using System.Linq;

namespace XenWorld.src.Controller.Player {
    public static class InputScanner {
        public static KeyboardState LastState;
        private static readonly KeyBindDictionary keyBindDictionary;

        static InputScanner() {
            keyBindDictionary = new KeyBindDictionary();
            keyBindDictionary.SeedDefaultBindings();
        }

        public static void InputScan() {
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
                        HandleUnboundKeys(currentState, key, ref moved);
                    }
                }
            }

            // Update previous keyboard state for next cycle
            LastState = currentState;
        }

        private static void HandleUnboundKeys(KeyboardState currentState, Keys key, ref bool moved) {
            // Handle action keys for selecting AbilityClass or casting Ability
            if (PlayerManager.Controller.IsChoosingClass || PlayerManager.Controller.IsCasting) {
                HandleCastingKeys(key);
            } else if (PlayerManager.Controller.CurrentMode != InteractionMode.None) {
                // Handle cursor movement within interaction mode
                HandleCursorMovement(currentState, LastState);
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

        private static void HandleCursorMovement(KeyboardState currentState, KeyboardState lastState) {
            int deltaX = 0;
            int deltaY = 0;

            // Movement keys
            bool leftPressed = currentState.IsKeyDown(Keys.Left);
            bool rightPressed = currentState.IsKeyDown(Keys.Right);
            bool upPressed = currentState.IsKeyDown(Keys.Up);
            bool downPressed = currentState.IsKeyDown(Keys.Down);

            // Check for new key presses to prevent continuous movement
            bool leftNewPress = IsNewKeyPress(Keys.Left, currentState, lastState);
            bool rightNewPress = IsNewKeyPress(Keys.Right, currentState, lastState);
            bool upNewPress = IsNewKeyPress(Keys.Up, currentState, lastState);
            bool downNewPress = IsNewKeyPress(Keys.Down, currentState, lastState);

            // Determine movement deltas
            if (leftNewPress) deltaX -= 1;
            if (rightNewPress) deltaX += 1;
            if (upNewPress) deltaY -= 1;
            if (downNewPress) deltaY += 1;

            // Only move if there's a new key press
            if (deltaX != 0 || deltaY != 0) {
                MapCursorService.MoveCursorByDelta(deltaX, deltaY);
            }
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
                    PlayerManager.Controller.Puppet.Coordinate.X + moveX,
                    PlayerManager.Controller.Puppet.Coordinate.Y + moveY));
            }

            return false;
        }
    }
}