using System;
using SavesSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hud {
    public class EscMenu : MonoBehaviour {
        private static EscMenu _instance;
        public static EscMenu Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<EscMenu>();
                return _instance;
            }
        }
        public bool isToggled;

        public GameObject escMenu;

        private void Start() {
            escMenu.SetActive(false);
            isToggled = false;
        }

        public void Toggle() {
            isToggled = !isToggled;
            escMenu.SetActive(isToggled);
            Time.timeScale = isToggled ? 0 : 1;
        }

        public void Continue() {
            Toggle();
        }

        public void ToMenu() {
            SaveController.SaveGame();
            SceneManager.LoadScene(0);
        }

        public void Exit() {
            SaveController.SaveGame();
            Application.Quit();
        }

    }
}
