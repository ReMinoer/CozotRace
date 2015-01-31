using UnityEngine;
using System.Collections;

public class IAMotor : MonoBehaviour
{
	public float Speed = 5;

	private GameObject _lastPoint;
	private GameObject _nextPoint;

	void Start ()
	{
		_lastPoint = Track.Instance.StartPoint;
		_nextPoint = _lastPoint.GetComponent<TrackPoint>().NextPoint;
	}

	void Update ()
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
