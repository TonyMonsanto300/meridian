using System;
using System.Linq;
using XenWorld.Controller;
using XenWorld.Model;
using XenWorld.src.Manager;

namespace XenWorld.src.Service {
    public static class AttackService {
        public static bool AttackCell(Puppet attacker, MapCell target) {
            if (target.Occupant != null) {
                var targetController = DummyManager.DummyControllers.Where(controller => controller.Puppet == target.Occupant).FirstOrDefault();
                if (targetController != null) {
                    int damage = DamageService.RollWeaponDamage(attacker.EquippedWeapon);
                    LogRenderer.AddLogMessage($"{attacker.Name} Attacked {targetController.Puppet.Name} for {damage} damage.");
                    bool lethal = targetController.Puppet.TakeDamage(damage);
                    if (lethal) {
                        attacker.AddExperience(25);
                        LogRenderer.AddLogMessage($"{targetController.Puppet.Name} has been defeated.");
                        return true;
                    }

                    if (targetController.NPCType != NPCType.Enemy) {
                        targetController.NPCType = NPCType.Enemy;
                    }
                    return true;
                } else {
                    Console.WriteLine("Target controller not found.");
                    return false;
                }
            } else {
                LogRenderer.AddLogMessage("No target to attack.");
                return false;
            }
        }
        public static bool AttackPuppet(Puppet attacker, Puppet target) {
            if (target != null) {
                // Check if the target is the player's puppet
                bool isPlayer = PlayerManager.Controller.Puppet == target;

                // Roll weapon damage
                int damage = DamageService.RollWeaponDamage(attacker.EquippedWeapon);

                // Log the attack
                LogRenderer.AddLogMessage($"{attacker.Name} attacked {target.Name} for {damage} damage.");

                // Apply damage to the target
                bool lethal = target.TakeDamage(damage);

                if (lethal) {
                    if (isPlayer) {
                        PlayerManager.PartyDead = true;
                        LogRenderer.AddLogMessage("You have been defeated.");
                    } else {
                        attacker.AddExperience(25);
                        LogRenderer.AddLogMessage($"{target.Name} has been defeated.");
                    }
                    return true;
                }

                // No need to change NPC type or search for a controller
                return true;
            } else {
                LogRenderer.AddLogMessage("No target to attack.");
                return false;
            }
        }
    }
}
