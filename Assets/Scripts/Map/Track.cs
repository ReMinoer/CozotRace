using System;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Track : DesignPattern.Singleton<Track>
{
	protected Track() {}

	public GameObject StartPoint;

    private Vector3[] _points;
    private float[] _distances;

    public const float GizmoSubstep = 5;

    public float Length { get; private set; }

    public bool IsDisable = false;

	void Start()
	{
        UpdateCache();
		DisableAllInsertModes();
	}

    public void UpdateCache()
    {
        List<GameObject> trackpoints = GetAllPoints();

        _points = new Vector3[trackpoints.Count + 1];
        _distances = new float[trackpoints.Count + 1];

        float accumulateDistance = 0;
        for (int i = 0; i < _points.Length; i++)
        {
            GameObject t1 = trackpoints[(i) % trackpoints.Count];
            GameObject t2 = trackpoints[(i + 1) % trackpoints.Count];
            if (t1 == null || t2 == null)
                continue;

            Vector3 p1 = t1.transform.position;
            Vector3 p2 = t2.transform.position;
            _points[i] = trackpoints[i % trackpoints.Count].transform.position;
            _distances[i] = accumulateDistance;
            accumulateDistance += (p1 - p2).magnitude;
        }

        Length = _distances[_distances.Length - 1];
    }

    public Vector3 GetRoutePosition(float dist)
    {
        int point = 0;

        if (Math.Abs(Length) < float.Epsilon)
            UpdateCache();

        dist = Mathf.Repeat(dist, Length);

        while (_distances[point] < dist)
            point++;

        int pointsCount = _points.Length;

        int p1Index = ((point - 1) + pointsCount) % pointsCount;
        int p2Index = point;
        float i = Mathf.InverseLerp(_distances[p1Index], _distances[p2Index], dist);

        int p0Index = ((point - 2) + pointsCount) % pointsCount;
        int p3Index = (point + 1) % pointsCount;

        // 2nd point may have been the 'last' point - a dupe of the first,
        // (to give a value of max track distance instead of zero)
        // but now it must be wrapped back to zero if that was the case.
        p2Index = p2Index % pointsCount;

        Vector3 p0 = _points[p0Index];
        Vector3 p1 = _points[p1Index];
        Vector3 p2 = _points[p2Index];
        Vector3 p3 = _points[p3Index];

        return CatmullRom(p0, p1, p2, p3, i);
    }

    public RoutePoint GetRoutePoint(float dist)
    {
        // position and direction
        Vector3 p1 = GetRoutePosition(dist);
        Vector3 p2 = GetRoutePosition(dist + 0.1f);
        Vector3 delta = p2 - p1;
        return new RoutePoint(p1, delta.normalized);
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        return 0.5f *
               ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
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

    public int GetPointsCount()
    {
        if (StartPoint == null)
            return 0;

        GameObject currentPoint = StartPoint;
        int count = 1;
        while (currentPoint.GetComponent<TrackPoint>().NextPoint != null)
        {
            currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
            count++;
        }

        return count;
    }
	
	public GameObject GetLastPoint()
	{
	    if (StartPoint == null)
	        return null;

		GameObject currentPoint = StartPoint;
		while (currentPoint.GetComponent<TrackPoint>().NextPoint != null)
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;

		return currentPoint;
	}

    public GameObject GetPoint(int index)
    {
        if (StartPoint == null)
            return null;

        GameObject currentPoint = StartPoint;
        for (int i = 0; i < index; i++)
        {
            if (currentPoint.GetComponent<TrackPoint>().NextPoint == null)
                throw new IndexOutOfRangeException();

            currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
        }

        return currentPoint;
    }
	
	public void DisableAllInsertModes()
	{
		GameObject currentPoint = StartPoint;
		while (currentPoint != null)
		{
			currentPoint.GetComponent<TrackPoint>().InsertMode = false;
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
		}
	}
	
	public void DisableInsertModeOtherPoints(GameObject point)
	{
		GameObject currentPoint = StartPoint;
		while (currentPoint != null)
		{
			if (currentPoint == point)
			{
				currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
				continue;
			}
			
			currentPoint.GetComponent<TrackPoint>().InsertMode = false;
			currentPoint = currentPoint.GetComponent<TrackPoint>().NextPoint;
		}
	}

    void OnDisable()
    {
        IsDisable = true;
    }

    void OnDrawGizmos()
    {
        UpdateCache();
        Vector3 prev = _points[0];
        Gizmos.color = Color.blue;
        for (float dist = 0; dist < Length; dist += 1)
        {
            Vector3 next = GetRoutePosition(dist);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
        Gizmos.DrawLine(prev, _points[0]);
    }

    public struct RoutePoint
    {
        public Vector3 Position;
        public Vector3 Direction;

        public RoutePoint(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
