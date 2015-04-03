using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Race : DesignPattern.Singleton<Race> {

	public List<PlayerVehicleData> Players; 
	public List<AiVehicleData> Ais;
	public List<ReplayVehicleData> Replays;
	public int Laps;
	public CheckPoint FirstCheckPoint;
	/*
	public int CheckPointsCount { get { } }

	CheckPoint GetLastCheckPoint () {

	}

	void Awake() {

	}

	void InitializeVehiclePosition() {

	}

*/

}
