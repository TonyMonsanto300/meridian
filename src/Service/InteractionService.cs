using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;

namespace XenWorld.src.Service {
    public static class InteractionService {
        public static bool PerformInteraction(InteractionMode mode) {

            switch (mode) {
                case InteractionMode.Attack:
                    AttackService.AttackCell(PlayerManager.Controller.Puppet, MapCursorService.TargetCell);
                    return true;
                case InteractionMode.Scan:
                    ScanService.ScanCell(MapCursorService.TargetCell);
                    return true;
                case InteractionMode.Mine:
                    MiningService.MineCell(MapCursorService.TargetCell);
                    return true;
                case InteractionMode.Cast:
                    // Implement casting logic here
                    return false;
                case InteractionMode.Skill:
                    return false;
                default:
                    return false;
            }
        }
    }
}
