using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static Vector3 playerPosition;
    public static Dictionary<string, bool> ShrinesDone = new Dictionary<string, bool>();
    public static int currentShrineID;
    public static bool _isReturningToMainScene;
    public static int teleportPowerUps;

    public static void SavePosition(Vector3 position)
    {
        playerPosition = position;
    }

    public static void UpdateTPPowerUps(int valueToAdd) {
        teleportPowerUps += valueToAdd;
        Debug.Log(teleportPowerUps);
    }
}
