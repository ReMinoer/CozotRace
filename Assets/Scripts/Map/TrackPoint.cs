using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TrackPoint : LinkedPoint
{
    private bool _isQuitting = false;

    protected override void Awake()
    {
        if (FindObjectOfType<Track>() != null)
            base.Awake();
    }

    void Start()
	{
		if (!Application.isPlaying)
			return;
		
		// Apply trackpoint on relief
		int layerMask = 1 << LayerMask.NameToLayer("Map");
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
			this.transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
	}
	
	protected override GameObject GetLastPoint()
	{
		return Track.Instance.GetLastPoint();
	}

	protected override List<GameObject> GetAllPoints()
	{
		return Track.Instance.GetAllPoints();
	}

	protected override void DisableInsertModeOtherPoints(GameObject gameObject)
	{
		Track.Instance.DisableInsertModeOtherPoints(this.gameObject);
	}

    protected override void OnDestroy()
    {
        if (!Track.Instance.IsDisable)
			base.OnDestroy ();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
