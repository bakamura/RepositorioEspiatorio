using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndShrine : MonoBehaviour
{
    [SerializeField] private string SceneToOpen;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!PlayerData.ShrinesDone.ContainsKey(SceneManager.GetActiveScene().name)) PlayerData.ShrinesDone.Add(SceneManager.GetActiveScene().name, true);
            MenuScript.instance.UpdateCheckList(SceneManager.GetActiveScene().name);
            PlayerData._isReturningToMainScene = true;
            SceneManager.LoadScene(SceneToOpen);
        }
    }
}
