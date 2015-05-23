using UnityEngine;
using System.Collections;

public class Contestant : MonoBehaviour
{
    public int CurrentLap { get; private set; }
    public CheckPoint CurrentCheckPoint { get; private set; }

	public void ValidateCheckPoint(CheckPoint Cp)
	{
		if (Cp == CurrentCheckPoint)
        {
            //Debug.Log("Next checkpoint");
            if (CurrentCheckPoint.NextPoint == null)
            {
                CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
            }
            else
            {
                if (CurrentCheckPoint == Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>())
                {
                    //Debug.Log("Next lap");
                    CurrentLap++;
                    if (CurrentLap > Race.Instance.Laps)
                    {
                        //Debug.Log("Finish race");
                        GameManager.Instance.ChangeState(new FinishedGameState(GameManager.Instance));
                    }
                }
                CurrentCheckPoint = Cp.NextPoint.GetComponent<CheckPoint>();
            }

            var ui = gameObject.GetComponentInChildren<RaceUiManager>();
            if (ui != null)
                ui.DisplayCheckpointTime();
        }
		else
        {
			Debug.Log("miss point");
		}

	}


	// Use this for initialization
	void Start ()
    {
		CurrentLap = 0;
        if (Race.Instance.FirstCheckPoint != null)
            CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
