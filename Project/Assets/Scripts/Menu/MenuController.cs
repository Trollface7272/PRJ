using System.Collections;
using System.Collections.Generic;
using SavesSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu {
    public class MenuController : MonoBehaviour {
        public GameObject loadingScreen;
        public Slider slider;
        public Text progressText;
        public GameObject mainMenu;
        public GameObject loadSaveMenu;

        private bool _loadGame;

        public void OpenSavesMenu(bool loadGame) {
            _loadGame = loadGame;
            loadSaveMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void SaveSlotClicked(int slotIndex) {
            var exists = SaveController.UpdatePath(slotIndex);
            if (!exists && _loadGame) return;
            if (_loadGame) LoadGame(1);
            else NewGame(1);
        }

        public void BackToMainMenu() {
            loadSaveMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        private void NewGame(int sceneIndex) {
            SaveController.LoadOnStart = false;
            loadSaveMenu.SetActive(false);
            StartCoroutine(LoadScene(sceneIndex));
        }

        private void LoadGame(int sceneIndex) {
            SaveController.LoadOnStart = true;
            loadSaveMenu.SetActive(false);
            StartCoroutine(LoadScene(sceneIndex));
        }
        
        private IEnumerator LoadScene(int sceneIndex) {
            Time.timeScale = 1f;
            var scene = SceneManager.LoadSceneAsync(sceneIndex);
            loadingScreen.SetActive(true);
            while (!scene.isDone) {
                var progress = Mathf.Clamp01(scene.progress / .9f);
                slider.value = progress;
                progressText.text = (int)progress * 100 + "%";
                yield return null;
            }
        }

        public void ExitGame() {
            Application.Quit();
        }
    }
}