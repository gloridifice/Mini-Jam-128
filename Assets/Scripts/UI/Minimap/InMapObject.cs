using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Minimap
{
    public class InMapObject : MonoBehaviour
    {
        
        private Vector2 mapPosition;

        public Vector2 MapPosition
        {
            get => mapPosition;
            private set => mapPosition = value;
        }

        public void Init()
        {
             
        }
        
        
    }
}