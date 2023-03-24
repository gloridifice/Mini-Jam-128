using TMPro;
using UnityEngine;

namespace UI.Viewport
{
    public class SoundTip : FadeTwnUIBehaviour
    {
        public TMP_Text text;

        public void Init(string content, float time)
        {
            text.text = content;
            this.ForceAppear();
            Invoke("WaitForDie", time);
        }

        void WaitForDie()
        {
            ForceDisappearToDestroy();
        }
    }
}