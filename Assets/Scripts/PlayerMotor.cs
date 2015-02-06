using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	public float ForwardAccelerationForce=2.5f;
	public float BackwardAccelerationForce=2.5f;
	public float BrakeForce=3f;
	public float Speed = 0f;
	public float ForwardSpeedLimit = 25f;
	public float BackwardSpeedLimit = 10f;
	public bool GoForward = false;
	public bool GoBackward = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Speed = rigidbody.velocity.magnitude;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		if (Speed < 1 && Speed > 0) {
			if(verticalInput > 0) {
				if(!GoForward) { 
					rigidbody.velocity = rigidbody.velocity * 0;
				}
				else {
					GetComponent<VehicleMotor>().ForwardAcceleration(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
				}
			}
			if(verticalInput<0) {
				if(GoForward) {
					rigidbody.velocity = rigidbody.velocity * 0;
				}
				else {
					GetComponent<VehicleMotor>().BackwardAcceleration(10*BackwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
				}
			}
		}
		if(Speed>=1) {
			if(verticalInput>0) {
				if(!GoForward) {
					GetComponent<VehicleMotor>().Brake (-10*BrakeForce * transform.forward * Time.deltaTime);
				}
				else {
					GetComponent<VehicleMotor>().ForwardAcceleration(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
				}
			}
			if(verticalInput<0) {
				if(GoForward) {
					GetComponent<VehicleMotor>().Brake (10*BrakeForce * transform.forward * Time.deltaTime);
				}
				else {
					GetComponent<VehicleMotor>().BackwardAcceleration(10*BackwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
				}
			}
		}
		if(Speed==0) {
			if(verticalInput>0) {
				GoForward=true;
				GetComponent<VehicleMotor>().ForwardAcceleration(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime);
				if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
					rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
			}
			if(verticalInput<0) {
				GoForward=false;
				GetComponent<VehicleMotor>().BackwardAcceleration(10*BackwardAccelerationForce * transform.forward * Time.deltaTime);
				if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
					rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
			}
		}


		/*if (GoForward) {
			if (verticalInput > 0) {
				GoForward=true;
				GetComponent<VehicleMotor>().ForwardAcceleration(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime);
				if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
					rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
			}
			if (verticalInput < 0) {
				if (1 > rigidbody.velocity.magnitude && rigidbody.velocity.magnitude > 0) {
					GoForward=true;
					rigidbody.velocity = rigidbody.velocity*0;
					rigidbody.Sleep();
				}
				if (rigidbody.velocity.magnitude == 0) {
					GoForward=false;
					GetComponent<VehicleMotor>().BackwardAcceleration(10*BackwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
				}
				else {
					GoForward=true;
					GetComponent<VehicleMotor>().Brake (10*BrakeForce * transform.forward * Time.deltaTime);
				}
			}
		} 
		else {
			if (verticalInput > 0) {
				if (1 > rigidbody.velocity.magnitude && rigidbody.velocity.magnitude > 0) {
					GoForward=false;
					rigidbody.velocity = rigidbody.velocity*0;
					rigidbody.Sleep();
				}
				else if(rigidbody.velocity.magnitude == 0) {
					GoForward=true;
					GetComponent<VehicleMotor>().ForwardAcceleration(-10*ForwardAccelerationForce * transform.forward * Time.deltaTime);
					if(rigidbody.velocity.magnitude > ForwardSpeedLimit)
						rigidbody.velocity = rigidbody.velocity.normalized*ForwardSpeedLimit;
				}
				else {
					GoForward=false;
					GetComponent<VehicleMotor>().Brake (-10*BrakeForce * transform.forward * Time.deltaTime);
				}
			}
			if (verticalInput < 0) {
				GoForward=false;
				GetComponent<VehicleMotor>().BackwardAcceleration(10*BackwardAccelerationForce * transform.forward * Time.deltaTime);
				if(rigidbody.velocity.magnitude > BackwardSpeedLimit)
					rigidbody.velocity = rigidbody.velocity.normalized*BackwardSpeedLimit;
			}
		}*/
		if (horizontalInput < 0) {
			GetComponent<VehicleMotor> ().LeftRotation ();
		}
		if (horizontalInput > 0) {
			GetComponent<VehicleMotor> ().RightRotation ();
		}
	}
}
