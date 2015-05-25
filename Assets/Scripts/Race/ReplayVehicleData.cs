using System;
using UnityEngine;
using System.Collections.Generic;
using DesignPattern;
using Object = UnityEngine.Object;

[Serializable]
public class ReplayVehicleData : VehicleData
{
    public List<DrivingState> DrivingStates { get; set; }

    public override GameObject Instantiate(Transform startPosition)
    {
        VehicleMotor vehicleMotor = Factory<VehicleMotor>.New("Vehicles/Vehicle");
        vehicleMotor.transform.position = startPosition.position;
        vehicleMotor.transform.rotation = startPosition.rotation;

        var replayInput = vehicleMotor.gameObject.AddComponent<ReplayInput>();
        replayInput.ListDState = DrivingStates;

        var model = Object.Instantiate(Resources.Load("Vehicles/Models/BaseModel")) as GameObject;
        if (model == null)
            throw new NullReferenceException();
        model.GetComponentInChildren<ReactorBehaviour>().Vehicle = vehicleMotor.gameObject;
        model.transform.SetParent(vehicleMotor.gameObject.transform, false);

        return vehicleMotor.gameObject;
    }
}
