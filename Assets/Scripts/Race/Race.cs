using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class Race : Singleton<Race>
{
    public GameObject FirstCheckPoint;
    public int Laps;

    public int CheckPointsCount
    {
        get
        {
            int count = 1;
            var pt = FirstCheckPoint.GetComponent<CheckPoint>();
            while (pt.NextPoint != null)
            {
                pt = pt.NextPoint.GetComponent<CheckPoint>();
                count++;
            }
            return count;
        }
    }

    protected Race()
    {
    }

    public List<GameObject> GetAllPoints()
    {
        var list = new List<GameObject>();

        GameObject currentPoint = FirstCheckPoint;
        while (currentPoint != null)
        {
            list.Add(currentPoint);
            currentPoint = currentPoint.GetComponent<CheckPoint>().NextPoint;
        }

        return list;
    }

    public GameObject GetLastCheckPoint()
    {
        GameObject currentPoint = FirstCheckPoint;
        while (currentPoint.GetComponent<CheckPoint>().NextPoint != null)
            currentPoint = currentPoint.GetComponent<CheckPoint>().NextPoint;

        return currentPoint;
    }

    public void DisableInsertModeOtherPoints(GameObject point)
    {
        GameObject currentPoint = FirstCheckPoint;
        while (currentPoint != null)
        {
            if (currentPoint == point)
            {
                currentPoint = currentPoint.GetComponent<CheckPoint>().NextPoint;
                continue;
            }

            currentPoint.GetComponent<CheckPoint>().InsertMode = false;
            currentPoint = currentPoint.GetComponent<CheckPoint>().NextPoint;
        }
    }

    public void DisableInsertModeAllPoints()
    {
        GameObject currentPoint = FirstCheckPoint;
        while (currentPoint != null)
        {
            currentPoint.GetComponent<CheckPoint>().InsertMode = false;
            currentPoint = currentPoint.GetComponent<CheckPoint>().NextPoint;
        }
    }

    private void Start()
    {
        DisableInsertModeAllPoints();
    }
}