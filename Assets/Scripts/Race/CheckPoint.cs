using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CheckPoint : LinkedPoint
{
	void OnTriggerEnter(Collider Col)
	{
		if (Col.isTrigger && Col.gameObject.GetComponent<Contestant>() != null)	Col.gameObject.GetComponent<Contestant> ().ValidateCheckPoint (this);

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position,Vector3.one);
		
		if (NextPoint != null)
			Gizmos.DrawLine(transform.position, NextPoint.transform.position);
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawCube(transform.position,Vector3.one);
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
