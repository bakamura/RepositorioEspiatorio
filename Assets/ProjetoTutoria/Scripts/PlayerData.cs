using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static Transform playerPosition;
    public static bool[] shrinesDone;

    public static void SaveShrine(int shrineID)
    {
        shrinesDone[shrineID] = true;
    }

    public static void SavePosition(Transform transform)
    {
        playerPosition = transform;
    }
}
