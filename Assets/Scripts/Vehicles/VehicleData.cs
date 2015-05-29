using System;
using DesignPattern;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class VehicleData
{
    public string Model;

    public virtual GameObject Instantiate(Transform startPosition)
    {
        VehicleMotor vehicleMotor = Factory<VehicleMotor>.New("Vehicles/Vehicle");
        vehicleMotor.transform.position = startPosition.position;
        vehicleMotor.transform.rotation = startPosition.rotation;

        Object modelResource = Resources.Load("Vehicles/Models/" + Model);
        if (modelResource == null)
        {
            modelResource = Resources.Load("Vehicles/Models/BaseModel");
            if (modelResource == null)
                throw new NullReferenceException();
        }
        var model = Object.Instantiate(modelResource) as GameObject;

        if (model == null)
            throw new NullReferenceException();

        model.GetComponentInChildren<ReactorBehaviour>().Vehicle = vehicleMotor.gameObject;
        model.transform.SetParent(vehicleMotor.gameObject.transform, false);

        return vehicleMotor.gameObject;
    }
}