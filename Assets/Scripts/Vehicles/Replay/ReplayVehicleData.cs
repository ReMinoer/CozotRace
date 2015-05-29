using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReplayVehicleData : VehicleData
{
    public List<DrivingState> DrivingStates { get; set; }

    public override GameObject Instantiate(Transform startPosition)
    {
        GameObject gameObject = base.Instantiate(startPosition);

        var replayInput = gameObject.AddComponent<ReplayInput>();
        replayInput.ListDState = DrivingStates;

        return gameObject;
    }
}