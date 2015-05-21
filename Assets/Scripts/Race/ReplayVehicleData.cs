using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class ReplayVehicleData : VehicleData
{
    public string ReplayPath { get; set; }
    public override GameObject Instantiate()
    {
        throw new System.NotImplementedException();
    }
}
