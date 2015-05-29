using System;
using DesignPattern;
using UnityEngine;

public class VehicleMotor : Factory<VehicleMotor>
{
    public float BackwardForce = 25f;
    public float BackwardSpeedMax = 10f;
    public float BrakeForce = 30f;
    public Transform Center;
    public float FloatingHeight = 1.5f;
    public float ForwardForce = 25f;
    public float ForwardSpeedMax = 25f;
    public float ForwardSpeedMaxActual = 25f;
    public float HoverForce = 65f;
    public bool IsBoosted;
    public float MaxDiffHeight = 1.0f;
    public float Nitro = 2500;
    public float NitroDecreaseSpeed = 1;
    public Transform Nose;
    public float SpeedEvolution = 0.2f;
    public Transform Tail;
    public float TurningAngle = 50f;
    private float _speedCoeff = 1f;
    private VehicleState _state;
    private const float StopThreshold = 0.1f;

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

    public void FixedUpdate()
    {
        var rayNose = new Ray();
        var rayTail = new Ray();
        var rayCenter = new Ray();

        if (Nose != null)
            rayNose = new Ray(Nose.position, -Nose.up);
        if (Tail != null)
            rayTail = new Ray(Tail.position, -Tail.up);
        if (Center != null)
            rayCenter = new Ray(Center.position, -Center.up);

        RaycastHit hitNose, hitTail, hitCenter;
        float proportionalHeight;
        Vector3 appliedForce = Vector3.zero;
        if (Physics.Raycast(rayCenter, out hitCenter, FloatingHeight))
        {
            proportionalHeight = (FloatingHeight - hitCenter.distance) / FloatingHeight;
            appliedForce = Vector3.up * proportionalHeight * HoverForce;
        }


        if (Physics.Raycast(rayNose, out hitNose, FloatingHeight, LayerMask.GetMask("Map")))
        {
            if (Physics.Raycast(rayTail, out hitTail, FloatingHeight, LayerMask.GetMask("Map")))
            {
                if (hitNose.distance + MaxDiffHeight >= hitTail.distance)
                {
                    GetComponent<Rigidbody>().AddForce(appliedForce, ForceMode.Acceleration);
                }
            }
        }

        if (ForwardSpeedMaxActual > ForwardSpeedMax)
            ForwardSpeedMaxActual -= SpeedEvolution;

        if (ForwardSpeedMaxActual < ForwardSpeedMax)
            ForwardSpeedMaxActual += SpeedEvolution;

        var terrainTexture = GetComponent<TerrainTexture>();
        if (terrainTexture != null)
        {
            int textureIndex = GetComponent<TerrainTexture>().GetMainTexture(transform.position);
            if (textureIndex != -1)
                _speedCoeff = Map.Instance.Grounds[textureIndex].SpeedCoeff;
        }
        else
            _speedCoeff = 1;

        if (IsBoosted)
        {
            ForwardSpeedMaxActual = 2 * ForwardSpeedMax;
            Nitro -= Time.deltaTime * NitroDecreaseSpeed;
            if (Nitro < 0) Nitro = 0;
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

        IsBoosted = state.Boost && Nitro > 0;

        Turn(state.Turn);

        if (state.Position.HasValue)
            transform.position = state.Position.Value;
        if (state.Rotation.HasValue)
            transform.rotation = state.Rotation.Value;

        state.Position = transform.position;
        state.Rotation = transform.rotation;
        state.Time = (float)GameManager.Instance.Chronometer.TotalSeconds;

        if (StateChanged != null)
            StateChanged.Invoke(this, new StateChangedEventArgs {State = state});
    }

    public void GoForward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, coeff * ForwardForce * _speedCoeff);
    }

    public void GoBackward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -coeff * BackwardForce * _speedCoeff);
    }

    public void Brake(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");
        GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, coeff * BrakeForce);
    }

    public void Turn(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

        GetComponent<Rigidbody>().AddRelativeTorque(0f, coeff * TurningAngle, 0f);
    }

    /*
    public void GetWellOriented(Vector3 face)
    {
        var rayFr = new Ray(transform.position, -transform.up);
        var rayFl = new Ray(transform.position, -transform.up);
        var rayBr = new Ray(transform.position, -transform.up);
        var rayBl = new Ray(transform.position, -transform.up);

        Vector3 upDir;
        Transform frontRightTransform = null;
        Transform frontLeftTransform = null;
        Transform backRightTransform = null;
        Transform backLeftTransform = null;
        Transform childTransform = transform.FindChild("car02");
        if (childTransform != null)
        {
            frontRightTransform = childTransform.gameObject.transform.FindChild("FrontRight");
            frontLeftTransform = childTransform.gameObject.transform.FindChild("FrontLeft");
            backRightTransform = childTransform.gameObject.transform.FindChild("BackRight");
            backLeftTransform = childTransform.gameObject.transform.FindChild("BackLeft");
        }
        if (frontRightTransform != null)
            rayFr = new Ray(frontRightTransform.position, -Vector3.up);
        if (frontLeftTransform != null)
            rayFl = new Ray(frontLeftTransform.position, -Vector3.up);
        if (backRightTransform != null)
            rayBr = new Ray(backRightTransform.position, -Vector3.up);
        if (backLeftTransform != null)
            rayBl = new Ray(backLeftTransform.position, -Vector3.up);
        RaycastHit hitFr, hitFl, hitBr, hitBl;
        Physics.Raycast(rayFr, out hitFr);
        Physics.Raycast(rayFl, out hitFl);
        Physics.Raycast(rayBr, out hitBr);
        Physics.Raycast(rayBl, out hitBl);
        upDir = ((hitBr.normal +
                  hitBl.normal +
                  hitFl.normal +
                  hitFr.normal
            ) / 4).normalized;
        Debug.Log(upDir);
        transform.rotation = Quaternion.FromToRotation(transform.up, upDir);
    }
     */

    public enum VehicleState
    {
        Stopped,
        Forward,
        Backward
    }

    public class StateChangedEventArgs : EventArgs
    {
        public DrivingState State { get; set; }
    }
}