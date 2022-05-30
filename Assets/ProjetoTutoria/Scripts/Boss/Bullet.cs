using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float knockback;
    [SerializeField] private float velocity;
    [HideInInspector] public bool isActive;
    [HideInInspector] public Vector3 target;
    private float currentDuration;
    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * velocity * target.normalized;
        currentDuration += Time.deltaTime;
        if (currentDuration > duration) Activate(false, Vector3.zero);
    }

    public void Activate(bool state, Vector3 targetlocation) {
        transform.localScale = Vector3.negativeInfinity;
        target = targetlocation;
        isActive = state;
        this.gameObject.SetActive(state);
    }
}
