using System;
using UnityEngine;
using System.Collections;

[Serializable]
public abstract class VehicleData
{
    public abstract GameObject Instantiate(Transform startPosition);
}
