using System;
using UnityEngine;

namespace UI.Module
{
    public class ProgressBar : UIBehaviour
    {
        public RectTransform container;
        public RectTransform progress;
        public float deltaSpeed = 0.02f;

        public float ProgressLength => progress.sizeDelta.x;
        public float ContainerLength => container.sizeDelta.x;

        private float targetLength;

        public float Rate
        {
            get => targetLength / ContainerLength;
            set => targetLength = ContainerLength * value;
        }

        private void FixedUpdate()
        {
            Vector2 vector2 = progress.sizeDelta;
            vector2.x = Mathf.Lerp(vector2.x, targetLength, deltaSpeed);
            progress.sizeDelta = vector2;
        }

        public void Init(float initRate = 0)
        {
            Rate = initRate;
            Vector2 v2 = progress.sizeDelta;
            v2.x = initRate * ContainerLength;
            progress.sizeDelta = v2;
        }
    }
}