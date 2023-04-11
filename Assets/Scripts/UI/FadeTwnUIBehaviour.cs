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

        [HideInInspector] public bool shouldAppear = false;
        public bool IsAppeared => gameObject.activeSelf && (AppearTwn.IsPlaying() || AppearTwn.IsComplete());
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public bool waitForDie = false;

        public virtual void UpdateAppearCondition(bool b)
        {
            if (waitForDie) return;
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
            if (waitForDie) return;
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
            waitForDie = true;
            ForceDisappear();
            AppearTwn.onComplete += () => { Destroy(this); };
        }
    }
}