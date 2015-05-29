using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrackPoint : LinkedPoint
{
    protected override void Awake()
    {
        if (FindObjectOfType<Track>() != null)
            base.Awake();
    }

    protected override GameObject GetLastPoint()
    {
        return Track.Instance.GetLastPoint();
    }

    protected override List<GameObject> GetAllPoints()
    {
        return Track.Instance.GetAllPoints();
    }

    protected override void DisableInsertModeOtherPoints(GameObject activePoint)
    {
        Track.Instance.DisableInsertModeOtherPoints(activePoint);
    }

    protected override void OnDestroy()
    {
        if (FindObjectOfType<Track>() != null && !Track.Instance.IsDisable)
            base.OnDestroy();
    }

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        // Apply trackpoint on relief
        int layerMask = 1 << LayerMask.NameToLayer("Map");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}