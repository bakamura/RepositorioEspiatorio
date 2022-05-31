using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float sizeIncrease;
    [SerializeField] private float maxSize;
    [SerializeField] private float duration;
    [SerializeField] private float knockback;
    [SerializeField] private float startDelay;
    private Collider hitbox;
    private float currentDuration;
    [HideInInspector] public bool isActive;
    private bool startIncreaseSize = false;

    private void Awake() {
        hitbox = GetComponentInChildren<Collider>();
    }
    void Update() {
        if (startIncreaseSize) {
            if (transform.localScale.x < maxSize) transform.localScale += new Vector3(sizeIncrease, sizeIncrease, 0) * Time.deltaTime;
            if (transform.localScale.x >= maxSize / 2 && !hitbox.enabled) hitbox.enabled = true;
            currentDuration += Time.deltaTime;
            if (currentDuration >= duration) Activate(false);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockback, ForceMode.Impulse);
    }
    public void Activate(bool state) {
        startIncreaseSize = false;
        transform.localScale = new Vector3(0,0,transform.lossyScale.z);
        hitbox.enabled = false;
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
        if (state)Invoke(nameof(StartUpdate), startDelay);
    }
    private void StartUpdate() {
        startIncreaseSize = true;
    }
}
