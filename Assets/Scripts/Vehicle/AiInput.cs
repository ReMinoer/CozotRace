using UnityEngine;
using System.Collections;

public class AiInput : MonoBehaviour
{
	/*
	public float Speed = 2;
	public float TurnSpeed = 2;
	*/
	
	private VehicleMotor _vehicle;

	private GameObject _lastPoint;
    private GameObject _nextPoint;

    private Vector3 _lastPosition;
    //private GameObject _trajectoryCircle;

	void Start ()
	{
		_vehicle = GetComponent<VehicleMotor>();

		_lastPoint = Track.Instance.StartPoint;
        _nextPoint = _lastPoint.GetComponent<TrackPoint>().NextPoint;

        _lastPosition = this.transform.position;
        /*
        _trajectoryCircle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
	    Destroy(_trajectoryCircle.collider);
	    _trajectoryCircle.transform.parent = this.transform;
	    _trajectoryCircle.renderer.material.color = Color.red;
	    _trajectoryCircle.transform.localScale = new Vector3(1, 0.01f, 1);
         */
	}

	void Update ()
	{
		BinocleMethod();
		//SimpleFollow();
		//SmoothTurnFollow();
	}

	void BinocleMethod()
    {
        DrivingState state = new DrivingState();

		Vector2 position = this.transform.position.ToXZ();
		Vector2 start = _lastPoint.transform.position.ToXZ();
		Vector2 destination = _nextPoint.transform.position.ToXZ();
		
		// STEP 1 : Get the trajectory circle radius
		float speed = rigidbody.velocity.ToXZ().magnitude;
		float angularVelocity = rigidbody.angularVelocity.ToXZ().magnitude;
		float circleRadius = speed / angularVelocity;
		
		// STEP 2 : Get the trajectory circle center
		Vector2 previousPosition;
		if (_lastPoint.GetComponent<TrackPoint> ().PreviousPoint == null)
			previousPosition = Track.Instance.GetLastPoint().transform.position;
		else
			previousPosition = _lastPoint.GetComponent<TrackPoint>().PreviousPoint.transform.position.ToXZ();
		Vector3 cross = Vector3.Cross((start - previousPosition).normalized.ToX0Y(),
		                            (destination - start).normalized.ToX0Y());
		
		Vector2 centerDirection = Quaternion.AngleAxis(cross.y > 0 ? -90 : 90, Vector3.right) * rigidbody.velocity.ToXZ();
		Vector2 circleCenter = position + (centerDirection.normalized * circleRadius);

		// STEP 3 : Send ray, check if intersect in the circle and set vertical movement
		Plane plane = new Plane((position - destination).ToX0Y().normalized, destination.ToX0Y());
		Ray ray = new Ray(circleCenter.ToX0Y(), (destination - circleCenter).ToX0Y().normalized);

		float enter = 0;
		if (plane.Raycast(ray, out enter) && enter <= circleRadius)
			state.Forward = 1f;
		else
			state.Backward = 1f;

		// STEP 4 : Turn
	    Vector2 plateForward = this.transform.forward.ToXZ();
        Vector3 crossTurn = Vector3.Cross(plateForward.ToX0Y(), (destination - position).normalized.ToX0Y());

		state.Turn = crossTurn.y > 0 ? 1 : -1;
		
		// STEP 5 : Send state to VehicleMotor
        _vehicle.ChangeState(state);

		// STEP 6 : Check trackpoint and go to next if necessary
		position = this.transform.position.ToXZ();
		Vector2 direction = (destination - position).normalized;
		Vector2 diff = destination - _lastPoint.transform.position.ToXZ();

		if (destination == position || Vector3.Dot(direction.ToX0Y(), diff.ToX0Y()) < 0)
		{
			_lastPoint = _nextPoint;
			if (_nextPoint.GetComponent<TrackPoint>().NextPoint != null)
				_nextPoint = _nextPoint.GetComponent<TrackPoint>().NextPoint;
			else if (_lastPoint.transform.position == Track.Instance.StartPoint.transform.position)
				_nextPoint = Track.Instance.StartPoint.GetComponent<TrackPoint>().NextPoint;
			else
				_nextPoint = Track.Instance.StartPoint;
        }

        // Draw debug trajectory
        Debug.DrawLine(_lastPosition, this.transform.position, Color.blue, 2);
        _lastPosition = this.transform.position;

        // Draw debug circle
        //_trajectoryCircle.transform.position = new Vector3(circleCenter.x, this.transform.position.y, circleCenter.y);
        //_trajectoryCircle.transform.localScale = Vector3.Scale(Vector3.one, new Vector3(circleRadius, 0.01f, circleRadius));
    }

	void CheckTrack()
	{

	}

	/*
	void SmoothTurnFollow()
	{
		Vector3 destination = _nextPoint.transform.position;
		Vector3 direction = (destination - this.transform.position).normalized;
		Vector3 diff = destination - _lastPoint.transform.position;
		
		Vector3 orientation = (this.transform.right).normalized;
		
		float angle = Vector3.Angle(direction, orientation);
		Vector3 cross = Vector3.Cross(direction, orientation);
		float turn = Mathf.Min(angle, TurnSpeed);
		this.transform.Rotate(this.transform.up, cross.y > 0 ? -turn : turn);
		orientation = (this.transform.right).normalized;
		
		this.transform.position += orientation * Speed * Time.deltaTime;
		
		if (destination == this.transform.position || Vector3.Dot(direction, diff) < 0)
		{
			_lastPoint = _nextPoint;
			if (_nextPoint.GetComponent<TrackPoint>().NextPoint != null)
				_nextPoint = _nextPoint.GetComponent<TrackPoint>().NextPoint;
			else if (_lastPoint.transform.position == Track.Instance.StartPoint.transform.position)
				_nextPoint = Track.Instance.StartPoint.GetComponent<TrackPoint>().NextPoint;
			else
				_nextPoint = Track.Instance.StartPoint;
		}
	}

	void SimpleFollow()
	{
		Vector3 destination = _nextPoint.transform.position;
		Vector3 direction = (destination - this.transform.position).normalized;
		Vector3 diff = destination - _lastPoint.transform.position;
		
		this.transform.position += direction * Speed * Time.deltaTime;
		
		if (destination == this.transform.position || Vector3.Dot(direction, diff) < 0)
		{
			this.transform.position = destination;
			_lastPoint = _nextPoint;
			if (_nextPoint.GetComponent<TrackPoint>().NextPoint != null)
				_nextPoint = _nextPoint.GetComponent<TrackPoint>().NextPoint;
			else if (_lastPoint.transform.position == Track.Instance.StartPoint.transform.position)
				_nextPoint = Track.Instance.StartPoint.GetComponent<TrackPoint>().NextPoint;
			else
				_nextPoint = Track.Instance.StartPoint;
		}
	}*/
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
