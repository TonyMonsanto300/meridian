using System;
using System.Linq;
using XenWorld.src.Manager;

namespace XenWorld.Manager {
    internal class EventManager {

        public static void PlayerTurn() {
            // Get the player's coordinates (ensure player is not null)
            var playerX = PlayerManager.Controller.Puppet.Location.X;
            var playerY = PlayerManager.Controller.Puppet.Location.Y;

            // Filter NPCs within 500 units using Manhattan distance (sum of the absolute differences of their X and Y coordinates)
            var nearbyControllers = DummyManager.DummyControllers
                .Where(controller =>
                    Math.Abs(controller.Puppet.Location.X - playerX) +
                    Math.Abs(controller.Puppet.Location.Y - playerY) <= 500)
                .ToList(); // Convert the filtered results to a list

            // Call TakeTurn only for NPCs within the 500 unit range
            nearbyControllers.ForEach(controller => controller.TakeTurn());
        }
    }
}