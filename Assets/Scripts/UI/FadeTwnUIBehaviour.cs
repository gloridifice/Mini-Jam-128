using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTwnUIBehaviour : UIBehaviour
    {
        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup => this.LazyGetComponent(canvasGroup);

        private Tweener appearTwn;

        public Tweener AppearTwn
        {
            get
            {
                if (appearTwn == null)
                {
                    CanvasGroup.alpha = 0;
                    appearTwn = CanvasGroup.DOFade(1f, 0.5f);
                    appearTwn.SetAutoKill(false);
                    appearTwn.Pause();
                    appearTwn.onRewind += () => { gameObject.SetActive(false); };
                }

                return appearTwn;
            }
        }

        public bool shouldAppear = false;
        public bool IsAppeared => gameObject.activeSelf && (AppearTwn.IsPlaying() || AppearTwn.IsComplete());
        public bool isActive = false;

        public virtual void UpdateAppearCondition(bool b)
        {
            shouldAppear = b;
            if (isActive && shouldAppear && !IsAppeared)
            {
                Appear();
            }
        }

        public virtual void Active()
        {
            isActive = true;
        }

        public virtual void ForceAppear()
        {
            shouldAppear = true;
            Active();
            Appear();
        }

        protected virtual void Appear()
        {
            gameObject.SetActive(true);
            AppearTwn.PlayForward();
        }

        public virtual void ForceDisappear()
        {
            isActive = false;
            shouldAppear = false;
            AppearTwn.PlayBackwards();
        }

        public virtual void ForceDisappearToDestroy()
        {
            ForceDisappear();
            AppearTwn.onComplete += () => { Destroy(this); };
        }
    }
}