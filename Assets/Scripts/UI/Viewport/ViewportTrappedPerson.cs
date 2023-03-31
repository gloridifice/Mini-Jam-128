using System;
using System.Diagnostics;
using DG.Tweening;
using GameInput;
using GameManager;
using TriageTags;
using UI.Viewport.PersonInfoBar;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Viewport
{
    public class ViewportTrappedPerson : ViewportElementBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Editor Inspector

        public Image image;
        public FadeTwnUIBehaviour crossIcon;
        public GameObject tagSelectorPrefab;
        public GameObject soundTipPrefab;

        [Header("Animation")] [Range(0, 2)] public float duration;
        [Range(0, 2)] public float scale;
        public Ease animEase;

        #endregion

        [HideInInspector] public TrappedPerson person;
        [HideInInspector] public bool isMouseOver;
        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup => this.LazyGetComponent(canvasGroup);

        private Tweener mouseHoverTwn;

        public Tweener MouserHoverTwn
        {
            get
            {
                if (mouseHoverTwn == null)
                {
                    mouseHoverTwn = Rect.DOScale(Vector3.one * scale, duration);
                    mouseHoverTwn.SetEase(animEase);
                    mouseHoverTwn.SetAutoKill(false);
                }

                return mouseHoverTwn;
            }
        }

        public bool CanInteractWith =>
            person.Status != PersonStatus.Died && person.TriageTag != TriageTags.TriageTags.Black;

        [HideInInspector] public UnityEvent<Vector2> onElementMoved;

        public void Init(TrappedPerson person)
        {
            this.person = person;
            base.Init(CalculatePos());
            this.person.viewportPerson = this;

            FreshPosition();
            LevelManager.Instance.CameraController.OnCameraMoved += OnCameraMoved;

            person.onTagChanged.AddListener(OnTagChanged);
            person.onStatusChanged.AddListener(OnStatusChanged);
        }

        protected override void Update()
        {
            base.Update();
            if (isMouseOver)
            {
                if (Input.GetKeyDown(KeyCode.H)) LevelManager.Instance.MarkTag(person, TriageTags.TriageTags.Red);
                if (Input.GetKeyDown(KeyCode.J)) LevelManager.Instance.MarkTag(person, TriageTags.TriageTags.Yellow);
                if (Input.GetKeyDown(KeyCode.K)) LevelManager.Instance.MarkTag(person, TriageTags.TriageTags.Green);
                if (Input.GetKeyDown(KeyCode.L)) LevelManager.Instance.MarkTag(person, TriageTags.TriageTags.Black);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameObject selectorObj = GameObject.Instantiate(tagSelectorPrefab, Rect.parent);
                    if (selectorObj.TryGetComponent(out TagSelector.TagSelector selector))
                    {
                        selector.Init(person, Rect.anchoredPosition);
                    }
                }

                person.isGettingInfo = Input.GetKey(KeyCode.Mouse1);
            }
        }

        private float soundTipGenerateCounter;
        public float soundInterval = 3;
        public float soundTipGenRange = 20;

        private void FixedUpdate()
        {
            if (!CanInteractWith) return;
            soundTipGenerateCounter -= Time.fixedDeltaTime;
            if (soundTipGenerateCounter <= 0)
            {
                soundTipGenerateCounter = soundInterval + Random.Range(-1f, 3f);
                if (person.soundScanningInfo.isUnlock)
                {
                    GameObject obj = GameObject.Instantiate(soundTipPrefab, transform);
                    if (obj.TryGetComponent(out SoundTip soundTip))
                    {
                        float x = 0f;
                        float y = 0f;
                        x += Random.Range(-soundTipGenRange, 0);
                        y += Random.Range(-soundTipGenRange, soundTipGenRange);
                        soundTip.Rect.anchoredPosition = new Vector2(x, y);
                        float time = 1.5f + Random.Range(0f, 1f);
                        soundTip.Init(person.GetSubtitle(), time);
                    }
                }
            }
        }

        void OnTagChanged(TrappedPerson person, TriageTag preTag, TriageTag newTag)
        {
            image.color = newTag.color;
        }

        void OnStatusChanged(TrappedPerson person, PersonStatus preStatus, PersonStatus newStatus)
        {
            if (newStatus == PersonStatus.Saved)
            {
                OnSaved();
            }

            if (newStatus == PersonStatus.Died)
            {
                image.color = Color.black;
                crossIcon.ForceAppear();
                CanvasGroup.DOFade(0.75f, 0.6f);
            }
        }

        void OnSaved()
        {
            Tweener twn0 = CanvasGroup.DOFade(0, 0.6f);
            Tweener twn1 = transform.DOScale(1.3f, 0.6f);

            twn1.SetEase(Ease.InQuart);
        }

        void OnCameraMoved(CameraController controller)
        {
            FreshPosition();
        }

        void FreshPosition()
        {
            targetPos = CalculatePos();
            onElementMoved.Invoke(targetPos);
        }

        Vector2 CalculatePos()
        {
            RectTransform parent = Rect.parent as RectTransform;
            Vector3 scPos = Camera.main.WorldToScreenPoint(person.transform.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, scPos, Camera.main,
                    out Vector2 localPoint))
            {
                return localPoint;
            }

            return Vector2.zero;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanInteractWith) return;
            isMouseOver = true;
            MouserHoverTwn.PlayForward();
            ShowInfo();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isMouseOver) return;
            isMouseOver = false;
            MouserHoverTwn.PlayBackwards();
            person.isGettingInfo = false;
            CloseInfo();
        }

        public GameObject infoBarPrefab;
        private PersonalInfoBar infoBar;

        private void ShowInfo()
        {
            if (infoBar == null)
            {
                GameObject obj = GameObject.Instantiate(infoBarPrefab, transform.parent);
                if (obj.TryGetComponent(out PersonalInfoBar bar))
                {
                    infoBar = bar;
                    infoBar.Init(person);
                    bar.Rect.anchoredPosition = Rect.anchoredPosition + 100 * Vector2.right;
                }
            }
            else
            {
                infoBar.Display();
            }
        }

        private void CloseInfo()
        {
            if (infoBar != null)
            {
                infoBar.Hide();
            }
        }
    }
}