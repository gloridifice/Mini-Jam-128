using UnityEngine;

namespace UI.Minimap
{
    public class MapPlayer : InMapObject
    {
        private CameraController cameraController;
        public RectTransform rangeRect;
        public void Init(CameraController controller)
        {
            cameraController = controller;
            Init(controller.GetMinimapPos());

            controller.OnCameraMoved += OnCameraMoved;
            Vector2 size = MinimapUtils.MapPositionToUIPosition(controller.GetMinimapSize());
            rangeRect.sizeDelta = size;

        }

        private void OnCameraMoved(CameraController controller)
        {
            MapPosition = controller.GetMinimapPos();
        }
    }
}