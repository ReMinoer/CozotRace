using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField]
    private float lookAheadForTargetOffset = 5;
    // The offset ahead along the route that the we will aim for

    [SerializeField]
    private float lookAheadForTargetFactor = .1f;
    // A multiplier adding distance ahead along the route to aim for, based on current speed

    [SerializeField]
    private float _lookAheadForSpeedOffset = 10;
    // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)

    [SerializeField]
    private float _lookAheadForSpeedFactor = .2f;
    // A multiplier adding distance ahead along the route for speed adjustments

    // these are public, readable by other objects - i.e. for an AI to know where to head!
    //public Track.RoutePoint targetPoint { get; private set; }
    //public Track.RoutePoint speedPoint { get; private set; }
    public Track.RoutePoint ProgressPoint { get; private set; }
    public Transform Target { get; private set; }
    public float ProgressDistance { get; private set; }

    private Vector3 _lastPosition;
    private float _speed;

    private VehicleMotor _vehicle;

    // setup script properties
    private void Start()
    {
        // we use a transform to represent the point to aim for, and the point which
        // is considered for upcoming changes-of-speed. This allows this component
        // to communicate this information to the AI without requiring further dependencies.

        // You can manually create a transform and assign it to this component *and* the AI,
        // then this component will update it, and the AI can read it.

        _vehicle = GetComponentInChildren<VehicleMotor>();

        if (Target == null)
        {
            Target = new GameObject(name + " Waypoint Target").transform;
        }

        Reset();
    }

    // reset the object to sensible values
    public void Reset()
    {
        ProgressDistance = 0;
    }

    void Update()
    {
        // Find the position to aim
        if (Time.deltaTime > 0)
            _speed = Mathf.Lerp(_speed, (_lastPosition - _vehicle.transform.position).magnitude / Time.deltaTime,
                               Time.deltaTime);

        Target.position =
                    Track.Instance.GetRoutePoint(ProgressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor * _speed)
                           .Position;
        if (Track.Instance.GetPointsCount() != 0)
        {
            Target.rotation =
                Quaternion.LookRotation(
                    Track.Instance.GetRoutePoint(ProgressDistance + _lookAheadForSpeedOffset +
                                                 _lookAheadForSpeedFactor * _speed)
                        .Direction);
        }

        // Find the current progress
        ProgressPoint = Track.Instance.GetRoutePoint(ProgressDistance);
        Vector3 progressDelta = ProgressPoint.Position - _vehicle.transform.position;
        if (Vector3.Dot(progressDelta, ProgressPoint.Direction) < 0)
        {
            ProgressDistance += progressDelta.magnitude * 0.5f;
        }

        _lastPosition = _vehicle.transform.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_vehicle.transform.position, Target.position);
            Gizmos.DrawLine(Target.position, Target.position + Target.forward);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Track.Instance.GetRoutePosition(ProgressDistance), 1);
        }
    }
}
