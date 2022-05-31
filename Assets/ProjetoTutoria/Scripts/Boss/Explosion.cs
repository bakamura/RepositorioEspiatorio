using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IObjectPollingManager
{
    [SerializeField, Tooltip("the complete duration of the explosion")] private float duration;
    [SerializeField, Range(0f, 1f), Tooltip("when the bomb will explode in percent of the duration")] private float explosionTime;
    [SerializeField] private float knockback;
    [SerializeField] private float velocity;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionParticle;
    private bool isActive;
    private Vector3 target;
    private float currentDuration;
    private const int playerLayer = 8;
    public bool IsActive { get { return isActive; } set { IsActive = isActive; } }
    void Update() {
        currentDuration += Time.deltaTime;
        if (currentDuration < duration * explosionTime) transform.position += Time.deltaTime * velocity * target.normalized;
        else Explode();        
    }

    public void Activate(bool state, Vector3 targetlocation) {
        target = targetlocation;
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }

    private void Explode() {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);
        if (hits.Length > 0) hits[0].attachedRigidbody.AddForce((hits[0].transform.position - transform.position).normalized * knockback, ForceMode.Impulse);
        Instantiate(explosionParticle, transform.position, Quaternion.identity, null);
        Activate(false, Vector3.zero);
    }
    public IEnumerator Activate(bool state, float delay, float[] targetLocation = null, GameObject targetRef = null) {
        yield return new WaitForSeconds(delay);
        if (targetLocation != null) target.Set(targetLocation[0], targetLocation[1], targetLocation[2]);
        isActive = state;
        currentDuration = 0;
        this.gameObject.SetActive(state);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

#endif
}
