using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleporterMenu : MonoBehaviour
{
    [SerializeField] private InputField[] cordinates;

    public void Teleport() {
        if (PlayerData.teleportPowerUps > 0) {
            Vector3 point = new Vector3(cordinates[0].text != null ? float.Parse(cordinates[0].text) : TrdControl.playerTransform.position.x, 
                cordinates[1].text != null ? float.Parse(cordinates[1].text) : TrdControl.playerTransform.position.y, 
                cordinates[2].text != null ? float.Parse(cordinates[2].text) : TrdControl.playerTransform.position.z);
            TrdControl.playerTransform.position = point;
            PlayerData.UpdateTPPowerUps(-1);
        }
        else Debug.Log("Not Enough TP Points");
    }
}
