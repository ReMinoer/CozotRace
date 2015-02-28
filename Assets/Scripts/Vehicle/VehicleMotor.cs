using System;
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

    public void Update()
    {
        // Lock floating height and rotation on X & Z axis
        transform.position = new Vector3(transform.position.x, FloatingHeight, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

	public void ChangeState(DrivingState state)
    {
        // Stop the vehicle if its speed near to zero
        if (state.Forward < float.Epsilon && state.Backward < float.Epsilon
            && rigidbody.velocity.magnitude < StopThreshold)
        {
            rigidbody.velocity = Vector3.zero;
            _state = VehicleState.Stopped;
        }

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
    }

    public void GoForward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        Vector3 force = coeff * ForwardForce * transform.forward * Time.deltaTime;
        rigidbody.AddForce(force, ForceMode.Impulse);

		if(rigidbody.velocity.magnitude > ForwardSpeedMax)
			rigidbody.velocity = rigidbody.velocity.normalized * ForwardSpeedMax;
	}

    public void GoBackward(float coeff)
    {
        if (coeff < 0)
            throw new ArgumentException("coeff must be superior or egal to 0 !");
        if (coeff < float.Epsilon)
            return;

        Vector3 force = -BackwardForce * transform.forward * Time.deltaTime;
        rigidbody.AddForce(force, ForceMode.Impulse);

        if (rigidbody.velocity.magnitude > BackwardSpeedMax)
            rigidbody.velocity = rigidbody.velocity.normalized * BackwardSpeedMax;
    }

	public void Brake(float coeff)
	{
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

	    Vector3 force = coeff * BrakeForce * transform.forward * Time.deltaTime;
        rigidbody.AddForce(force, ForceMode.Impulse);
	}

	public void Turn(float coeff)
    {
        if (coeff < -1 || coeff > 1)
            throw new ArgumentException("coeff must be between -1 and 1 !");

        Vector3 angle = coeff * TurningAngle * Vector3.up * Time.deltaTime;
        rigidbody.AddTorque(angle);
        //transform.Rotate(angle);
	}
}
