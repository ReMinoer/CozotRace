using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Contestant : MonoBehaviour
{
    public string PlayerName { get; set; }
    public int CurrentLap { get; private set; }
    public CheckPoint CurrentCheckPoint { get; private set; }
    public ReadOnlyCollection<TimeSpan> SplitTimes
    {
        get { return _splitTimes.AsReadOnly(); }
    }

    private List<TimeSpan> _splitTimes;

	public void ValidateCheckPoint(CheckPoint Cp)
	{
		if (Cp == CurrentCheckPoint)
        {
            _splitTimes.Add(GameManager.Instance.Chronometer);

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
                        GameManager.Instance.EndRace(this);
                    }
                }
                CurrentCheckPoint = Cp.NextPoint.GetComponent<CheckPoint>();
            }


            var ui = gameObject.GetComponentInChildren<RaceUiManager>();
            if (ui != null)
            {
                ui.DisplayCheckpointTime(_splitTimes[_splitTimes.Count - 1]);
                var firstContestant = GameManager.Instance.Contestants[0].GetComponent<Contestant>();
                if (gameObject != firstContestant.gameObject)
                {
                    int checkpointIndex = _splitTimes.Count - 1;
                    ui.DisplayGapTime(_splitTimes[checkpointIndex] - firstContestant.SplitTimes[checkpointIndex]);
                }
            }

            if (GameManager.Instance.Contestants.Count > 1 && gameObject == GameManager.Instance.Contestants[1])
            {
                var firstPlayerUi = GameManager.Instance.Contestants[0].GetComponentInChildren<RaceUiManager>();
                if (firstPlayerUi != null)
                {
                    var firstContestant = firstPlayerUi.gameObject.GetComponentInParent<Contestant>();
                    if (gameObject != firstContestant.gameObject)
                    {
                        if (_splitTimes.Count == firstContestant.SplitTimes.Count)
                        {
                            int checkpointIndex = _splitTimes.Count - 1;
                            firstPlayerUi.DisplayGapTime(
                                firstContestant.SplitTimes[checkpointIndex] - _splitTimes[checkpointIndex]);
                        }
                    }
                }
            }
        }
		else
        {
			Debug.Log("miss point");
		}

	}


	// Use this for initialization
	void Start ()
    {
        _splitTimes = new List<TimeSpan>();

		CurrentLap = 0;
        if (Race.Instance.FirstCheckPoint != null)
            CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
