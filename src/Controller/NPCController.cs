using System;
using XenWorld.Model.Map;
using XenWorld.Model;
using XenWorld.Manager;
using XenWorld.src.Manager;
using XenWorld.src.Service;

namespace XenWorld.Controller {
    public enum NPCType {
        Idle,
        Enemy
    }

    public class NPCController {
        private Puppet enemy;
        private Puppet? player = null;
        private ZoneMap mapGrid;
        private NPCType type;

        public Puppet Puppet { get { return enemy; } set { enemy = value; } }
        private Puppet? target = null;

        private int scanRange = 4;
        private int patrolDistance = 3;

        private int originX, originY;
        private int patrolDirection = -1; // -1 for up, 1 for down

        public NPCType NPCType { get { return type; } set { type = value; } }

        public NPCController(Puppet enemy, PlayerController playerController, ZoneMap mapGrid, NPCType type = NPCType.Idle) {
            this.enemy = enemy;
            this.player = playerController.Puppet;
            this.mapGrid = mapGrid;
            this.type = type;

            // Set origin position for patrol
            originX = enemy.Coordinate.X;
            originY = enemy.Coordinate.Y;
        }

        public void TakeTurn() {
            if (enemy == null) {
                return; // Prevent actions if the enemy is null
            }

            if (type == NPCType.Idle) {
                return; // If NPC is idle, do nothing
            }

            bool canAttack = false;

            // Step 1: Scan for player
            if (target == null) {
                ScanForPlayer();
            }

            // Step 2: Check if NPC can attack first
            if (target != null) {
                canAttack = CheckAndAttackPlayer(); // If we can attack, we do so
            }

            // Step 3: If we can't attack, move towards the player or patrol if no target found
            if (!canAttack) {
                if (target != null) {
                    MoveTowardTarget();
                } else {
                    Patrol();
                }
            }
        }

        private void ScanForPlayer() {
            target = null; // Reset target before scanning.

            int startX = Math.Max(0, enemy.Coordinate.X - scanRange);
            int endX = Math.Min(mapGrid.Width - 1, enemy.Coordinate.X + scanRange);
            int startY = Math.Max(0, enemy.Coordinate.Y - scanRange);
            int endY = Math.Min(mapGrid.Height - 1, enemy.Coordinate.Y + scanRange);

            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    MapCell cell = mapGrid.Grid[x, y];
                    if (cell.Occupant != null && cell.Occupant != enemy) {
                        // Set the target to the found occupant (player or another enemy)
                        target = cell.Occupant;
                        return; // Exit early if a target is found
                    }
                }
            }
        }

        private bool CheckAndAttackPlayer() {
            if (player == null) return false; // If no player, can't attack

            int deltaX = Math.Abs(player.Coordinate.X - enemy.Coordinate.X);
            int deltaY = Math.Abs(player.Coordinate.Y - enemy.Coordinate.Y);

            // Check if the player is in any adjacent cell (including diagonal)
            if (deltaX <= 1 && deltaY <= 1) {
                Attack(player); // Perform the attack
                return true; // Attack was successful
            }

            return false; // Can't attack
        }

        private bool MoveTowardTarget() {
            if (player == null) return false; // If player is null, can't move towards them

            int deltaX = player.Coordinate.X - enemy.Coordinate.X;
            int deltaY = player.Coordinate.Y - enemy.Coordinate.Y;

            int moveX = 0;
            int moveY = 0;

            // Determine the direction to move based on the player's position
            if (deltaX != 0) {
                moveX = deltaX > 0 ? 1 : -1;
            }
            if (deltaY != 0) {
                moveY = deltaY > 0 ? 1 : -1;
            }

            // Try diagonal movement
            if (TryMoveEnemyTo(enemy.Coordinate.X + moveX, enemy.Coordinate.Y + moveY)) {
                return true; // Successfully moved diagonally
            }

            // If diagonal movement isn't possible, try horizontal movement
            if (moveX != 0 && TryMoveEnemyTo(enemy.Coordinate.X + moveX, enemy.Coordinate.Y)) {
                return true; // Successfully moved horizontally
            }

            // If horizontal movement isn't possible, try vertical movement
            if (moveY != 0 && TryMoveEnemyTo(enemy.Coordinate.X, enemy.Coordinate.Y + moveY)) {
                return true; // Successfully moved vertically
            }

            return false; // No movement possible
        }

        private bool Patrol() {
            int newY = enemy.Coordinate.Y + patrolDirection;

            // Ensure NPC stays within patrol distance from origin
            int distanceFromOrigin = Math.Abs(newY - originY);
            if (distanceFromOrigin > patrolDistance) {
                patrolDirection *= -1; // Reverse patrol direction
                newY = enemy.Coordinate.Y + patrolDirection; // Recalculate new position
            }

            // Try moving to the new position
            if (TryMoveEnemyTo(enemy.Coordinate.X, newY)) {
                return true; // Successful move
            } else {
                // Can't move, change direction
                patrolDirection *= -1;
            }

            return false; // Patrol failed
        }

        private bool TryMoveEnemyTo(int newX, int newY) {
            if (newX < 0 || newX >= mapGrid.Width || newY < 0 || newY >= mapGrid.Height) {
                return false; // Out of bounds
            }

            MapCell targetCell = mapGrid.Grid[newX, newY];

            // Check if the target cell is empty and passable
            if (targetCell.Occupant == null && !targetCell.Terrain.Obstacle) {
                // Move the NPC to the new position
                mapGrid.Grid[enemy.Coordinate.X, enemy.Coordinate.Y].Occupant = null;
                enemy.Coordinate.X = newX;
                enemy.Coordinate.Y = newY;
                targetCell.Occupant = enemy;

                return true; // Successful move
            }

            return false; // Can't move to this cell
        }

        private void Attack(Puppet attackTarget) {
            if (attackTarget != null) {
                AttackService.AttackPuppet(enemy, attackTarget);
            }
        }
    }
}
