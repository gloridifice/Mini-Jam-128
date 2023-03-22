using System;
using DG.Tweening;
using GameManager;
using TriageTags;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UI.Viewport.TagSelector
{
    public class TagUI : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TriageTagType tagType;

        private TagSelector selector;
        public TagSelector Selector => this.LazyGetComponentInParent(selector);
        public bool isMouseOver;
        private Tweener mouseHoverTwn;
        public Tweener MouserHoverTwn
        {
            get
            {
                if (mouseHoverTwn == null)
                {
                    mouseHoverTwn = Rect.DOScale(Vector3.one * 1.2f, Selector.highlightDuration);
                    mouseHoverTwn.SetEase(Ease.OutQuad);
                    mouseHoverTwn.SetAutoKill(false);
                }

                return mouseHoverTwn;
            }
        }
        private void OnEnable()
        {
            Selector.OnMarkExecute += OnMarkExecute;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
            MouserHoverTwn.PlayForward();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
            MouserHoverTwn.PlayBackwards();
        }

        void OnMarkExecute()
        {
            if (this.isMouseOver)
            {
                Selector.Mark(tagType.GetInstance());
            }
        }
    }
}