using UnityEngine;
using System.Collections;

public class GroundDetector : MonoBehaviour {

	public PlayerMotorTexture vehicle;

	void Start () {
	}

	void Update () {
	}

	void OnTriggerStay (Collider collider)
	{
		foreach (Map.GroundProperty ground in Map.Instance.Grounds)
			if (collider.gameObject.renderer.material.mainTexture == ground.Texture)
		{
			vehicle.SpeedCoeff = ground.SpeedCoeff;
			break;
		}
	}
}
