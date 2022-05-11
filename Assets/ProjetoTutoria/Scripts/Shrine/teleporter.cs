using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    [SerializeField] private Transform returnPoint;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") other.transform.position = returnPoint.position;
    }
}
