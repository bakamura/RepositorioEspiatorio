using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndShrine : MonoBehaviour
{
    [SerializeField] private string SceneToOpen;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!PlayerData.ShrinesDone.ContainsKey(this.gameObject.name)) PlayerData.ShrinesDone.Add(this.gameObject.name, true);
            SceneManager.LoadScene(SceneToOpen);
        }
    }
}
