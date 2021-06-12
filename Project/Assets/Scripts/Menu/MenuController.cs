using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu {
    public class MenuController : MonoBehaviour {
        public GameObject loadingScreen;
        public Slider slider;
        public Text progressText;
        public GameObject mainMenu;

        public void NewGame(int sceneIndex) {
            StartCoroutine(LoadScene(sceneIndex));
        }
        
        private IEnumerator LoadScene(int sceneIndex) {
            mainMenu.SetActive(false);
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