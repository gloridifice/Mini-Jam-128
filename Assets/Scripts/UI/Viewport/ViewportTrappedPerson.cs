using System.Diagnostics;
using GameManager;
using TriageTags;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Viewport
{
    public class ViewportTrappedPerson : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image image;
        public TrappedPerson person;

        public void Init(TrappedPerson person)
        {
            this.person = person;

            FreshPosition();
            LevelManager.Instance.CameraController.OnCameraMoved += OnCameraMoved;
        }

        void OnTagChanged(TriageTag preTag, TriageTag newTag)
        {
            image.color = newTag.color;
        }

        void OnCameraMoved(CameraController controller)
        {
            FreshPosition();
        }

        void FreshPosition()
        {
            RectTransform parent = Rect.parent as RectTransform;
            Vector3 scPos = Camera.main.WorldToScreenPoint(person.transform.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, scPos, Camera.main,
                    out Vector2 localPoint))
            {
                Rect.anchoredPosition = localPoint;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}