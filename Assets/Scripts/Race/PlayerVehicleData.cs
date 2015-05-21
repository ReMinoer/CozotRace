using System;
using UnityEngine;
using System.Collections;
using DesignPattern;

[Serializable]
public class PlayerVehicleData : VehicleData
{
    public PlayerIndex PlayerIndex;

    public override GameObject Instantiate()
    {
        PlayerInput playerInput = Factory<PlayerInput>.New("Vehicles/PlayerVehicle");
        playerInput.Index = PlayerIndex;
        return playerInput.gameObject;
    }
}
