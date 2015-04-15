using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

public class RaceUiManager : MonoBehaviour
{
    public GameObject Vehicle;

    public GameObject SpeedText;

    private const float SpeedDisplayCoeff = 50;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var speedToDisplay = (int)Mathf.Abs(Vehicle.GetComponent<VehicleMotor>().SignedSpeed * SpeedDisplayCoeff);
        SpeedText.GetComponent<Text>().text = speedToDisplay.ToString();
	}
}
