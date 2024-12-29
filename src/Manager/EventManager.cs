using System;
using System.Collections.Generic;
using System.Linq;
using XenWorld.Controller;
using XenWorld.src.Manager;

namespace XenWorld.Manager {
    internal class EventManager {

        public static void PlayerTurn() {
            // Get the player's coordinates (ensure player is not null)
            var playerX = PlayerManager.Controller.Puppet.Coordinate.X;
            var playerY = PlayerManager.Controller.Puppet.Coordinate.Y;

            // Filter NPCs within 500 units using Manhattan distance (sum of the absolute differences of their X and Y coordinates)
            var nearbyControllers = NPCManager.NPCControllers
                .Where(controller =>
                    Math.Abs(controller.Puppet.Coordinate.X - playerX) +
                    Math.Abs(controller.Puppet.Coordinate.Y - playerY) <= 500)
                .ToList(); // Convert the filtered results to a list

            // Call TakeTurn only for NPCs within the 500 unit range
            nearbyControllers.ForEach(controller => controller.TakeTurn());
        }
    }
}