using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	//public float Speed = 0f;
	private DrivingState drivingState;
	//private bool goForward;
	// Use this for initialization
	void Start () {
		drivingState = new DrivingState ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//goForward = drivingState.GoesForward;
		//Speed = GetComponent<VehicleMotor>().SpeedCompute();
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		/*if (Speed < 1 && Speed > 0) {
			if(verticalInput > 0) {
				if(!goForward) { 
					GetComponent<VehicleMotor>().Stop();
				}
				else {
					//GetComponent<VehicleMotor>().ForwardAcceleration();
					drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
				}
			}
			if(verticalInput<0) {
				if(goForward) {
					GetComponent<VehicleMotor>().Stop();
				}
				else {
					//GetComponent<VehicleMotor>().BackwardAcceleration();
					drivingState.Backward=-Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
				}
			}
		}
		if(Speed>=1) {
			if(verticalInput>0) {
				if(!goForward) {
					GetComponent<VehicleMotor>().Brake (-1);
				}
				else {
					//GetComponent<VehicleMotor>().ForwardAcceleration();
					drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
				}
			}
			if(verticalInput<0) {
				if(goForward) {
					GetComponent<VehicleMotor>().Brake (1);
				}
				else {
					//GetComponent<VehicleMotor>().BackwardAcceleration();
					drivingState.Backward=-Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
				}
			}
		}
		if(Speed==0) {
			if(verticalInput>0) {
				drivingState.GoesForward=true;
				//GetComponent<VehicleMotor>().ForwardAcceleration();
				drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
			}
			if(verticalInput<0) {
				drivingState.GoesForward=false;
				//GetComponent<VehicleMotor>().BackwardAcceleration();
				drivingState.Backward=-Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
			}
		}*/

		drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
		drivingState.Backward=-Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
		drivingState.Turn = horizontalInput;

		GetComponent<VehicleMotor> ().ChangeState (drivingState);
	}
}
