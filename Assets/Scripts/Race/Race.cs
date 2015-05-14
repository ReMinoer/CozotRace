using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Race : DesignPattern.Singleton<Race> {

	public List<PlayerVehicleData> Players; 
	public List<AiVehicleData> Ais;
	public List<ReplayVehicleData> Replays;
	public int Laps;
	public GameObject FirstCheckPoint;

	protected Race() {}

	public int CheckPointsCount
	{
		get
		{
			int Count = 1;
			CheckPoint Pt = FirstCheckPoint.GetComponent<CheckPoint>();
			while (Pt.NextPoint != null) {
				Pt = Pt.NextPoint.GetComponent<CheckPoint>();
				Count++;
			}
			return Count;
		}
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

	void Awake() {

	}

	void Start()
	{
		DisableInsertModeAllPoints();
	}

	void InitializeVehiclePosition() {

	}



}
