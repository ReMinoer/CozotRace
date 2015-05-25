using UnityEngine;
using System.Collections.Generic;

public class ReplayManager : MonoBehaviour
{
    public Dictionary<VehicleMotor, ReplayRecorder> Recorders { get; private set; }

	void Awake()
    {
        Recorders = new Dictionary<VehicleMotor, ReplayRecorder>();
	}
	
	public void AddRecorder(VehicleMotor vehicleMotor)
    {
	    var recorder = new ReplayRecorder();
	    vehicleMotor.StateChanged += recorder.OnVehicleStateChanged;
        Recorders.Add(vehicleMotor, recorder);
    }

    public void StopRecorder(VehicleMotor vehicleMotor)
    {
        if (!Recorders.ContainsKey(vehicleMotor))
            return;

        vehicleMotor.StateChanged -= Recorders[vehicleMotor].OnVehicleStateChanged;
    }

    public void Clear(VehicleMotor vehicleMotor)
    {
        Recorders.Clear();
    }
}
