using DesignPattern;
using UnityEngine;

public class ReplayCameraSystem : Factory<ReplayCameraSystem>
{
    public Transform Target;
    public float Height = 10f;
    private Camera _camera;
    private const float Speed = 0.12f;
    private const float SpeedRotation = 0.4f;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        Vector3 targetPosition = Target.position + Height * Vector3.up;
        _camera.transform.position = targetPosition;
        _camera.transform.LookAt(Target);
    }

    private void Update()
    {
        Vector3 targetPosition = Target.position + Height * Vector3.up;
        Vector3 gap = targetPosition - _camera.transform.position;

        if (gap.magnitude < 1f)
            return;

        _camera.transform.position += gap.normalized * Speed;

        Quaternion targetRotation = Quaternion.LookRotation(Target.position - _camera.transform.position);
        _camera.transform.rotation = Quaternion.RotateTowards(_camera.transform.rotation, targetRotation, SpeedRotation);
    }
}