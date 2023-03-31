using System;
using DG.Tweening;
using TriageTags;
using UI.Module;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Viewport.PersonInfoBar
{
    [RequireComponent(typeof(FloatingUI))]
    public class PersonalInfoBar : FadeTwnUIBehaviour
    {
        public ScanningBar scanningBar;
        public RescueBar rescueBar;

        public HeartBeatPanel heartBeatPanel;
        public LifeTimePanel lifeTimePanel;

        public FloatEvent onScanningProgressChanged;
        public FloatEvent onRescueProgressChanged;
        public FloatEvent onHeartBeatChanged;
        public UnityEvent onSoundUnlock;
        public UnityEvent onHealthUnlock;
        public UnityEvent onLifeUnlock;

        private FloatingUI floatingUI;
        public FloatingUI FloatingUI => this.LazyGetComponent(floatingUI);

        [HideInInspector] public TrappedPerson person;
        private bool initialized;


        public void Init(TrappedPerson trappedPerson)
        {
            person = trappedPerson;
            person.onScanningProgressChanged.AddListener(onScanningProgressChanged.Invoke);
            person.onRescueProgressChanged.AddListener(onRescueProgressChanged.Invoke);
            person.onHeartBeatChanged.AddListener(onHeartBeatChanged.Invoke);
            person.onSoundUnlock.AddListener(onSoundUnlock.Invoke);
            person.onHealthUnlock.AddListener(onHealthUnlock.Invoke);
            person.onLifeUnlock.AddListener(onLifeUnlock.Invoke);
            person.onStatusChanged.AddListener(OnPersonStatusChanged);
            person.viewportPerson.onElementMoved.AddListener(OnMove);
            person.onTagChanged.AddListener(OnPersonTagChanged);
            OnMove(person.viewportPerson.Rect.anchoredPosition);
            
            scanningBar.Init(person);
            rescueBar.Init(person);
            heartBeatPanel.Init(person);
            lifeTimePanel.Init(person);
            Display();
            initialized = true;
            
            foreach (Transform trans in Rect)
            {
                trans.gameObject.SetActive(false);
            }
        }

        public void OnMove(Vector2 pos)
        {
            FloatingUI.actualPos = pos + 40f * Vector2.right;
        }

        void OnPersonTagChanged(TrappedPerson trappedPerson, TriageTag preTag, TriageTag newTag)
        {
            if (newTag == TriageTags.TriageTags.Black)
            {
                Die();
            }
        }

        public void Hide()
        {
            scanningBar.ForceDisappear();
            heartBeatPanel.ForceDisappear();
            lifeTimePanel.ForceDisappear();
        }
        public void Display()
        {
            this.ForceAppear();
            scanningBar.Active();
            rescueBar.Active();
            heartBeatPanel.Active();
            lifeTimePanel.Active();
        }

        void OnPersonStatusChanged(TrappedPerson person, PersonStatus preStatus, PersonStatus newStatus)
        {
            if (newStatus == PersonStatus.Saved || newStatus == PersonStatus.Died)
            {
                Die();
            }
        }
        public void Die()
        {
            this.ForceDisappearToDestroy();
        }
        private void Update()
        {
            if (!initialized) return;
            scanningBar.UpdateAppearCondition(person.ShouldShowScanning);
            rescueBar.UpdateAppearCondition(person.ShouldShowRescueBar);
            heartBeatPanel.UpdateAppearCondition(person.healthScanningInfo.isUnlock);
            lifeTimePanel.UpdateAppearCondition(person.lifeScanningInfo.isUnlock);
        }
    }
}