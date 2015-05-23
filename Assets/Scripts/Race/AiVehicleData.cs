using System;
using UnityEngine;
using DesignPattern;
using Object = UnityEngine.Object;

[Serializable]
public class AiVehicleData : VehicleData
{
    public override GameObject Instantiate()
    {
        VehicleMotor vehicleMotor = Factory<VehicleMotor>.New("Vehicles/Vehicle");
        vehicleMotor.gameObject.AddComponent<AiInput>();

        var model = Object.Instantiate(Resources.Load("Vehicles/Models/BaseModel")) as GameObject;
        if (model == null)
            throw new NullReferenceException();
        model.GetComponentInChildren<ReactorBehaviour>().Vehicle = vehicleMotor.gameObject;
        model.transform.parent = vehicleMotor.gameObject.transform;

        return vehicleMotor.gameObject;
    }
}
