using System;
using System.Diagnostics;
using GameInput;
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
        public bool isMouseOver;

        public void Init(TrappedPerson person)
        {
            this.person = person;

            FreshPosition();
            person.OnTrappedPersonTagChanged += OnTagChanged;
            person.OnPersonStatusChanged += OnStatusChanged;
            LevelManager.Instance.CameraController.OnCameraMoved += OnCameraMoved;
        }

        private void Update()
        {
            if (isMouseOver)
            {
                if (Input.GetKeyDown(KeyCode.H)) LevelManager.Instance.MakeTag(person, TriageTags.TriageTags.Red);
                if (Input.GetKeyDown(KeyCode.J)) LevelManager.Instance.MakeTag(person, TriageTags.TriageTags.Yellow);
                if (Input.GetKeyDown(KeyCode.K)) LevelManager.Instance.MakeTag(person, TriageTags.TriageTags.Green);
                if (Input.GetKeyDown(KeyCode.L)) LevelManager.Instance.MakeTag(person, TriageTags.TriageTags.Black);
            }
        }

        void OnTagChanged(TriageTag preTag, TriageTag newTag)
        {
            image.color = newTag.color;
        }

        void OnStatusChanged(PersonStatus preStatus, PersonStatus newStatus)
        {
            if (newStatus == PersonStatus.Saved)
            {
                gameObject.SetActive(false);
            }

            if (newStatus == PersonStatus.Died)
            {
                image.color = Color.black;
            }
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
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
        }
    }
}