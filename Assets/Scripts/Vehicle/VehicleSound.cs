using System;
using UnityEngine;

public class VehicleSound : MonoBehaviour
{
    public AudioSource MotorSound;
    public AudioSource AccelerationSound;
    public AudioSource BrakingSound;

    float lastForward;

    void Awake()
    {
        MotorSound.Play();
        GetComponent<VehicleMotor>().StateChanged += OnVehicleStateChanged;
    }

    public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
    {
        if(args.State.Forward > 0.0 && lastForward == 0)
        {
            AccelerationSound.Play();
        }
        lastForward = args.State.Forward;
        
        if(args.State.Forward < float.Epsilon)
        {
            AccelerationSound.Stop();
        }
    }
}
