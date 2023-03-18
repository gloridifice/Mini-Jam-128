using UnityEngine;

namespace UI.Minimap
{
    public static class MinimapUtils
    {
        public static Vector2 GetMinimapPos(this CameraController cameraController)
        {
            MinimapManager manager = MinimapManager.Instance;
            return WorldPositionToMapPosition(manager.RangeBox, cameraController.Center);
        }

        public static Vector2 GetMinimapSize(this CameraController controller)
        {
            MinimapManager manager = MinimapManager.Instance;
            Vector2 a = WorldPositionToMapPosition(manager.RangeBox, controller.topRight);
            Vector2 b = WorldPositionToMapPosition(manager.RangeBox, controller.bottomLeft);
            Debug.Log(a + ", " + b);
            return a - b;
        }

        public static Vector2 MapPositionToUIPosition(Vector2 mapPos)
        {
            MinimapManager manager = MinimapManager.Instance;
            float height = mapPos.y * manager.Height;
            float width = mapPos.x * manager.Width;
            return new Vector2(width, height);
        }

        public static Vector2 WorldPositionToMapPosition(WorldRangeBox rangeBox, Vector3 worldPos)
        {
            return (worldPos.XZ() - rangeBox.Zero) / new Vector2(rangeBox.XSize, rangeBox.ZSize);
        }

        public static Vector2 WorldPositionToMapPosition(WorldRangeBox rangeBox, Vector2 worldPos)
        {
            return (worldPos - rangeBox.Zero) / new Vector2(rangeBox.XSize, rangeBox.ZSize);
        }
    }
}