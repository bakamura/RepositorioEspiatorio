using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpring : MonoBehaviour {

    [SerializeField] private float _strengh;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player" && collision.rigidbody.transform.position.y - (collision.rigidbody.transform.lossyScale.y / 2f) < transform.position.y + (transform.lossyScale.y / 2f) + 0.05f) collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * _strengh, ForceMode.Acceleration);
    }
}
