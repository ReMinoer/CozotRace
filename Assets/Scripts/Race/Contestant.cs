using UnityEngine;
using System.Collections;

public class Contestant : MonoBehaviour {
	private int _currentLap;
	private CheckPoint _currentCheckPoint;

	void ValidateCheckPoint (CheckPoint CP) {

		if (_currentCheckPoint == CP) {
			_currentLap++;
		
			if (Race.Instance.Laps < _currentLap) {
				//event fin de course
			} else {
				_currentCheckPoint = Race.Instance.FirstCheckPoint;
			}
		} else {
			//_currentCheckPoint = CP.next ();
		}


	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
