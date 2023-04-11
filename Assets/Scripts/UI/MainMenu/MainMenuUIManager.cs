using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MainMenuUIManager : UIBehaviour
    {
        public void OnStartButtonClicked()
        {
            MiniJam128.GameManager.Instance.LoadLevel(0);
        }

        public void OnStuffButtonClicked()
        {
            //todo
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}