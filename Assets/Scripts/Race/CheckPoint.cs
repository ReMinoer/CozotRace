using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CheckPoint : LinkedPoint
{
    protected override void Awake()
    {
        if (FindObjectOfType<Race>() != null)
            base.Awake();
    }

	void OnTriggerEnter(Collider Col)
	{
		if (Col.isTrigger && Col.gameObject.GetComponent<Contestant>() != null)
            Col.gameObject.GetComponent<Contestant> ().ValidateCheckPoint (this);
	}

	void OnDrawGizmos()
	{
		if (NextPoint != null)
        {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine (transform.position, NextPoint.transform.position);
		}
		else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, Race.Instance.FirstCheckPoint.transform.position);
		}
	}

	protected override GameObject GetLastPoint()
	{
		return Race.Instance.GetLastCheckPoint();
	}
	protected override List<GameObject> GetAllPoints()
	{
		return Race.Instance.GetAllPoints();
	}
	protected override void DisableInsertModeOtherPoints(GameObject gameObject)
	{
		Race.Instance.DisableInsertModeOtherPoints(gameObject);
	}
}
