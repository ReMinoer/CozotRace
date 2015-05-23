using System;
using UnityEngine;
using DesignPattern;
using Object = UnityEngine.Object;

[Serializable]
public class PlayerVehicleData : VehicleData
{
    public PlayerIndex PlayerIndex;

    public override GameObject Instantiate()
    {
        VehicleMotor vehicleMotor = Factory<VehicleMotor>.New("Vehicles/Vehicle");
        vehicleMotor.gameObject.AddComponent<PlayerInput>();
        vehicleMotor.gameObject.GetComponent<PlayerInput>().Index = PlayerIndex;

        var model = Object.Instantiate(Resources.Load("Vehicles/Models/BaseModel")) as GameObject;
        if (model == null)
            throw new NullReferenceException();
        model.GetComponentInChildren<ReactorBehaviour>().Vehicle = vehicleMotor.gameObject;
        model.transform.parent = vehicleMotor.gameObject.transform;

        CameraVitesseEffects camera = Factory<CameraVitesseEffects>.New("Vehicles/CameraDynamic");
        camera.target = vehicleMotor.transform;
        camera.parentRigidbody = vehicleMotor.GetComponent<Rigidbody>();
        camera.gameObject.transform.parent = vehicleMotor.gameObject.transform;

        RaceUiManager ui = Factory<RaceUiManager>.New("Vehicles/RaceUiManager");
        ui.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
        ui.Vehicle = vehicleMotor.gameObject;
        ui.gameObject.transform.parent = vehicleMotor.gameObject.transform;

        CameraManager.Instance.RaceUiManagers.Add(ui);

        return vehicleMotor.gameObject;
    }
}
