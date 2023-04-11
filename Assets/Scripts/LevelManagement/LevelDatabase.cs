using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniJam128.LevelManagement
{
    [CreateAssetMenu(fileName = "Levels Database", menuName = "MiniJam128/LevelDatabase", order = 0)]
    public class LevelDatabase : ScriptableObject
    {
        public List<LevelInstance> levels;
    }

    [Serializable]
    public class LevelInstance
    {
        public string name;
        public SceneAsset scene;

        [HideInInspector] public int index;
    }
}