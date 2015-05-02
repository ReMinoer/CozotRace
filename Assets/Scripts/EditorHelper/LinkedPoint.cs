using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract class LinkedPoint : MonoBehaviour
{
	public GameObject PreviousPoint;
	public GameObject NextPoint;
	
	public bool InsertMode = false;
	
	void Awake()
	{
		if(Application.isEditor && !Application.isPlaying)
		{
			if (this.NextPoint == null && this.PreviousPoint == null)
				return;
			
			// Allow to duplicate last point of the track
			GameObject lastPoint = GetLastPoint();
			if (this.NextPoint == null && lastPoint != this.gameObject)
				Link(lastPoint, this.gameObject);
		}
	}
	
	void Update ()
	{
		// Handle the insertion mode
		if (InsertMode && Application.isEditor && !Application.isPlaying)
		{
			bool insertionFind = false;
			bool sameInsertion = false;
			
			List<GameObject> points = GetAllPoints();
			points.Remove(this.gameObject);
			points.Sort((a, b) => (a.transform.position - transform.position).magnitude.CompareTo(
				(b.transform.position - transform.position).magnitude));
			
			List<GameObject> alreadySeePoints = new List<GameObject>();
			GameObject previousPoint = null, nextPoint = null;
			
			foreach (GameObject newNearest in points)
			{
				foreach (GameObject alreadySee in alreadySeePoints)
				{
					if (this.NextPoint == newNearest && this.PreviousPoint == alreadySee)
					{
						sameInsertion = true;
						break;
					}
					if (this.PreviousPoint == newNearest && this.NextPoint == alreadySee)
					{
						sameInsertion = true;
						break;
					}
					if (newNearest.GetComponent<LinkedPoint>().NextPoint == alreadySee)
					{
						previousPoint = newNearest;
						nextPoint = alreadySee;
						insertionFind = true;
						break;
					}
					if (newNearest.GetComponent<LinkedPoint>().PreviousPoint == alreadySee)
					{
						previousPoint = alreadySee;
						nextPoint = newNearest;
						insertionFind = true;
						break;
					}
				}
				
				if (insertionFind || sameInsertion)
					break;
				else
					alreadySeePoints.Add(newNearest);
			}
			
			if (insertionFind)
			{
				Unlink(this.gameObject);
				
				Link(this.gameObject, nextPoint);
				Link(previousPoint, this.gameObject);
			}
		}
	}
	
	private static void Link(GameObject point, GameObject nextPoint)
	{
		point.GetComponent<LinkedPoint>().NextPoint = nextPoint;
		if (nextPoint != null)
			nextPoint.GetComponent<LinkedPoint>().PreviousPoint = point;
	}
	
	private static void Unlink(GameObject point)
	{
		LinkedPoint trackPoint = point.GetComponent<LinkedPoint>();
		
		if (trackPoint.NextPoint != null)
			trackPoint.NextPoint.GetComponent<LinkedPoint>().PreviousPoint = trackPoint.PreviousPoint;
		if (trackPoint.PreviousPoint != null)
			trackPoint.PreviousPoint.GetComponent<LinkedPoint>().NextPoint = trackPoint.NextPoint;
	}
	
	protected virtual void OnDestroy()
	{
		Unlink(this.gameObject);
	}
	
	void OnValidate()
	{
		if (InsertMode)
			DisableInsertModeOtherPoints(this.gameObject);
		
		if (PreviousPoint != null)
		{
			GameObject lastPoint = GetLastPoint();
			if (PreviousPoint == lastPoint.GetComponent<LinkedPoint>().PreviousPoint
			    && NextPoint == lastPoint.GetComponent<LinkedPoint>().NextPoint)
				return;
		}
		
		if (PreviousPoint != null)
			PreviousPoint.GetComponent<LinkedPoint>().NextPoint = this.gameObject;
		if (NextPoint != null)
			NextPoint.GetComponent<LinkedPoint>().PreviousPoint = this.gameObject;
	}

	protected abstract GameObject GetLastPoint();
	protected abstract List<GameObject> GetAllPoints();
	protected abstract void DisableInsertModeOtherPoints(GameObject gameObject);
}