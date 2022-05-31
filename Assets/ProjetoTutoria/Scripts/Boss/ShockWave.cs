using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour, IObjectPollingManager {
    [SerializeField] private float sizeIncrease;
    [SerializeField] private float duration;
    //[SerializeField] private float knockback;
    private float currentDuration;
    private bool isActive;

    public bool IsActive { get { return isActive; } set { IsActive = isActive; } }

    void Update()
    {
        transform.localScale += new Vector3(sizeIncrease, 0, sizeIncrease) * Time.deltaTime;
        currentDuration += Time.deltaTime;
        if (currentDuration >= duration) StartCoroutine(Activate(false, 0));
    }
    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.CompareTag("Player")) collision.rigidbody.AddForce((collision.transform.position - transform.position).normalized * knockback, ForceMode.Impulse);
    //}

    public IEnumerator Activate(bool state, float delay, float[] targetLocation = null, GameObject targetRef = null) {
        yield return new WaitForSeconds(delay);
        transform.localScale = new Vector3(0, transform.lossyScale.y, 0);
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }
}
