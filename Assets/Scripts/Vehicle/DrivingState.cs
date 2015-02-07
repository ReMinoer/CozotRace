using UnityEngine;
using System.Collections;

[System.Serializable]
public class DrivingState
{
	// Continous state
	public float Forward; // [0;1]
	public float Backward; // [0;1]
	public float Turn; // [-1;1]
	public bool Boost;

	// Ponctual state
	public bool DashLeft;
	public bool DashRight;

	// Unsynchronize fix
	public Vector3 Position;
	public Quaternion Rotation;

	public bool HasChange(DrivingState other)
	{
		return Forward != other.Forward
			|| Backward != other.Backward
			|| Turn != other.Turn
			|| Boost != other.Boost
			|| DashLeft
			|| DashRight;
	}
}
