using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Track : DesignPattern.Singleton<Track> {
	
	protected Track() {}

	public GameObject StartPoint;

	public void DisableAllInsertModes()
	{
		GameObject currentPoint = StartPoint;
		while (currentPoint != null)
		{
			currentPoint.GetComponent<TrackPoint>().InsertMode = false;
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
		}
	}
	
	public List<GameObject> GetAllPoints()
	{
		var list = new List<GameObject>();

		GameObject currentPoint = StartPoint;
		while (currentPoint != null)
		{
			list.Add(currentPoint);
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
		}
		
		return list;
	}
	
	public GameObject GetLastPoint()
	{
		GameObject currentPoint = StartPoint;
		while (currentPoint.GetComponent<TrackPoint>().NextPoint != null)
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;

		return currentPoint;
	}
}
