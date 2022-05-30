using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {
    [SerializeField] private float sizeIncrease;
    [SerializeField] private float duration;
    [SerializeField] private float knockback;
    private float currentDuration;
    [HideInInspector] public bool isActive;
    void Update()
    {
        transform.localScale += new Vector3(sizeIncrease, 0, sizeIncrease) * Time.deltaTime;
        currentDuration += Time.deltaTime;
        if (currentDuration >= duration) Activate(false);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockback, ForceMode.Impulse);
    }

    public void Activate(bool state) {
        transform.localScale = Vector3.zero;
        isActive = state;
        this.gameObject.SetActive(state);
    }
}
