using System.Collections;
using UnityEngine;

public interface IObjectPollingManager {
    public bool IsActive { get; set; }
    public IEnumerator Activate(bool state, float delay, float[] targetLocation = null, GameObject targetRef = null);
}