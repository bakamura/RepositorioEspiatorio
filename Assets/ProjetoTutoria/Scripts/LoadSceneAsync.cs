using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour
{
    private AsyncOperation _asyncOperation = null;
    [SerializeField] private string SceneToLoad;
    
    // Update is called once per frame
    void Update()
    {
        if (_asyncOperation != null)
        {
            LoadingSimbol.instance.FillImage(_asyncOperation.progress+0.1f);
            if (_asyncOperation.isDone)
            {
                LoadingSimbol.instance.FillImage(0);
                _asyncOperation = null;
            }
        }
    }

    public void LoadScene()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
    }
}
