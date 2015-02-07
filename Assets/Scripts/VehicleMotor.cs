using UnityEngine;
using System.Collections;

public class VehicleMotor : MonoBehaviour {
	public float ForwardAccelerationForce=2.5f;
	public float BackwardAccelerationForce=2.5f;
	public float ForwardSpeedLimit = 25f;
	public float BackwardSpeedLimit = 10f;
	public float BrakeForce=3f;

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

	public void LeftRotation() {
		Vector3 angle = Vector3.down*100*Time.deltaTime;
		transform.Rotate (angle);
		}

	public void RightRotation() {
		Vector3 angle = Vector3.up*100*Time.deltaTime;
		transform.Rotate (angle);
		}
}
