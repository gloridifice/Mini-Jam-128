using TMPro;
using UnityEngine;

namespace UI.Module
{
    public class CounterWithIcon : UIBehaviour
    {
        public TMP_Text text;
        
        private int count;

        public int Counter
        {
            get => count;
            set
            {
                count = value;
                text.text = value.ToString();
            }
        }

        public void Init(int value = 0)
        {
            Counter = value;
        }
    }
}