using System;
using TMPro;
using UI.Module;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Viewport.PersonInfoBar
{
    public class RescueBar : FadeTwnUIBehaviour
    {
        public ProgressBar progressBar;
        public Image icon;
        public TMP_Text text;
        private TriageTags.TriageTag tag;

        public TriageTags.TriageTag Tag
        {
            get => tag;
            set
            {
                tag = value;
                SetColor(tag.uiLightColor, tag.uiDarkColor);
            }
        }

        [HideInInspector] private TrappedPerson person;
        public void Init(TrappedPerson trappedPerson)
        {
            this.person = trappedPerson;
            this.Tag = trappedPerson.TriageTag;

            progressBar.Init();
        }

        public void OnRescueProgressChanged(float value)
        {
            Tag = person.TriageTag;
            progressBar.Rate = value;
        }
        private void SetColor(Color lightColor, Color darkColor)
        {
            icon.color = lightColor;
            text.color = lightColor;
            if (progressBar.container.TryGetComponent(out Image image)) image.color = darkColor;
            if (progressBar.progress.TryGetComponent(out Image progressImage)) progressImage.color = lightColor;
        }
    }
}