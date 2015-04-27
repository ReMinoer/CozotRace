using UnityEngine;
using System.Collections;

public class Contestant : MonoBehaviour {
	private int _currentLap;
	private CheckPoint _currentCheckPoint;

	public void ValidateCheckPoint(CheckPoint Cp)
	{
		if (Cp == _currentCheckPoint) {
			Debug.Log ("test");
			if (_currentCheckPoint.NextPoint == null) {
				_currentLap++;
				if (Race.Instance.Laps < _currentLap) {
					Debug.Log("fin de course");
				} 
				else {
					Debug.Log("nouveau tour");
					_currentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
				}
			}
			else {
				//maj du checkpoint
				_currentCheckPoint = Cp.NextPoint.GetComponent<CheckPoint>();
			}
		}
		else {
			Debug.Log("miss point");
		}

	}


	// Use this for initialization
	void Start () {
		_currentLap = 1;
		_currentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
