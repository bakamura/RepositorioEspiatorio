using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] private bool GameOverScreen;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            if (GameOverScreen) {
                MenuScript.instance.GameOverScreen();
                return;
            }
            MenuScript.instance.VitoryScreen();
        }
    }
}
