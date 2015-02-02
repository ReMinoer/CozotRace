using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class TrackPoint : MonoBehaviour {

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
			GameObject lastPoint = Track.Instance.GetLastPoint();
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

			List<GameObject> points = Track.Instance.GetAllPoints();
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
					if (newNearest.GetComponent<TrackPoint>().NextPoint == alreadySee)
					{
						previousPoint = newNearest;
						nextPoint = alreadySee;
						insertionFind = true;
						break;
					}
					if (newNearest.GetComponent<TrackPoint>().PreviousPoint == alreadySee)
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
		point.GetComponent<TrackPoint>().NextPoint = nextPoint;
		if (nextPoint != null)
			nextPoint.GetComponent<TrackPoint>().PreviousPoint = point;
	}
	
	private static void Unlink(GameObject point)
	{
		TrackPoint trackPoint = point.GetComponent<TrackPoint>();
		
		if (trackPoint.NextPoint != null)
			trackPoint.NextPoint.GetComponent<TrackPoint>().PreviousPoint = trackPoint.PreviousPoint;
		if (trackPoint.PreviousPoint != null)
			trackPoint.PreviousPoint.GetComponent<TrackPoint>().NextPoint = trackPoint.NextPoint;
	}

	void OnDestroy()
	{
		Unlink(this.gameObject);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position, 0.2f);

		if (NextPoint != null)
			Gizmos.DrawLine(transform.position, NextPoint.transform.position);
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position, 0.2f);
		
		if (NextPoint != null)
		{
			Gizmos.DrawLine(transform.position, NextPoint.transform.position);
			Gizmos.DrawSphere((transform.position + NextPoint.transform.position) / 2, 0.1f);
		}
		
		if (PreviousPoint != null)
			Gizmos.DrawSphere((transform.position + PreviousPoint.transform.position) / 2, 0.1f);
	}

	void OnValidate()
	{
		if (InsertMode)
			Track.Instance.DisableInsertModeOtherPoints(this.gameObject);

		GameObject lastPoint = Track.Instance.GetLastPoint();
		if (PreviousPoint == lastPoint.GetComponent<TrackPoint>().PreviousPoint
		    && NextPoint == lastPoint.GetComponent<TrackPoint>().NextPoint)
			return;

		if (PreviousPoint != null)
			PreviousPoint.GetComponent<TrackPoint>().NextPoint = this.gameObject;
		if (NextPoint != null)
			NextPoint.GetComponent<TrackPoint>().PreviousPoint = this.gameObject;
	}
}
