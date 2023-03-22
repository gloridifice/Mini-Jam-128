using System;
using GameManager;
using TriageTags;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.Viewport.TagSelector
{
    public class TagSelector : UIBehaviour
    {
        
        private TrappedPerson person;
        public event Action OnMarkExecute = () => { };

        public float range;
        public float highlightDuration;
        private bool lastMouseIn;
        private bool isMouseIn;

        public void Init(TrappedPerson trappedPerson, Vector2 position)
        {
            person = trappedPerson;
            Rect.anchoredPosition = position;
        }

        public void Mark(TriageTag triageTag)
        {
            LevelManager.Instance.MakeTag(person, triageTag);
        }

        private void Update()
        {
            lastMouseIn = isMouseIn;
            RectTransform parent = Rect.parent as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, Camera.main, out Vector2 localPoint))
            {
                isMouseIn = (localPoint - Rect.anchoredPosition).magnitude < range;
            }

            if (lastMouseIn && !isMouseIn)
            {
                OnPointerExit();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnMarkExecute.Invoke();
                Die();
            }
        }

        public void OnPointerExit()
        {
            print("mouse leave");
            Die();
        }
        
        public void Die()
        {
            DestroyImmediate(this.gameObject);
        }
    }
}