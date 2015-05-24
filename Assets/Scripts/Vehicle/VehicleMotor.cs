using System;
using DesignPattern;
using UnityEngine;

public class VehicleMotor : Factory<VehicleMotor>
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
	public float MaxDiffHeight = 1.0f;
	public float TurningAngle = 50f;
	public bool isBoosted = false;
	public float nitro = 2500;
	public float nitroDecreaseSpeed = 1;

	public Transform Nose;
	public Transform Center;
	public Transform Tail;

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

		Ray rayNose = new Ray();
		Ray rayTail = new Ray();
		Ray rayCenter = new Ray();

		if(Nose != null)
			rayNose = new Ray (Nose.position, -Nose.up);
		if (Tail != null)
			rayTail = new Ray (Tail.position, -Tail.up);
		if (Center != null)
			rayCenter = new Ray (Center.position, -Center.up);
		
		RaycastHit hitNose, hitTail, hitCenter;
		float proportionalHeight;
		Vector3 appliedForce = Vector3.zero;
		if(Physics.Raycast(rayCenter,out hitCenter, FloatingHeight)) {
			proportionalHeight = (FloatingHeight - hitCenter.distance) / FloatingHeight ;
			appliedForce = Vector3.up * proportionalHeight * HoverForce;
		}


		if (Physics.Raycast(rayNose, out hitNose, FloatingHeight, LayerMask.GetMask("Map"))) {
            if (Physics.Raycast(rayTail, out hitTail, FloatingHeight, LayerMask.GetMask("Map")))
            {
				// Debug.Log("Diff : "+(Mathf.Abs(hitNose.distance-hitTail.distance))+"\n");
				if(hitNose.distance + MaxDiffHeight >= hitTail.distance) {
					GetComponent<Rigidbody>().AddForce(appliedForce, ForceMode.Acceleration);
				}
				/*else {
					float proportionalHeight = (FloatingHeight - hitTail.distance) / FloatingHeight;
					Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
					GetComponent<Rigidbody> ().AddForce (appliedHoverForce, ForceMode.Acceleration);
				}*/
			}
		}
		
		if (ForwardSpeedMaxActual > ForwardSpeedMax)
			ForwardSpeedMaxActual -= SpeedEvolution;

		if (ForwardSpeedMaxActual < ForwardSpeedMax)
			ForwardSpeedMaxActual += SpeedEvolution;

		/*if (Physics.Raycast (rayTail, out hitTail, FloatingHeight)) {
			if (Physics.Raycast (rayNose, out hitNose, FloatingHeight)) {
				if(hitTail.distance-hitNose.distance < MaxDiffHeight) {
					float proportionalHeight = (FloatingHeight - hitTail.distance) / FloatingHeight ;
					Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
					GetComponent<Rigidbody>().AddForce(appliedHoverForce, ForceMode.Acceleration);
				}
				else {
					float proportionalHeight = (FloatingHeight - hitNose.distance) / FloatingHeight;
					Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
					GetComponent<Rigidbody> ().AddForce (appliedHoverForce, ForceMode.Acceleration);
				}
			}
		}*/

		TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
        if (terrainTexture != null)
        {
            int textureIndex = GetComponent<TerrainTexture>().GetMainTexture(transform.position);
            if (textureIndex != -1)
                SpeedCoeff = Map.Instance.Grounds[textureIndex].SpeedCoeff;
        }
        else
            SpeedCoeff = 1;

		if (isBoosted) {
			ForwardSpeedMaxActual = 2 * ForwardSpeedMax;
			nitro -= Time.deltaTime * nitroDecreaseSpeed;
			if(nitro<0) nitro=0;
		}
		else
			ForwardSpeedMaxActual = ForwardSpeedMax;

        if (_state == VehicleState.Forward && GetComponent<Rigidbody>().velocity.magnitude > ForwardSpeedMaxActual)
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

		isBoosted = state.Boost && nitro>0;

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
