using UnityEngine;
using System.Collections;

public class IAMotor : MonoBehaviour
{
	public float Speed = 2;
	public float TurnSpeed = 2;

	private GameObject _lastPoint;
	private GameObject _nextPoint;

	void Start ()
	{
		_lastPoint = Track.Instance.StartPoint;
		_nextPoint = _lastPoint.GetComponent<TrackPoint>().NextPoint;
	}

	void Update ()
	{
		//SimpleFollow();
		SmoothTurnFollow();
	}

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
	}
}
