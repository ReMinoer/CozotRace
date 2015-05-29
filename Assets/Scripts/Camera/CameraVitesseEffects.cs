using DesignPattern;
using UnityEngine;

public class CameraVitesseEffects : Factory<CameraVitesseEffects>
{
    public Transform Target;
    public Rigidbody ParentRigidbody;
    public float Distance = 0.35f;
    public float DistanceMultiplier = 0.08F;
    public float DistanceSnapTime = 0.5F;
    public float RotationSnapTime = 0.5F;
    public float Height = 0.3f;
    public float HeightDamping = 10.0f;
    public float LookAtHeight = 0.02f;
    private float _currentHeight;
    private float _currentRotationAngle;
    private Vector3 _lookAtVector;
    private float _usedDistance = 1.0F;
    private float _wantedHeight;
    private Vector3 _wantedPosition;
    private float _wantedRotationAngle;
    private float _yVelocity;
    private float _zVelocity;

    private void Start()
    {
        _lookAtVector = new Vector3(0, LookAtHeight, 0);
    }

    private void LateUpdate()
    {
        _wantedHeight = Target.position.y + Height;
        _currentHeight = transform.position.y;

        _wantedRotationAngle = Target.eulerAngles.y;
        _currentRotationAngle = transform.eulerAngles.y;

        _currentRotationAngle = Mathf.SmoothDampAngle(_currentRotationAngle, _wantedRotationAngle, ref _yVelocity,
            RotationSnapTime);

        if (_currentHeight - _wantedHeight > 0.06)
        {
            _currentHeight = Mathf.Lerp(_currentHeight, _wantedHeight, HeightDamping * Time.deltaTime);
        }
        else
        {
            _currentHeight = _wantedHeight;
        }

        _wantedPosition = Target.position;
        _wantedPosition.y = _currentHeight;

        _usedDistance = Mathf.SmoothDampAngle(_usedDistance,
            Distance + (ParentRigidbody.velocity.magnitude * DistanceMultiplier), ref _zVelocity, DistanceSnapTime);

        _wantedPosition += Quaternion.Euler(0, _currentRotationAngle, 0) * new Vector3(0, 0, -_usedDistance);

        transform.position = _wantedPosition;

        transform.LookAt(Target.position + _lookAtVector);
    }
}