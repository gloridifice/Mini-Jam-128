using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Module
{
    public class LockableIcon : UIBehaviour
    {
        public Image image;
        public Sprite lockSprite;
        public Sprite unlockSprite;

        public bool isLockDefault = true;

        private bool isLocked;
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                isLocked = value;
                image.sprite = value ? lockSprite : unlockSprite;
            }
        }

        public void Unlock()
        {
            IsLocked = false;
        }

        public void Lock()
        {
            IsLocked = true;
        }
        private void Awake()
        {
            image.sprite = isLockDefault ? lockSprite : unlockSprite;
        }
    }
}