using System;
using UnityEngine;

public class VehicleSound : MonoBehaviour
{
    public AudioSource AccelerationSound;
    public AudioSource MotorSound;
    private float _lastForward;

    public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
    {
        if (args.State.Forward > 0.0 && Math.Abs(_lastForward) < float.Epsilon)
        {
            AccelerationSound.Play();
        }
        _lastForward = args.State.Forward;

        if (args.State.Forward < float.Epsilon)
        {
            AccelerationSound.Stop();
        }
    }

    private void Awake()
    {
        MotorSound.Play();
        GetComponent<VehicleMotor>().StateChanged += OnVehicleStateChanged;
    }
}