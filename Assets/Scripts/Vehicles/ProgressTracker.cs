using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField]
    private float _lookAheadForTargetOffset = 5;
    [SerializeField]
    private float _lookAheadForTargetFactor = 1;
    [SerializeField]
    private float _lookAheadForSpeedOffset = 5;
    [SerializeField]
    private float _lookAheadForSpeedFactor = 2;

    private Vector3 _lastPosition;
    private float _speed;
    private VehicleMotor _vehicle;

    public Track.RoutePoint ProgressPoint { get; private set; }
    public Transform Target { get; private set; }
    public float ProgressDistance { get; private set; }

    public Vector3 RoutePosition
    {
        get { return Track.Instance.GetRoutePosition(ProgressDistance); }
    }

    public Quaternion RouteRotation
    {
        get
        {
            return Quaternion.LookRotation(Track.Instance.GetRoutePosition(ProgressDistance + 0.5f) - RoutePosition);
        }
    }

    // reset the object to sensible values
    public void Reset()
    {
        ProgressDistance = 0;
    }

    // setup script properties
    private void Start()
    {
        // we use a transform to represent the point to aim for, and the point which
        // is considered for upcoming changes-of-speed. This allows this component
        // to communicate this information to the AI without requiring further dependencies.

        // You can manually create a transform and assign it to this component *and* the AI,
        // then this component will update it, and the AI can read it.

        _vehicle = GetComponentInChildren<VehicleMotor>();
        _lastPosition = _vehicle.transform.position;

        if (Target == null)
        {
            Target = new GameObject(name + " Waypoint Target").transform;
        }

        Reset();
    }

    private void Update()
    {
        // Find the position to aim
        if (Time.deltaTime > 0)
            _speed = Mathf.Lerp(_speed, (_lastPosition - _vehicle.transform.position).magnitude / Time.deltaTime,
                Time.deltaTime);

        Target.position =
            Track.Instance.GetRoutePoint(ProgressDistance + _lookAheadForTargetOffset +
                                         _lookAheadForTargetFactor * _speed)
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