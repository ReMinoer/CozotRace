using System;
using UnityEngine;

[Serializable]
public class AiVehicleData : VehicleData
{
    public override GameObject Instantiate(Transform startPosition)
    {
        GameObject gameObject = base.Instantiate(startPosition);

        gameObject.AddComponent<AiInput>();

        return gameObject;
    }
}
