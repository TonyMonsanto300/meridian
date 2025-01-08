using System;
using System.Linq;
using XenWorld.Controller;
using XenWorld.src.Manager;

namespace XenWorld.src.Service {
    public static class ScanService {
        public static void ScanCell(MapCell targetCell) {
            DummyController controller = null;
            if (targetCell.Occupied) {
                controller = DummyManager.DummyControllers.ToList().Where(x => x.Puppet == targetCell.Occupant).First();
                if (controller != null) {
                    LogRenderer.AddLogMessage(controller.Message);
                } else {
                    throw new Exception("Cant find controller for this dummy"); //TODO: Add to MagicStrings
                    //NOTE: Possibly create a fallback to reverse generate a new controller for unthered dummies
                    //-- They would likely become jobless due to loss of controller info
                }
            }
        }
    }
}