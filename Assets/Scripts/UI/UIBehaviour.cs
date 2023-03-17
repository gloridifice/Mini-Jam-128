using UnityEngine;

namespace UI
{
    public class UIBehaviour : MonoBehaviour
    {
        private RectTransform rect;
        public RectTransform Rect
        {
            get
            {
                if (rect == null)
                {
                    rect = GetComponent<RectTransform>();
                }
                return rect;
            }
        }
    }
}