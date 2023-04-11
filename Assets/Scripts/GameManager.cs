using System;
using GameManager;
using MiniJam128.LevelManagement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniJam128
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public LevelDatabase levelDatabase;

        private LevelInstance currentLevel;

        public int MaxLevelIndex => levelDatabase.levels.Count - 1;
        public bool IsLastLevel => currentLevel.index == MaxLevelIndex;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (currentLevel != null)
            {
                LevelManager.Instance.levelInstance = currentLevel;
            }
        }
        public void LoadLevel(int index)
        {
            currentLevel = levelDatabase.levels[index];
            currentLevel.index = index;
            SceneManager.LoadScene(currentLevel.scene);
        }

        public void LoadMainMenu()
        {
            currentLevel = null;
            SceneManager.LoadScene("MainMenu");
        }
        
    }
}