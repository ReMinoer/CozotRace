using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    public Dictionary<VehicleMotor, ReplayRecorder> Recorders { get; private set; }
    public List<VehicleData> VehiclesData { get; set; }

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

    private void Awake()
    {
        VehiclesData = new List<VehicleData>();
        Recorders = new Dictionary<VehicleMotor, ReplayRecorder>();
    }
}