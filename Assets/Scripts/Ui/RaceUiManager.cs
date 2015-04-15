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

    private const float SpeedDisplayCoeff = 50;

	void Start ()
	{
        SpeedBar.GetComponent<Slider>().minValue = 0;
        SpeedBar.GetComponent<Slider>().maxValue = Vehicle.GetComponent<VehicleMotor>().ForwardSpeedMax;

        NitroBar.GetComponent<Slider>().minValue = 0;
        NitroBar.GetComponent<Slider>().maxValue = 1;
	}
	
	void Update ()
	{
	    VehicleMotor vehicleMotor = Vehicle.GetComponent<VehicleMotor>();
	    var speedToDisplay = (int)Mathf.Abs(vehicleMotor.SignedSpeed * SpeedDisplayCoeff);
        SpeedText.GetComponent<Text>().text = speedToDisplay.ToString();

	    if (vehicleMotor.SignedSpeed >= 0)
	    {
            SpeedBar.GetComponent<Slider>().value = Mathf.Clamp(vehicleMotor.SignedSpeed, 0, vehicleMotor.ForwardSpeedMax);
            SpeedBar.GetComponent<Slider>().maxValue = vehicleMotor.ForwardSpeedMax;
	        SpeedBar.GetComponent<Slider>()
	            .GetComponentsInChildren<Image>().First(image => image.type == Image.Type.Filled).color = Color.green;
	    }
	    else
        {
            SpeedBar.GetComponent<Slider>().value = Mathf.Clamp(-vehicleMotor.SignedSpeed, 0, vehicleMotor.BackwardSpeedMax);
            SpeedBar.GetComponent<Slider>().maxValue = Vehicle.GetComponent<VehicleMotor>().BackwardSpeedMax;
            SpeedBar.GetComponent<Slider>()
                .GetComponentsInChildren<Image>().First(image => image.type == Image.Type.Filled).color = Color.red;
        }

        NitroBar.GetComponent<Slider>().value = 0.75f;
	}
}
