using UnityEngine;
using System.Collections;

public class Contestant : MonoBehaviour {
    public int CurrentLap { get; private set; }
    public CheckPoint CurrentCheckPoint { get; private set; }

	public void ValidateCheckPoint(CheckPoint Cp)
	{
		if (Cp == CurrentCheckPoint)
        {
			if (CurrentCheckPoint.NextPoint == null)
            {
				CurrentLap++;
				CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
				if (Race.Instance.Laps < CurrentLap)
				    GameManager.Instance.ChangeState(new FinishedGameState(GameManager.Instance));
			}
			else
				CurrentCheckPoint = Cp.NextPoint.GetComponent<CheckPoint>();
		}
		else
        {
			Debug.Log("miss point");
		}

	}


	// Use this for initialization
	void Start ()
    {
		CurrentLap = 1;
		CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
