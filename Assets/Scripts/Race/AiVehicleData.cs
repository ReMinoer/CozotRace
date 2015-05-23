using System;
using UnityEngine;
using System.Collections;
using DesignPattern;

[Serializable]
public class AiVehicleData : VehicleData
{
    public override GameObject Instantiate()
    {
        AiInput aiInput = Factory<AiInput>.New("Vehicles/AiVehicle");
        return aiInput.gameObject;
    }
}
