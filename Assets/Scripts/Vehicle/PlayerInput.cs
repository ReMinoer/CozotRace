using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		DrivingState drivingState = new DrivingState ();
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		drivingState.Forward=Mathf.Clamp(verticalInput, 0, 1);
		drivingState.Backward=-Mathf.Clamp(verticalInput, -1, 0);
		drivingState.Turn = horizontalInput;

		GetComponent<VehicleMotor> ().ChangeState (drivingState);
	}
}
