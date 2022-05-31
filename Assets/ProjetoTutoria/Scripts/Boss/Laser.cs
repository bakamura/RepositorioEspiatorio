using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IObjectPollingManager
{
    [SerializeField] private float sizeIncrease;
    [SerializeField] private float maxSize;
    [SerializeField] private float duration;
    [SerializeField] private float knockback;
    [SerializeField] private float startDelay;
    [SerializeField] private Collider hitbox;
    private float currentDuration;
    private bool isActive;
    private bool startIncreaseSize = false;
    private Vector3 baseSize = Vector3.zero;
    public bool IsActive { get { return isActive; } set { IsActive = isActive; } }

    void Update() {
        if (startIncreaseSize) {
            if (transform.localScale.x < maxSize) transform.localScale += new Vector3(sizeIncrease, sizeIncrease, 0) * Time.deltaTime;
            if (transform.localScale.x >= maxSize / 2 && !hitbox.enabled) hitbox.enabled = true;
            currentDuration += Time.deltaTime;
            if (currentDuration >= duration) StartCoroutine(Activate(false, 0));
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockback, ForceMode.Impulse);
    }

    public IEnumerator Activate(bool state, float delay, float[] targetLocation = null, GameObject targetRef = null) {
        yield return new WaitForSeconds(delay);
        startIncreaseSize = false;
        if (baseSize == Vector3.zero) baseSize = transform.lossyScale;
        transform.localScale = baseSize;
        hitbox.enabled = false;
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
        if (state) Invoke(nameof(StartUpdate), startDelay);
    }

    private void StartUpdate() {
        startIncreaseSize = true;
    }

}
