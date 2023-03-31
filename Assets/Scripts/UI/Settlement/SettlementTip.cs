using TMPro;
using UnityEngine;

namespace UI.Settlement
{
    public class SettlementTip : UIBehaviour
    {
        public TMP_Text text;
        public void Init(string value)
        {
            text.text = value;
        }
    }
}