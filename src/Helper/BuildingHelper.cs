using System;
using XenWorld.Model.Map;
using XenWorld.src.Model.Puppet;
namespace XenWorld.src.Helper {
    public static class BuildingHelper {
        public static PuppetDirection GetDoorDirection(Building building) {
            if (building == null) {
                throw new ArgumentNullException(nameof(building));
            }

            if (building.AnchorCell == null) {
                throw new ArgumentException("Building must have an AnchorCell.", nameof(building));
            }

            if (building.DoorCell == null) {
                throw new ArgumentException("Building must have a DoorCell.", nameof(building));
            }

            int deltaX = building.DoorCell.Coordinate.X - building.AnchorCell.Coordinate.X;
            int deltaY = building.DoorCell.Coordinate.Y - building.AnchorCell.Coordinate.Y;

            // Determine the direction based on deltaX and deltaY
            if (deltaX == 0 && deltaY < 0) {
                return PuppetDirection.North;
            } else if (deltaX > 0 && deltaY == 0) {
                return PuppetDirection.East;
            } else if (deltaX == 0 && deltaY > 0) {
                return PuppetDirection.South;
            } else if (deltaX < 0 && deltaY == 0) {
                return PuppetDirection.West;
            } else {
                return PuppetDirection.None;
            }
        }
    }
}