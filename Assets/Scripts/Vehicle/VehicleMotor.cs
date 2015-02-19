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
		return rigidbody.velocity.magnitude;
	}

	public void Stop() {
		rigidbody.velocity = rigidbody.velocity * 0;
	}

	public void ForwardAcceleration() {
		rigidbody.AddForce(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
		if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
			rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
	}

	public void Brake(int upOrDown) {
		rigidbody.AddForce(upOrDown*10*BrakeForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
	}

	public void BackwardAcceleration() {
		rigidbody.AddForce(10*BackwardAccelerationForce * transform.forward * Time.deltaTime,ForceMode.Impulse);
		if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
			rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
	}

	public void Turn(float rotationCoef) {
		Vector3 angle = Vector3.up*MaxRotationAngle*rotationCoef*Time.deltaTime;
		transform.Rotate (angle);
	}
}
