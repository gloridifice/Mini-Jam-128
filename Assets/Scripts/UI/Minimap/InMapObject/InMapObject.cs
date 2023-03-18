using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Minimap
{
    public class InMapObject : UIBehaviour
    {
        
        private Vector2 mapPosition;

        public Vector2 MapPosition
        {
            get => mapPosition;
            set
            {
                mapPosition = value;
                Rect.anchoredPosition = MinimapUtils.MapPositionToUIPosition(value);
            }
        }

        public virtual void Init(Vector2 mapPos)
        {
            MapPosition = mapPos;
        }
    }
}