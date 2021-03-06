﻿using System;
using DesignPattern;
using UnityEngine;

[Serializable]
public class PlayerVehicleData : VehicleData
{
    public PlayerIndex PlayerIndex;

    public override GameObject Instantiate(Transform startPosition)
    {
        GameObject gameObject = base.Instantiate(startPosition);

        var playerInput = gameObject.AddComponent<PlayerInput>();
        playerInput.Index = PlayerIndex;

        CameraVitesseEffects camera = Factory<CameraVitesseEffects>.New("Vehicles/CameraDynamic");
        camera.Target = gameObject.transform;
        camera.ParentRigidbody = gameObject.GetComponent<Rigidbody>();
        camera.transform.rotation = gameObject.transform.rotation;

        RaceUiManager ui = Factory<RaceUiManager>.New("Vehicles/RaceUiManager");
        ui.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
        ui.Vehicle = gameObject;
        ui.gameObject.transform.SetParent(gameObject.transform, false);

        CameraManager.Instance.RaceUiManagers.Add(ui);

        return gameObject;
    }
}