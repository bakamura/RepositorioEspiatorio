using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField, Tooltip("the complete duration of the explosion")] private float duration;
    [SerializeField, Tooltip("when the bomb will explode in percent of the duration")] private float explosionTime;
    [SerializeField] private float knockback;
    [SerializeField] private float velocity;
    [HideInInspector] public bool isActive;
    [HideInInspector] public Vector3 target;
    private float currentDuration;
    // Update is called once per frame
    void Update() {
        currentDuration += Time.deltaTime;
        if (currentDuration < duration) transform.position += Time.deltaTime * velocity * target.normalized;
        else {

        }
    }

    public void Activate(bool state, Vector3 targetlocation) {
        transform.localScale = Vector3.negativeInfinity;
        target = targetlocation;
        isActive = state;
        this.gameObject.SetActive(state);
    }

    private void Explode() {

    }
}
