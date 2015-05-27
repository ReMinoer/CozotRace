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
            ValidateProgression();

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
		    var aiInput = gameObject.GetComponent<AiInput>();
		    if (aiInput != null)
            {
                do
                {
                    ValidateProgression();
                } while (CurrentCheckPoint == Cp.GetComponent<CheckPoint>());

                _splitTimes.Add(GameManager.Instance.Chronometer);

                ValidateProgression();
                return;
            }

			CheckPoint old = CurrentCheckPoint;
			for(int i=0; i<3; i++)
            {
				if (old.PreviousPoint==null)
					old = Race.Instance.GetLastCheckPoint().GetComponent<CheckPoint>();
				else
					old = old.PreviousPoint.GetComponent<CheckPoint>();

				if (Cp == old)
					return;
			}
			RespawnAtNextCheckPoint();
		}
	}

    private void ValidateProgression()
    {
        _splitTimes.Add(GameManager.Instance.Chronometer);

        if (CurrentCheckPoint.NextPoint == null)
        {
            CurrentCheckPoint = Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>();
        }
        else
        {
            if (CurrentCheckPoint == Race.Instance.FirstCheckPoint.GetComponent<CheckPoint>())
            {
                CurrentLap++;
                if (CurrentLap > Race.Instance.Laps)
                {
                    GameManager.Instance.EndRace(this);
                }
            }
            CurrentCheckPoint = CurrentCheckPoint.NextPoint.GetComponent<CheckPoint>();
        }
    }

	void RespawnAtNextCheckPoint() {
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		transform.rotation = CurrentCheckPoint.transform.rotation;
		transform.position = CurrentCheckPoint.transform.position;
		transform.Translate (-transform.forward*3);
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
