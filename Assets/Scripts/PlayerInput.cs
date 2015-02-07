using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	public float Speed = 0f;
	public bool GoForward = false;
	private DrivingState drivingState;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Speed = GetComponent<VehicleMotor>().SpeedCompute();
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		if (Speed < 1 && Speed > 0) {
			if(verticalInput > 0) {
				if(!GoForward) { 
					GetComponent<VehicleMotor>().Stop();
				}
				else {
					//GetComponent<VehicleMotor>().ForwardAcceleration();
					drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
				}
			}
			if(verticalInput<0) {
				if(GoForward) {
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
				if(!GoForward) {
					GetComponent<VehicleMotor>().Brake (-1);
				}
				else {
					//GetComponent<VehicleMotor>().ForwardAcceleration();
					drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
				}
			}
			if(verticalInput<0) {
				if(GoForward) {
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
				GoForward=true;
				//GetComponent<VehicleMotor>().ForwardAcceleration();
				drivingState.Forward=Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
			}
			if(verticalInput<0) {
				GoForward=false;
				//GetComponent<VehicleMotor>().BackwardAcceleration();
				drivingState.Backward=-Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
			}
		}
		
		if (horizontalInput < 0) {
			GetComponent<VehicleMotor> ().LeftRotation ();
		}
		if (horizontalInput > 0) {
			GetComponent<VehicleMotor> ().RightRotation ();
		}
	}
}
