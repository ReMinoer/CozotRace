using DesignPattern;
using UnityEngine;

public class ReplayCameraSystem : Factory<ReplayCameraSystem>
{
    public Transform Target;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        _camera.transform.position = Target.position + 10 * Vector3.up;
        _camera.transform.LookAt(Target);
    }
}
