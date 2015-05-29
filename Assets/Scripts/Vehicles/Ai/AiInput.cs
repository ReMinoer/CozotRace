using UnityEngine;

public class AiInput : MonoBehaviour
{
    public bool DebugTrajectory = false;
    [SerializeField] [Range(0, 1)] private readonly float _accelWanderAmount = 0.1f;
    // how much the cars acceleration will wander

    [SerializeField] private readonly float _accelWanderSpeed = 0.1f;
    // how fast the cars acceleration wandering will fluctuate

    [SerializeField] private readonly float _cautiousAngularVelocityFactor = 30f;
    // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)

    [SerializeField] [Range(0, 180)] private readonly float _cautiousMaxAngle = 90f;
    // angle of approaching corner to treat as warranting maximum caution

    [SerializeField] [Range(0, 1)] private readonly float _cautiousSpeedFactor = 0.8f;
    // percentage of max speed to use when being maximally cautious

    [SerializeField] private readonly float _lateralWanderDistance = 3f;
    // how far the car will wander laterally towards its target

    [SerializeField] private readonly float _minimalTurn = 0.2f;
    [SerializeField] private readonly float _reactionAngle = 30f;
    private float _avoidOtherCarSlowdown; // how much to slow down due to colliding with another car, whilst avoiding
    private float _avoidOtherCarTime; // time until which to avoid the car we recently collided with
    private float _avoidPathOffset; // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
    private Vector3 _lastPosition;
    private ProgressTracker _progressTracker;
    private float _randomPerlin;
    // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)

    private Rigidbody _rigidbody;
    private VehicleMotor _vehicle;

    private void Awake()
    {
        _vehicle = GetComponent<VehicleMotor>();
        _progressTracker = GetComponent<ProgressTracker>();
        _rigidbody = GetComponent<Rigidbody>();

        _randomPerlin = Random.value * 100;
    }

    private void FixedUpdate()
    {
        Vector3 fwd = _vehicle.transform.forward;
        if (_rigidbody.velocity.magnitude > _vehicle.ForwardSpeedMax * 0.1f)
        {
            fwd = _rigidbody.velocity;
        }

        // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

        if (_progressTracker.Target == null)
            return;

        // check out the angle of our target compared to the current direction of the car
        float approachingCornerAngle = Vector3.Angle(_progressTracker.Target.position - _vehicle.transform.position, fwd);

        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
        float spinningAngle = _rigidbody.angularVelocity.magnitude * _cautiousAngularVelocityFactor;

        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
        float cautiousnessRequired = Mathf.InverseLerp(0, _cautiousMaxAngle,
            Mathf.Max(spinningAngle,
                approachingCornerAngle));

        float desiredSpeed = Mathf.Lerp(_vehicle.ForwardSpeedMax, _vehicle.ForwardSpeedMax * _cautiousSpeedFactor,
            cautiousnessRequired);

        Vector3 offsetTargetPos = _progressTracker.Target.position;

        // if are we currently taking evasive action to prevent being stuck against another car:
        if (Time.time < _avoidOtherCarTime)
        {
            // slow down if necessary (if we were behind the other car when collision occured)
            desiredSpeed *= _avoidOtherCarSlowdown;

            // and veer towards the side of our path-to-target that is away from the other car
            offsetTargetPos += _progressTracker.transform.right * _avoidPathOffset;
        }
        else
        {
            // no need for evasive action, we can just wander across the path-to-target in a random way,
            // which can help prevent AI from seeming too uniform and robotic in their driving
            offsetTargetPos += _progressTracker.transform.right *
                               (Mathf.PerlinNoise(Time.time * _lateralWanderDistance, _randomPerlin) * 2 - 1) *
                               _lateralWanderDistance;
        }

        bool accelerate = desiredSpeed > _vehicle.SignedSpeed;

        // add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving
        // i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
        float accelerateAmount = (1 - _accelWanderAmount) +
                                 (Mathf.PerlinNoise(Time.time * _accelWanderSpeed, _randomPerlin) * _accelWanderAmount);

        Vector2 forward2D = _vehicle.transform.forward.ToXz();
        Vector2 position2D = _vehicle.transform.position.ToXz();
        Vector2 destination2D = offsetTargetPos.ToXz();
        float angleTurn = Vector3.Angle(forward2D.ToX0Y(), (destination2D - position2D).normalized.ToX0Y());
        Vector3 crossTurn = Vector3.Cross(forward2D.ToX0Y(), (destination2D - position2D).normalized.ToX0Y());
        float turn = Mathf.Lerp(0, (crossTurn.y > 0 ? 1 : -1), Mathf.Clamp01(angleTurn / _reactionAngle));

        if (turn > -_minimalTurn && turn < _minimalTurn)
            turn = 0;

        var state = new DrivingState
        {
            Forward = accelerate ? accelerateAmount : 0,
            Backward = 0, //accelerate ? 0 : accelerateAmount,
            Turn = turn
        };

        _vehicle.ChangeState(state);

        // Draw debug trajectory
        if (DebugTrajectory)
            Debug.DrawLine(_lastPosition, _vehicle.transform.position, accelerate ? Color.green : Color.red, 5);
        _lastPosition = _vehicle.transform.position;
    }

    private void OnCollisionStay(Collision col)
    {
        // detect collision against other cars, so that we can take evasive action
        if (col.rigidbody != null)
        {
            var otherVehicle = col.rigidbody.GetComponent<VehicleMotor>();
            if (otherVehicle != null)
            {
                // we'll take evasive action for 1 second
                _avoidOtherCarTime = Time.time + 1;

                // but who's in front?...
                if (Vector3.Angle(transform.forward, otherVehicle.transform.position - transform.position) < 90)
                {
                    // the other ai is in front, so it is only good manners that we ought to brake...
                    _avoidOtherCarSlowdown = 0.5f;
                }
                else
                {
                    // we're in front! ain't slowing down for anybody...
                    _avoidOtherCarSlowdown = 1;
                }

                // both cars should take evasive action by driving along an offset from the path centre,
                // away from the other car
                Vector3 otherCarLocalDelta = transform.InverseTransformPoint(otherVehicle.transform.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                _avoidPathOffset = _lateralWanderDistance * -Mathf.Sign(otherCarAngle);
            }
        }
    }
}