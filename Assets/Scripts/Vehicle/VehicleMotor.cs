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
	public float ForwardSpeedMaxActual = 25f;
	public float SpeedEvolution = 0.2f;
	public float BackwardSpeedMax = 10f;
	public float TurningAngle = 50f;

	public bool isBoosted = false;

    private const float StopThreshold = 0.1f;
	private float SpeedCoeff = 1f;

    public float SignedSpeed
    {
        get
        {
            Vector3 velocity2D = GetComponent<Rigidbody>().velocity;
            velocity2D.y = 0;
            float speed = velocity2D.magnitude;
            return _state == VehicleState.Backward ? -speed : speed;
        }
    }

	public event EventHandler<StateChangedEventArgs> StateChanged;

	public class StateChangedEventArgs : EventArgs
    {
		public DrivingState State { get; set; }
	}

    public void FixedUpdate()
    {
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

		if (Physics.Raycast (rayN, out hitN, FloatingHeight))
        {
			float proportionalHeight = (FloatingHeight - hitN.distance) / FloatingHeight ;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
			GetComponent<Rigidbody>().AddForce(appliedHoverForce, ForceMode.Acceleration);
		}

		if (Physics.Raycast (rayT, out hitT, FloatingHeight))
        {
			float proportionalHeight = (FloatingHeight - hitT.distance) / FloatingHeight;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
			GetComponent<Rigidbody> ().AddForce (appliedHoverForce, ForceMode.Acceleration);
		}
		
		if (ForwardSpeedMaxActual > ForwardSpeedMax)
			ForwardSpeedMaxActual -= SpeedEvolution;

		if (ForwardSpeedMaxActual < ForwardSpeedMax)
			ForwardSpeedMaxActual += SpeedEvolution;

		TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
		if (terrainTexture != null)
			SpeedCoeff = Map.Instance.Grounds [GetComponent<TerrainTexture>().GetMainTexture (transform.position)].SpeedCoeff;
		else
			SpeedCoeff = 1;

        if (_state == VehicleState.Forward && GetComponent<Rigidbody>().velocity.magnitude > ForwardSpeedMaxActual)
			if (!isBoosted)
				GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * ForwardSpeedMaxActual;

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

		/*TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
		if (terrainTexture != null)
			SpeedCoeff = Map.Instance.Grounds [terrainTexture.GetMainTexture (transform.position)].SpeedCoeff;
		else
			SpeedCoeff = 1;*/

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

		GetComponent<Rigidbody> ().AddRelativeForce (0f,0f,coeff * ForwardForce * SpeedCoeff);
		Vector3 face = transform.forward;
		//GetWellOriented(face);
	}

    public void GoBackward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

		GetComponent<Rigidbody>().AddRelativeForce(0f,0f,-coeff*BackwardForce * SpeedCoeff);
		Vector3 face = transform.forward;
		//GetWellOriented(face);
    }

	public void Brake(float coeff)
	{
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");
		GetComponent<Rigidbody> ().AddRelativeForce (0f,0f,coeff*BrakeForce);
		Vector3 face = transform.forward;
	//	GetWellOriented(face);
	}

	public void Turn(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

        GetComponent<Rigidbody>().AddRelativeTorque(0f,coeff*TurningAngle,0f);
		Vector3 face = transform.forward;
	//	GetWellOriented(face);
	}

	public void GetWellOriented(Vector3 face) {
		Ray rayFR = new Ray (transform.position, -transform.up);
		Ray rayFL = new Ray (transform.position, -transform.up);
		Ray rayBR = new Ray (transform.position, -transform.up);
		Ray rayBL = new Ray (transform.position, -transform.up);
		
		Vector3 upDir;
		Transform frontRightTransform = null;
		Transform frontLeftTransform = null;
		Transform backRightTransform = null;
		Transform backLeftTransform = null;
		Transform childTransform = transform.FindChild ("car02");
		if (childTransform != null) {
			frontRightTransform = childTransform.gameObject.transform.FindChild ("FrontRight");
			frontLeftTransform = childTransform.gameObject.transform.FindChild ("FrontLeft");
			backRightTransform = childTransform.gameObject.transform.FindChild ("BackRight");
			backLeftTransform = childTransform.gameObject.transform.FindChild ("BackLeft");
		}
		if (frontRightTransform != null)
			rayFR = new Ray (frontRightTransform.position, -Vector3.up);
		if (frontLeftTransform != null)
			rayFL = new Ray (frontLeftTransform.position, -Vector3.up);
		if (backRightTransform != null)
			rayBR = new Ray (backRightTransform.position, -Vector3.up);
		if (backLeftTransform != null)
			rayBL = new Ray (backLeftTransform.position, -Vector3.up);
		RaycastHit hitFR, hitFL, hitBR, hitBL;
		Physics.Raycast (rayFR, out hitFR);
		Physics.Raycast (rayFL, out hitFL);
		Physics.Raycast (rayBR, out hitBR);
		Physics.Raycast (rayBL, out hitBL);
		upDir = ((hitBR.normal +
		         hitBL.normal +
		         hitFL.normal +
		         hitFR.normal
		         )/4).normalized;
		Debug.Log (upDir);
		transform.rotation = Quaternion.FromToRotation (transform.up, upDir);
}
}
