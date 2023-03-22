using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenAnim
{
    public class ScaleFadeLoop : UIBehaviour
    {
        private Sequence sequence;
        private Image image;
        public Image Image => this.LazyGetComponent(image);
        [Range(0f, 1f)] public float alpha = 0.5f;
        [Range(0f, 2f)] public float scale = 1.2f;

        private void Awake()
        {
            Color color = Image.color;
            color.a = 0;
            Image.color = color;

            sequence = DOTween.Sequence();
            sequence.SetAutoKill(false);
            sequence.Append(Image.DOFade(alpha, 1f));
            sequence.Append(Image.DOFade(0f, 1f));
            sequence.Insert(0f, Rect.DOScale(scale * Vector3.one, 2f));
            sequence.SetLoops(-1);

            sequence.Play();
        }
    }
}