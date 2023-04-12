using System.Collections.Generic;
using GameManager;
using Level;
using TMPro;
using UI.Module;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Settlement
{
    public class SettlementUIManager : FadeTwnUIBehaviour
    {
        public LevelManager LevelManager => LevelManager.Instance;
        public TMP_Text dayText;
        public PersonCounter PersonCounter => LevelManager.PersonCounter;
        public GameObject tipPrefab;
        public RectTransform tipsParent;
        public CounterWithIcon savedCounter;
        public CounterWithIcon foundCounter;
        public RectTransform keyToContinue;

        [Header("Last Day")]
        public RectTransform lastDayPanel;
        public CounterWithIcon allFoundCounter;
        public CounterWithIcon allSavedCounter;
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
            SetDay(LevelManager.Instance.levelInstance.index + 1);
            SetSavedCount(PersonCounter.SavedPersonsCount);
            SetFoundCount(LevelManager.foundPersonCount);
            foreach (var tip in tips)
            {
                if (tip.GetSettlementTip(this, out string content))
                {
                    AddGenerateTip(content);
                }
            }

            
            if (MiniJam128.GameManager.Instance.IsLastLevel)
            {
                keyToContinue.gameObject.SetActive(false);
                lastDayPanel.gameObject.SetActive(true);
                int allSCount = 0;
                int allFCount = 0;
                foreach (var info in MiniJam128.GameManager.Instance.levelSettlementInfos)
                {
                    allFCount += info.foundPersonCount;
                    allSCount += info.savedPersonCount;
                }

                allFoundCounter.Counter = allFCount;
                allSavedCounter.Counter = allSCount;
            }
            else
            {
                lastDayPanel.gameObject.SetActive(false);
            }
        }

        public void SetDay(int day)
        {
            dayText.text = "DAY " + day;
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