using System.Collections.Generic;
using GameManager;
using Level;
using UI.Module;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Settlement
{
    public class SettlementUIManager : FadeTwnUIBehaviour
    {
        public LevelManager LevelManager => LevelManager.Instance;
        public Counter Counter => LevelManager.Counter;
        public GameObject tipPrefab;
        public RectTransform tipsParent;
        public CounterWithIcon savedCounter;
        public CounterWithIcon foundCounter;
        private List<ISettlementTip> tips = new()
        {
            new DyingTip(),
            new FamilyTip(),
            new ReturnToWorkTip(),
            new PartTip("arm"),
            new PartTip("leg"),
            new PartTip("vision")
        };

        public void DisplayAndInit()
        {
            this.ForceAppear();
            Init();
        }
        public void Init()
        {
            SetSavedCount(Counter.SavedPersonsCount);
            SetFoundCount(LevelManager.foundPersonCount);
            foreach (var tip in tips)
            {
                if (tip.GetSettlementTip(this, out string content))
                {
                    AddGenerateTip(content);
                }
            }
        }

        public void AddGenerateTip(string tip)
        {
            GameObject obj = GameObject.Instantiate(tipPrefab, tipsParent);
            if (obj.TryGetComponent(out SettlementTip settlementTip))
            {
                settlementTip.Init(tip);
            }
        }

        public void SetSavedCount(int value)
        {
            savedCounter.Counter = value;
        }

        public void SetFoundCount(int value)
        {
            foundCounter.Counter = value;
        }
    }
}