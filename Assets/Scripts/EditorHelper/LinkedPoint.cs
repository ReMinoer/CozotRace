using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LinkedPoint : MonoBehaviour
{
    public GameObject PreviousPoint;
    public GameObject NextPoint;
    public bool InsertMode;

    protected virtual void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (NextPoint == null && PreviousPoint == null)
                return;

            // Allow to duplicate last point of the track
            GameObject lastPoint = GetLastPoint();
            if (NextPoint == null && lastPoint != gameObject)
                Link(lastPoint, gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        Unlink(gameObject);
    }

    protected abstract GameObject GetLastPoint();
    protected abstract List<GameObject> GetAllPoints();
    protected abstract void DisableInsertModeOtherPoints(GameObject gameObject);

    private void Update()
    {
        // Handle the insertion mode
        if (InsertMode && Application.isEditor && !Application.isPlaying)
        {
            bool insertionFind = false;
            bool sameInsertion = false;

            List<GameObject> points = GetAllPoints();
            points.Remove(gameObject);
            points.Sort((a, b) => (a.transform.position - transform.position).magnitude.CompareTo(
                (b.transform.position - transform.position).magnitude));

            var alreadySeePoints = new List<GameObject>();
            GameObject previousPoint = null, nextPoint = null;

            foreach (GameObject newNearest in points)
            {
                foreach (GameObject alreadySee in alreadySeePoints)
                {
                    if (NextPoint == newNearest && PreviousPoint == alreadySee)
                    {
                        sameInsertion = true;
                        break;
                    }
                    if (PreviousPoint == newNearest && NextPoint == alreadySee)
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
                alreadySeePoints.Add(newNearest);
            }

            if (insertionFind)
            {
                Unlink(gameObject);

                Link(gameObject, nextPoint);
                Link(previousPoint, gameObject);
            }
        }
    }

    static private void Link(GameObject point, GameObject nextPoint)
    {
        point.GetComponent<LinkedPoint>().NextPoint = nextPoint;
        if (nextPoint != null)
            nextPoint.GetComponent<LinkedPoint>().PreviousPoint = point;
    }

    static private void Unlink(GameObject point)
    {
        var trackPoint = point.GetComponent<LinkedPoint>();

        if (trackPoint.NextPoint != null)
            trackPoint.NextPoint.GetComponent<LinkedPoint>().PreviousPoint = trackPoint.PreviousPoint;
        if (trackPoint.PreviousPoint != null)
            trackPoint.PreviousPoint.GetComponent<LinkedPoint>().NextPoint = trackPoint.NextPoint;
    }

    private void OnValidate()
    {
        if (InsertMode)
            DisableInsertModeOtherPoints(gameObject);

        if (PreviousPoint != null)
        {
            GameObject lastPoint = GetLastPoint();
            if (PreviousPoint == lastPoint.GetComponent<LinkedPoint>().PreviousPoint
                && NextPoint == lastPoint.GetComponent<LinkedPoint>().NextPoint)
                return;
        }

        if (PreviousPoint != null)
            PreviousPoint.GetComponent<LinkedPoint>().NextPoint = gameObject;
        if (NextPoint != null)
            NextPoint.GetComponent<LinkedPoint>().PreviousPoint = gameObject;
    }
}