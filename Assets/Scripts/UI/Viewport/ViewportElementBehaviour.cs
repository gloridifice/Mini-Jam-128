using System;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace UI.Viewport
{
    public class ViewportElementBehaviour : UIBehaviour
    {
        [HideInInspector] public Vector2 targetPos;
        [Range(0,1)]public float deltaSpeed = 0.7f;
        
        public virtual void Init(Vector2 pos)
        {
            Rect.anchoredPosition = pos;
            targetPos = pos;
        }

        protected virtual void Update()
        {
            this.Rect.anchoredPosition = Vector2.Lerp(Rect.anchoredPosition, targetPos, Time.deltaTime * 50f * deltaSpeed);
        }
    }
}