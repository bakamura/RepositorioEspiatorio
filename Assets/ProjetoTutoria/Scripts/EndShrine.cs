using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndShrine : MonoBehaviour
{
    [SerializeField] private string SceneToOpen;
    [SerializeField] private string ShrineCompleted;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!PlayerData.ShrinesDone.ContainsKey(ShrineCompleted)) PlayerData.ShrinesDone.Add(ShrineCompleted, true);
            MenuScript.instance.UpdateCheckList(ShrineCompleted);
            PlayerData._isReturningToMainScene = true;
            SceneManager.LoadScene(SceneToOpen);
        }
    }
}
