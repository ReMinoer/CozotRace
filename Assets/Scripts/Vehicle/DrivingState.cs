using UnityEngine;
using System.Collections;

[System.Serializable]
public class DrivingState
{
	// Continous state
	public float Accelerate; // [0;1]
	public float Brake; // [0;1] (include backward)
	public float Turn; // [-TurnSpeedMax;TurnSpeedMax]
	public bool Boost;

	// Ponctual state
	public bool DashLeft;
	public bool DashRight;

	// Unsynchronize fix
	public Vector3 Position;
	public Quaternion Rotation;

	public bool HasChange(DrivingState other)
	{
		return Accelerate != other.Accelerate
			|| Brake != other.Brake
			|| Turn != other.Turn
			|| Boost != other.Boost
			|| DashLeft
			|| DashRight;
	}
}
