using DG.Tweening;
using TriageTags;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Minimap
{
    public class MapTrappedPerson : InMapObject
    {
        public Image colorDot;
        public FadeTwnUIBehaviour crossIcon;
        
        private TrappedPerson trappedPerson;
        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup => this.LazyGetComponent(canvasGroup);
        private FadeTwnUIBehaviour fadeTwn;
        public FadeTwnUIBehaviour FadeTwn => this.LazyGetComponent(fadeTwn);

        public void Init(TrappedPerson person)
        {
            trappedPerson = person;
            Init(MinimapUtils.WorldPositionToMapPosition(MinimapManager.Instance.RangeBox, person.transform.position));
            
            person.onTagChanged.AddListener(OnTagChanged);
            person.onStatusChanged.AddListener(OnStatusChanged);

            FadeTwn.ForceAppear();
            
            OnStatusChanged(person, person.Status, person.Status);
            OnTagChanged(person, person.TriageTag, person.TriageTag);
        }

        void OnGetRescue()
        {
            Tweener tweener = CanvasGroup.DOFade(0, 0.5f);
            Tweener tweener1 = transform.DOScale(1.25f, 0.6f);
            tweener1.SetEase(Ease.OutQuart);
        }
        void OnTagChanged(TrappedPerson person, TriageTag preTag, TriageTag newTag)
        {
            if (newTag == TriageTags.TriageTags.None)
            {
                return;
            }
            colorDot.color = newTag.color;
        }

        void OnStatusChanged(TrappedPerson person, PersonStatus preStatus, PersonStatus newStatus)
        {
            if (newStatus == PersonStatus.Waiting) { return; }
            if (newStatus == PersonStatus.Saved)
            {
                OnGetRescue();
            }
            if (newStatus == PersonStatus.Died)
            {
                colorDot.color = Color.black;
            }

            CanvasGroup.alpha = 0.5f;
            crossIcon.ForceAppear();
        }
    }
}