using System;
using UnityEngine;

[Serializable]
public class DrivingState
{
    public float Time;
    // Continous state
    public float Forward; // [0;1]
    public float Backward; // [0;1]
    public float Turn; // [-1;1]
    public bool Boost;
    // Ponctual state
    public bool DashLeft;
    public bool DashRight;
    // Unsynchronize fix
    public Vector3? Position;
    public Quaternion? Rotation;

    public bool HasChange(DrivingState other)
    {
        return Math.Abs(Forward - other.Forward) > float.Epsilon
               || Math.Abs(Backward - other.Backward) > float.Epsilon
               || Math.Abs(Turn - other.Turn) > float.Epsilon
               || Boost != other.Boost
               || DashLeft
               || DashRight;
    }
}