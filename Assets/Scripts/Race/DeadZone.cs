using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var progressTracker = other.gameObject.GetComponent<ProgressTracker>();
        if (progressTracker == null)
            return;

        other.gameObject.transform.position = progressTracker.RoutePosition;
        other.gameObject.transform.rotation = progressTracker.RouteRotation;
    }
}