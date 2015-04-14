using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleMotor))]
[RequireComponent(typeof(ProgressTracker))]
public class AiInput : MonoBehaviour
{
    public bool DebugTrajectory = false;
    private Vector3 _lastPosition;

    private VehicleMotor _vehicle;
    private ProgressTracker _progressTracker;
    private Rigidbody _rigidbody;

    [SerializeField]
    [Range(0, 1)]
    private float _cautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
    [SerializeField]
    [Range(0, 180)]
    private float _cautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
    [SerializeField]
    private float _cautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)

    [SerializeField]
    private float _reactionAngle = 60f;
    [SerializeField]
    private float _minimalTurn = 0.2f;

    private void Awake()
    {
        _vehicle = GetComponent<VehicleMotor>();
        _progressTracker = GetComponent<ProgressTracker>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.forward;
        if (_rigidbody.velocity.magnitude > _vehicle.ForwardSpeedMax * 0.1f)
        {
            fwd = _rigidbody.velocity;
        }

        // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

        // check out the angle of our target compared to the current direction of the car
        float approachingCornerAngle = Vector3.Angle(_progressTracker.Target.position - transform.position, fwd);

        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
        float spinningAngle = _rigidbody.angularVelocity.magnitude * _cautiousAngularVelocityFactor;

        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
        float cautiousnessRequired = Mathf.InverseLerp(0, _cautiousMaxAngle,
                                                        Mathf.Max(spinningAngle,
                                                                    approachingCornerAngle));

        float desiredSpeed = Mathf.Lerp(_vehicle.ForwardSpeedMax, _vehicle.ForwardSpeedMax * _cautiousSpeedFactor, cautiousnessRequired);

        bool accelerate = desiredSpeed > _vehicle.SignedSpeed;

        Vector3 offsetTargetPos = _progressTracker.Target.position;

        Vector2 forward2D = this.transform.forward.ToXZ();
        Vector2 position2D = this.transform.position.ToXZ();
        Vector2 destination2D = offsetTargetPos.ToXZ();
        float angleTurn = Vector3.Angle(forward2D.ToX0Y(), (destination2D - position2D).normalized.ToX0Y());
        Vector3 crossTurn = Vector3.Cross(forward2D.ToX0Y(), (destination2D - position2D).normalized.ToX0Y());
        float turn = Mathf.Lerp(0, (crossTurn.y > 0 ? 1 : -1), Mathf.Clamp01(angleTurn / _reactionAngle));

        if (turn > -_minimalTurn && turn < _minimalTurn)
            turn = 0;

        var state = new DrivingState
        {
            Forward = accelerate ? 1 : 0,
            Backward = accelerate ? 0 : 1,
            Turn = turn
        };

        _vehicle.ChangeState(state);

        // Draw debug trajectory
        if (DebugTrajectory)
            Debug.DrawLine(_lastPosition, this.transform.position, accelerate ? Color.green : Color.red, 5);
        _lastPosition = this.transform.position;
    }
}

public static class VectorExtension
{
	public static Vector2 ToXZ(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	public static Vector3 ToX0Y(this Vector2 vector)
	{
		return new Vector3(vector.x, 0, vector.y);
	}
}
