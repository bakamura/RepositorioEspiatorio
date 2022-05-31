using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour/*, ObjectPollingManager*/ {
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
        transform.localScale = new Vector3(0, transform.lossyScale.y, 0);
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }

    //public IEnumerator Activate(bool state, float delay, float[] targetPosition = null, Transform targtetTransform = null) {
    //    yield return new WaitForSeconds(delay);
    //    transform.localScale = new Vector3(0, transform.lossyScale.y, 0);
    //    isActive = state;
    //    currentDuration = 0;
    //    this.gameObject.SetActive(state);
    //}
}
