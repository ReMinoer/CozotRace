﻿using System;
using UnityEngine;

public class VehicleMotor : MonoBehaviour
{
    public enum VehicleState
    {
        Stopped,
        Forward,
        Backward
    }

    private VehicleState _state;

	public float ForwardForce = 25f;
    public float BackwardForce = 25f;
    public float BrakeForce = 30f;
	public float ForwardSpeedMax = 25f;
	public float BackwardSpeedMax = 10f;
	public float TurningAngle = 50f;
    public float FloatingHeight = 1;

    private const float StopThreshold = 0.1f;

	public event EventHandler<StateChangedEventArgs> StateChanged;

	public class StateChangedEventArgs : EventArgs
    {
		public DrivingState State { get; set; }
	}

    public void Update()
    {
        // Lock floating height and rotation on X & Z axis
        transform.position = new Vector3(transform.position.x, FloatingHeight, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void FixedUpdate()
    {
        if (_state == VehicleState.Forward && GetComponent<Rigidbody>().velocity.magnitude > ForwardSpeedMax)
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * ForwardSpeedMax;

        if (_state == VehicleState.Backward && GetComponent<Rigidbody>().velocity.magnitude > BackwardSpeedMax)
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * BackwardSpeedMax;
    }

	public void ChangeState(DrivingState state)
    {
        // Stop the vehicle if its speed near to zero
        if (state.Forward < float.Epsilon && state.Backward < float.Epsilon
            && GetComponent<Rigidbody>().velocity.magnitude < StopThreshold)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            _state = VehicleState.Stopped;
        }
        else if (Vector3.Dot(GetComponent<Rigidbody>().velocity.normalized, transform.forward) >= 0)
            _state = VehicleState.Forward;
        else
            _state = VehicleState.Backward;

	    switch (_state)
	    {
            case VehicleState.Forward:
                GoForward(state.Forward);
                Brake(-state.Backward);
                break;

            case VehicleState.Backward:
                GoBackward(state.Backward);
                Brake(state.Forward);
                break;
        }

		Turn (state.Turn);

		state.Time = Time.realtimeSinceStartup;

		if (StateChanged != null)
			StateChanged.Invoke (this, new StateChangedEventArgs { State = state });
    }

    public void GoForward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        Vector3 force = coeff * ForwardForce * transform.forward * Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}

    public void GoBackward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        Vector3 force = -BackwardForce * transform.forward * Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

	public void Brake(float coeff)
	{
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

	    Vector3 force = coeff * BrakeForce * transform.forward * Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}

	public void Turn(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

        Vector3 angle = coeff * TurningAngle * Vector3.up * Time.deltaTime;
        GetComponent<Rigidbody>().AddTorque(angle);
	}
}
