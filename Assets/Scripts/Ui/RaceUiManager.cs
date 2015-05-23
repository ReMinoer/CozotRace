using System;
using UnityEngine;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine.UI;

public class RaceUiManager : MonoBehaviour
{
    public GameObject Vehicle;

    public GameObject SpeedText;
    public GameObject SpeedBar;
    public GameObject NitroBar;

    public GameObject PositionText;
    public GameObject PositionMaxText;
    public GameObject LapText;

    public GameObject TimeText;
    public GameObject CheckpointTimeText;
    public GameObject GapText;

    public GameObject CountdownText;

    private const float SpeedDisplayCoeff = 50;
    private const float CheckpointTimePeriod = 2;
    static private Color PositiveColor = Color.green;
    static private Color NegativeColor = Color.red;

    private bool _displayCheckpoint;
    private bool _displayGap;
    private float _checkpointTime;

	void Start ()
	{
        SpeedBar.GetComponent<Slider>().minValue = 0;
        SpeedBar.GetComponent<Slider>().maxValue = Vehicle.GetComponent<VehicleMotor>().ForwardSpeedMax;

        NitroBar.GetComponent<Slider>().minValue = 0;
        NitroBar.GetComponent<Slider>().maxValue = 1;
	}
	
	void Update ()
	{
	    var vehicleMotor = Vehicle.GetComponent<VehicleMotor>();

        // SpeedText
	    var speedToDisplay = (int)Mathf.Abs(vehicleMotor.SignedSpeed * SpeedDisplayCoeff);
        SpeedText.GetComponent<Text>().text = speedToDisplay.ToString();

        // SpeedBar
	    if (vehicleMotor.SignedSpeed >= 0)
	    {
            SpeedBar.GetComponent<Slider>().value = Mathf.Clamp(vehicleMotor.SignedSpeed, 0, vehicleMotor.ForwardSpeedMax);
            SpeedBar.GetComponent<Slider>().maxValue = vehicleMotor.ForwardSpeedMax;
	        SpeedBar.GetComponent<Slider>()
                .GetComponentsInChildren<Image>().First(image => image.type == Image.Type.Filled).color = PositiveColor;
	    }
	    else
        {
            SpeedBar.GetComponent<Slider>().value = Mathf.Clamp(-vehicleMotor.SignedSpeed, 0, vehicleMotor.BackwardSpeedMax);
            SpeedBar.GetComponent<Slider>().maxValue = Vehicle.GetComponent<VehicleMotor>().BackwardSpeedMax;
            SpeedBar.GetComponent<Slider>()
                .GetComponentsInChildren<Image>().First(image => image.type == Image.Type.Filled).color = NegativeColor;
        }

        // NitroBar
        NitroBar.GetComponent<Slider>().value = 0.75f;

        // PositionText
        PositionText.GetComponent<Text>().text = (GameManager.Instance.Contestants.IndexOf(Vehicle.transform.parent.gameObject) + 1).ToString();

        // PositionMaxText
        PositionMaxText.GetComponent<Text>().text = "/" + GameManager.Instance.Contestants.Count;

        // LapText
        LapText.GetComponent<Text>().text = string.Format("Lap : {0} of {1}", Vehicle.GetComponentInParent<Contestant>().CurrentLap, Race.Instance.Laps);
        
        // TimeText
        TimeText.GetComponent<Text>().text = GetChronometerText(GameManager.Instance.Chronometer, true);

	    if (Time.time - _checkpointTime > CheckpointTimePeriod)
        {
            _displayCheckpoint = false;
            _displayGap = false;
	    }

        // CheckpointTimeText
        CheckpointTimeText.GetComponent<Text>().enabled = _displayCheckpoint;

        // GapText
        GapText.GetComponent<Text>().enabled = _displayCheckpoint && _displayGap;

        // CountdownText
	    CountdownText.GetComponent<Text>().text = GameManager.Instance.CountdownEnabled
	        ? GameManager.Instance.Countdown.Seconds.ToString()
	        : "";
	}

    public void DisplayCheckpointTime()
    {
        _displayCheckpoint = true;
        _checkpointTime = Time.time;

        TimeSpan checkpoint = GameManager.Instance.Chronometer;

        CheckpointTimeText.GetComponent<Text>().text = GetChronometerText(checkpoint, true);
    }

    public void DisplayGapTime()
    {
        _displayGap = true;

        TimeSpan gap = TimeSpan.FromSeconds(0);

        GapText.GetComponent<Text>().color = gap.Ticks > 0 ? NegativeColor : PositiveColor;
        GapText.GetComponent<Text>().text = GetChronometerText(gap, false, true);
    }

    static private string GetChronometerText(TimeSpan timeSpan, bool alwaysDisplayMinutes, bool displaySignPlus = false)
    {
        string sign = timeSpan.Ticks >= 0 ? (displaySignPlus ? "+" : "") : "-";

        TimeSpan tempSpan = timeSpan;
        if (tempSpan.Ticks < 0)
            tempSpan = tempSpan.Negate();

        if (Mathf.Abs(tempSpan.Minutes) >= 10)
            return sign + string.Format("{0:00}:{1:00}.{2:000}",
                                        tempSpan.Minutes,
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        if (Mathf.Abs(tempSpan.Minutes) >= 1 || alwaysDisplayMinutes)
            return sign + string.Format("{0:0}:{1:00}.{2:000}",
                                        tempSpan.Minutes,
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        if (Mathf.Abs(tempSpan.Seconds) >= 10)
            return sign + string.Format("{0:00}.{1:000}",
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);

        return sign + string.Format("{0:0}.{1:000}",
                                        tempSpan.Seconds,
                                        tempSpan.Milliseconds);
    }
}
