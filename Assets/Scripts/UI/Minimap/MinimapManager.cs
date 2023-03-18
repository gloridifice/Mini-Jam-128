using System;
using System.Collections.Generic;
using GameManager;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Minimap
{
    public class MinimapManager : UIBehaviour
    {
        public static MinimapManager Instance;
        #region EditorInspector

        [SerializeField] private GameObject trappedPersonPrefab;
        [SerializeField] private GameObject playerPrefab;

        public RectTransform mapObjectsParent;

        #endregion

        private float height, width;
        public float Height => Rect.sizeDelta.y;
        public float Width => Rect.sizeDelta.x;
        public WorldRangeBox RangeBox => LevelManager.Instance.rangeBox;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(List<TrappedPerson> trappedPersons)
        {
            //添加伤员
            foreach (var person in trappedPersons)
            {
                GameObject uiObj = GameObject.Instantiate(trappedPersonPrefab, mapObjectsParent);
                if (uiObj.TryGetComponent(out MapTrappedPerson mapTrappedPerson))
                {
                    mapTrappedPerson.Init(person);
                }
            }

            //添加玩家
            GameObject playerUIObj = GameObject.Instantiate(playerPrefab, mapObjectsParent);
            if (playerUIObj.TryGetComponent(out MapPlayer mapObject))
            {
                mapObject.Init(LevelManager.Instance.CameraController);
            }
        }
    }
}