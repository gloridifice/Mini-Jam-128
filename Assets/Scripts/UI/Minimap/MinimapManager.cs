using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace UI.Minimap
{
    public class MinimapManager : UIBehaviour
    {
        public static MinimapManager Instance;
        private float height, width;
        public float Height => Rect.sizeDelta.y;
        public float Width => Rect.sizeDelta.x;

        private void Awake()
        {
            if (Instance != null)
            {
               DestroyImmediate(this); 
            }
            else
            {
                Instance = this;
            } 
        }

        public void InitMinimap(List<TrappedPerson> trappedPersons)
        {
            //添加伤员
            //添加玩家
        }
    }
}