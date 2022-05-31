using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPollingManager {
    [SerializeField] private float duration;
    [SerializeField] private float knockback;
    [SerializeField] private float velocity;
    [SerializeField] private float bulletSize;
    [HideInInspector] public bool isActive;
    private Vector3 targetStartLocation;
    private Transform targetTransformRef;
    private float currentDuration;
    public bool IsActive { get { return isActive; } set { IsActive = isActive; } }
    // Update is called once per frame
    void Update() {
        transform.position += Time.deltaTime * velocity * targetStartLocation.normalized;
        currentDuration += Time.deltaTime;
        if (currentDuration > duration) Activate(false, Vector3.zero, null);
        else if (Vector3.Distance(transform.position, targetTransformRef.position) <= bulletSize) {
            targetTransformRef.GetComponent<Rigidbody>().AddForce((targetTransformRef.position - transform.position).normalized * knockback, ForceMode.Impulse);
            Activate(false, Vector3.zero, null);
        }
    }

    public void Activate(bool state, Vector3 targetlocation, Transform targetTransform) {
        targetStartLocation = targetlocation;
        targetTransformRef = targetTransform;
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }
    public IEnumerator Activate(bool state, float delay, float[] targetLocation = null, GameObject targetRef = null) {
        yield return new WaitForSeconds(delay);
        if (targetLocation != null) targetStartLocation.Set(targetLocation[0], targetLocation[1], targetLocation[2]);
        if(targetRef != null) targetTransformRef = targetRef.transform;
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bulletSize);
    }


#endif
}
