using UnityEngine;
using System.Collections;

public class VehicleMotor : MonoBehaviour {

	public void ForwardAcceleration(Vector3 forwardAccelerationForce) {
		rigidbody.AddForce(forwardAccelerationForce,ForceMode.Impulse);
	}

	public void Brake(Vector3 brakeForce) {
		rigidbody.AddForce(brakeForce,ForceMode.Impulse);
	}

	public void BackwardAcceleration(Vector3 backwardAccelerationForce) {
		rigidbody.AddForce(backwardAccelerationForce,ForceMode.Impulse);
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
