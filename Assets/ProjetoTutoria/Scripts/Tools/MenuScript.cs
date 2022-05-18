using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public static MenuScript instance;
    [SerializeField] private CanvasGroup[] Screens;
    [SerializeField] private string MainMenuScene;
    private CanvasGroup currentActiveScene;
    private string _sceneToOpen;
    [SerializeField] private GameObject[] _shrineCheckBox;

    private void Awake() {
        if (instance == null) {
            instance = this;
            if (_sceneToOpen != MainMenuScene) {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else if (instance != this) Destroy(this.gameObject);
    }

    private void Update() {
        if (Input.GetButtonDown("ShrineList")) UpdateScreen(Screens[3]);
        if (Input.GetKeyDown(KeyCode.T)) UpdateScreen(Screens[4]);
    }

    public void VitoryScreen() {
        UpdateScreen(Screens[1]);
    }

    public void GameOverScreen() {
        UpdateScreen(Screens[2]);
    }

    public void OpenScene(string scene) {
        _sceneToOpen = scene;
        if (_sceneToOpen != MainMenuScene) UpdateScreen(null);
        SceneManager.LoadScene(_sceneToOpen);
    }

    private void ContainsCurrentActiveScreen() {
        if (currentActiveScene == null) {
            foreach (CanvasGroup screen in Screens)
                if (screen.alpha != 0) {
                    screen.alpha = 0;
                    screen.blocksRaycasts = false;
                    screen.interactable = false;
                }
        }
        else {
            currentActiveScene.alpha = 0;
            currentActiveScene.interactable = false;
            currentActiveScene.blocksRaycasts = false;
            currentActiveScene = null;
        }
    }

    public void UpdateScreen(CanvasGroup newScreen) {
        if (newScreen == null || newScreen == currentActiveScene) { // deactivates HUD
            ContainsCurrentActiveScreen();
        }
        else { // deactivates HUD and activates other HUD
            ContainsCurrentActiveScreen();
            newScreen.alpha = 1;
            newScreen.blocksRaycasts = true;
            newScreen.interactable = true;
            currentActiveScene = newScreen;
        }
    }

    public void UpdateCheckList(string shrineName) {
        if (PlayerData.ShrinesDone.ContainsKey(shrineName)) {
            foreach (GameObject UiCheckBox in _shrineCheckBox) {
                if (shrineName == UiCheckBox.name) {
                    UiCheckBox.GetComponentInChildren<Image>().color = Color.green;
                    break;
                }
            }
        }
    }
}
