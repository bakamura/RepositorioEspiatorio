using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static MenuScript instance;
    [SerializeField] private CanvasGroup[] Screens;
    [SerializeField] private string MainMenuScene;
    private CanvasGroup currentActiveScene;
    private string _sceneToOpen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (_sceneToOpen != MainMenuScene)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else if (instance != this) Destroy(this.gameObject);
    }
    //private void OnLevelWasLoaded(int level)
    //{
    //    if (SceneManager.GetActiveScene().name != MainMenuScene)
    //    {
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    public void OpenScene(string scene)
    {
        _sceneToOpen = scene;
        SceneManager.LoadScene(_sceneToOpen);
        UpdateScreen(null);
    }

    private void ContainsCurrentActiveScreen()
    {
        if (currentActiveScene == null)
        {
            foreach (CanvasGroup screen in Screens)
                if (screen.alpha != 0)
                {
                    screen.alpha = 0;
                    screen.blocksRaycasts = false;
                    screen.interactable = false;
                }
        }
        else
        {
            currentActiveScene.alpha = 0;
            currentActiveScene.interactable = false;
            currentActiveScene.blocksRaycasts = false;
            currentActiveScene = null;
        }
    }

    public void UpdateScreen(CanvasGroup newScreen)
    {
        if (newScreen == null)
        {
            ContainsCurrentActiveScreen();
        }
        else
        {
            ContainsCurrentActiveScreen();
            newScreen.alpha = 0;
            newScreen.blocksRaycasts = false;
            newScreen.interactable = false;
            currentActiveScene = newScreen;
        }
    }
}
