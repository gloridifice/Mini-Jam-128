using GameManager;
using UI.Minimap;
using UI.Module;
using UI.Viewport;
using UnityEngine;

namespace UI
{
    public class LevelUIManager : MonoBehaviour
    {
        public MinimapManager minimapManager;
        public ViewportUIManager viewportUIManager;
        public BatteryDisplay battery;
        public CounterWithIcon savedPersonCount;
        public CounterWithIcon foundPersonCount;

        public void Init()
        {
            minimapManager.Init();
            viewportUIManager.Init(LevelManager.Instance.TrappedPersons);
            battery.Init();
            savedPersonCount.Init();
            foundPersonCount.Init();
            LevelManager.Instance.onFoundPersonCountChanged.AddListener((arg0 => foundPersonCount.Counter = arg0));
            LevelManager.Instance.onSavedPersonCountChanged.AddListener((arg0 => savedPersonCount.Counter = arg0));
        }
    }
}