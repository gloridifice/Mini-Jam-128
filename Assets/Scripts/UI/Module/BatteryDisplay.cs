using System;
using System.Collections.Generic;
using GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Module
{
    public class BatteryDisplay : UIBehaviour
    {
        public List<RectTransform> batteryCubes;
        public TMP_Text percentageText;

        public void Init()
        {
            LevelManager.Instance.onBatteryChanged.AddListener(SetBattery); 
        }

        /// <param name="value">range: 0f ~ 1f</param>
        public void SetBattery(float value)
        {
            percentageText.text = value + "%";

            int count = Mathf.CeilToInt(value * batteryCubes.Count);
            for (int i = 0; i < batteryCubes.Count; i++)
            {
                if (i < count)
                {
                    if (!batteryCubes[i].gameObject.activeSelf)
                    {
                        batteryCubes[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (batteryCubes[i].gameObject.activeSelf)
                    {
                        batteryCubes[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}