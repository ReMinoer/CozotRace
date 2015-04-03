using UnityEngine;
using System.Collections;

public class VehicleMotor : MonoBehaviour {
	public float ForwardAccelerationForce=2.5f;
	public float BackwardAccelerationForce=2.5f;
	public float ForwardSpeedLimit = 25f;
	public float BackwardSpeedLimit = 10f;
	public float BrakeForce=3f;
	public bool GoForward = false;
	public float MaxRotationAngle = 100f;
	public float Speed = 0f;
	public float currentTime;
	public DrivingState formerState = new DrivingState();

	void Start () {
		currentTime = Time.time;
	}


	public void ChangeState(DrivingState state) {
			currentTime = Time.time;
			Speed = SpeedCompute();
			if (Speed < 1 && Speed > 0) {
				if(state.Forward > 0) {
					if(!GoForward) { 
						Stop();
					}
					else {
						ForwardAcceleration();
					}
				}
				if(state.Backward>0) {
					if(GoForward) {
						Stop();
					}
					else {
						BackwardAcceleration();
					}
				}
			}
			if(Speed>=1) {
				if(state.Forward>0) {
					if(!GoForward) {
						Brake (-1);
					}
					else {
						ForwardAcceleration();
					}
				}
				if(state.Backward>0) {
					if(GoForward) {
						Brake (1);
					}
					else {
						BackwardAcceleration();
					}
				}
			}
			if(Speed==0) {
				if(state.Forward>0) {
					GoForward=true;
					ForwardAcceleration();
				}
				if(state.Backward>0) {
					GoForward=false;
					BackwardAcceleration();
				}
			}

			Turn (state.Turn);
		formerState=state;
	}

	public float SpeedCompute() {
		return GetComponent<Rigidbody>().velocity.magnitude;
	}

	public void Stop() {
		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 0;
	}

	public void ForwardAcceleration() {
		GetComponent<Rigidbody>().AddForce(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
		if(GetComponent<Rigidbody>().velocity.magnitude > ForwardSpeedLimit)
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*ForwardSpeedLimit;
	}

	public void Brake(int upOrDown) {
		GetComponent<Rigidbody>().AddForce(upOrDown*10*BrakeForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
	}

	public void BackwardAcceleration() {
		GetComponent<Rigidbody>().AddForce(10*BackwardAccelerationForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
		if(GetComponent<Rigidbody>().velocity.magnitude > BackwardSpeedLimit)
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized*BackwardSpeedLimit;
	}

	public void Turn(float rotationCoef) {
		Vector3 angle = Vector3.up*MaxRotationAngle*rotationCoef*Time.deltaTime;
		transform.Rotate (angle);
	}
}
