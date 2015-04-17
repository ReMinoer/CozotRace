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

	public float FloatingHeight = 1.5f;
	public float HoverForce = 65f;
	public float ForwardForce = 25f;
    public float BackwardForce = 25f;
    public float BrakeForce = 30f;
	public float ForwardSpeedMax = 25f;
	public float BackwardSpeedMax = 10f;
	public float TurningAngle = 50f;


    private const float StopThreshold = 0.1f;
	private float SpeedCoeff = 1f;

	public event EventHandler<StateChangedEventArgs> StateChanged;

	public class StateChangedEventArgs : EventArgs {
		public DrivingState State { get; set; }

	}

    public void FixedUpdate()
    {
        // Lock floating height and rotation on X & Z axis
       // transform.position = new Vector3(transform.position.x, FloatingHeight, transform.position.z);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		Ray rayN = new Ray (transform.position, -transform.up);
		Ray rayT = new Ray (transform.position, -transform.up);

		Transform noseTransform = null;
		Transform tailTransform = null;
		Transform childTransform = transform.FindChild ("car02");

		if (childTransform != null) {
			noseTransform = childTransform.gameObject.transform.FindChild ("Nose");
			tailTransform = childTransform.gameObject.transform.FindChild ("Tail");
		}

		if(noseTransform != null)
			rayN = new Ray (noseTransform.position, -noseTransform.up);
		if (tailTransform != null)
			rayT = new Ray (tailTransform.position, -tailTransform.up);
		
		RaycastHit hitN, hitT;

		if (Physics.Raycast (rayN, out hitN, FloatingHeight)) {
			float proportionalHeight = (FloatingHeight - hitN.distance) / FloatingHeight ;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
			GetComponent<Rigidbody>().AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
		if (Physics.Raycast (rayT, out hitT, FloatingHeight)) {
						float proportionalHeight = (FloatingHeight - hitT.distance) / FloatingHeight;
						Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
						GetComponent<Rigidbody> ().AddForce (appliedHoverForce, ForceMode.Acceleration);
				}

		TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
		if (terrainTexture != null)
			SpeedCoeff = Map.Instance.Grounds [GetComponent<TerrainTexture>().GetMainTexture (transform.position)].SpeedCoeff;
		else
			SpeedCoeff = 1;
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

		TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
		if (terrainTexture != null)
			SpeedCoeff = Map.Instance.Grounds [terrainTexture.GetMainTexture (transform.position)].SpeedCoeff;
		else
			SpeedCoeff = 1;

        VehicleState currentState;
	    do
	    {
            currentState = _state;

	        switch (_state)
	        {
                case VehicleState.Stopped:
                    if (state.Forward > 0)
                        _state = VehicleState.Forward;
                    else if (state.Backward > 0)
                        _state = VehicleState.Backward;
                    break;

                case VehicleState.Forward:
                    GoForward(state.Forward);
                    Brake(-state.Backward);
                    break;

                case VehicleState.Backward:
                    GoBackward(state.Backward);
                    Brake(state.Forward);
                    break;
            }

        } while (_state != currentState);

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
		GetComponent<Rigidbody> ().AddRelativeForce (0f,0f,coeff * ForwardForce);
		if(GetComponent<Rigidbody>().velocity.magnitude > ForwardSpeedMax)
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * ForwardSpeedMax * SpeedCoeff;
	}

    public void GoBackward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;
		GetComponent<Rigidbody>().AddRelativeForce(0f,0f,-coeff*BackwardForce);
        if (GetComponent<Rigidbody>().velocity.magnitude > BackwardSpeedMax)
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * BackwardSpeedMax * SpeedCoeff;
    }

	public void Brake(float coeff)
	{
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");
		GetComponent<Rigidbody> ().AddRelativeForce (0f,0f,coeff*BrakeForce);
	}

	public void Turn(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");
        GetComponent<Rigidbody>().AddRelativeTorque(0f,coeff*TurningAngle,0f);
	}
}
