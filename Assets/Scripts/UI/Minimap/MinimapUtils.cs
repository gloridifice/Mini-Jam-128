using UnityEngine;

namespace UI.Minimap
{
    public class MinimapUtils
    {
        public static Vector2 GetPlayerPosInMinimap(Camera camera)
        {
            //todo
            return new Vector2(0.5f, 0.5f);
        }

        public static Vector2 MapPositionToUIPosition(Vector2 mapPos)
        {
            MinimapManager manager = MinimapManager.Instance;
            float height = mapPos.y * manager.Height;
            float width = mapPos.x * manager.Width;
            return new Vector2(width, height);
        }
    }
}