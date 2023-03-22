using System;
using System.Collections.Generic;
using UI.Module;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Viewport.PersonInfoBar
{
    public class ScanningBar : FadeTwnUIBehaviour
    {
        public LockableIcon sound;
        public LockableIcon health;
        public LockableIcon life;

        public ProgressBar progressBar;

        [HideInInspector] public TrappedPerson person;

        public void Init(TrappedPerson trappedPerson)
        {
            this.person = trappedPerson;
            var progressBarPos = progressBar.Rect.anchoredPosition;
            var containerLength = progressBar.ContainerLength;
            sound.Rect.anchoredPosition =
                progressBarPos + containerLength * person.soundScanningInfo.rate * Vector2.right;
            health.Rect.anchoredPosition =
                progressBarPos + containerLength * person.healthScanningInfo.rate * Vector2.right;
            life.Rect.anchoredPosition =
                progressBarPos + containerLength * person.lifeScanningInfo.rate * Vector2.right;

            progressBar.Init();
        }

        public void OnScanningProgressChanged(float value)
        {
            SetProgress(value);
        }

        public void SetProgress(float value)
        {
            progressBar.Rate = value;
        }

        public void OnSoundUnlock()
        {
            sound.Unlock();
        }

        public void OnHealthUnlock()
        {
            health.Unlock();
        }

        public void OnLifeUnlock()
        {
            life.Unlock();
        }
    }
}