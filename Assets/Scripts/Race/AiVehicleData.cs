using System;
using UnityEngine;
using DesignPattern;
using Object = UnityEngine.Object;

[Serializable]
public class AiVehicleData : VehicleData
{
    public override GameObject Instantiate(Transform startPosition)
    {
        VehicleMotor vehicleMotor = Factory<VehicleMotor>.New("Vehicles/Vehicle");
        vehicleMotor.transform.position = startPosition.position;
        vehicleMotor.transform.rotation = startPosition.rotation;

        vehicleMotor.gameObject.AddComponent<AiInput>();

        var model = Object.Instantiate(Resources.Load("Vehicles/Models/BaseModel")) as GameObject;
        if (model == null)
            throw new NullReferenceException();
        model.GetComponentInChildren<ReactorBehaviour>().Vehicle = vehicleMotor.gameObject;
        model.transform.SetParent(vehicleMotor.gameObject.transform, false);

        return vehicleMotor.gameObject;
    }
}
