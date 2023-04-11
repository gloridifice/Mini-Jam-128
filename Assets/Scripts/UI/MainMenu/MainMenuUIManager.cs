using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MainMenuUIManager : UIBehaviour
    {
        public FadeTwnUIBehaviour stuffPanel;
        private bool isStuffOpen;

        public void OnStartButtonClicked()
        {
            MiniJam128.GameManager.Instance.LoadLevel(0);
        }

        public void OnStuffButtonClicked()
        {
            if (isStuffOpen)
            {
                stuffPanel.ForceDisappear();
                isStuffOpen = false;
            }
            else
            {
                stuffPanel.ForceAppear();
                isStuffOpen = true;
            }
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}