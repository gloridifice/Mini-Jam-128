using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Module.Button
{
    public class RoundedButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Image frameImage;
        public TMP_Text buttonText;
        public Image iconImage;
        public UnityEvent onClick;

        [HideInInspector] public Color highlightColor = Color.white;

        private Sequence mouseHoverTwn;
        public Sequence MouseHoverTwn
        {
            get{
                if (mouseHoverTwn == null)
                {
                    mouseHoverTwn = DOTween.Sequence();
                    Tweener twn1 = Rect.DOAnchorPosX(Rect.anchoredPosition.x + 15, 0.3f);
                    twn1.SetEase(Ease.OutQuad);
                    Tweener twn2 = buttonText.DOColor(highlightColor, 0.4f);
                    mouseHoverTwn.Append(twn1);
                    mouseHoverTwn.Insert(0, twn2);
                    
                    mouseHoverTwn.SetAutoKill(false);
                    mouseHoverTwn.Pause();
                }

                return mouseHoverTwn;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
               MouseHoverTwn.PlayForward();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseHoverTwn.PlayBackwards();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }
    }
}