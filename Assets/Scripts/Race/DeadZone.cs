using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        ProgressTracker progressTracker = other.gameObject.GetComponent<ProgressTracker>();
        if (progressTracker == null)
            return;

        other.gameObject.transform.position = progressTracker.RoutePosition;
        other.gameObject.transform.rotation = progressTracker.RouteRotation;
    }
}
