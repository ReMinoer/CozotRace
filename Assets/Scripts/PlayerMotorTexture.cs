using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotorTexture : MonoBehaviour {

	public struct GroundProperty {
		public Texture FloorTexture;
		public float MultiplierSpeedTexture;
	}

	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;
	
	public Texture FloorAcceleratorTexture;
	public float FloorAcceleratorSpeed = 2.0f;
	public Texture FloorReducerTexture;
	public float FloorReducerSpeed = 0.5f;
	
	private List<GroundProperty> Floor = new List<GroundProperty>();
	private float MultiplierSpeed = 1.0f;
	
	void Start () {
		GroundProperty FloorAccelerator = new GroundProperty();
		FloorAccelerator.FloorTexture = FloorAcceleratorTexture;
		FloorAccelerator.MultiplierSpeedTexture = FloorAcceleratorSpeed;

		GroundProperty FloorReducer = new GroundProperty();
		FloorReducer.FloorTexture = FloorReducerTexture;
		FloorReducer.MultiplierSpeedTexture = FloorReducerSpeed;

		Floor.Add (FloorAccelerator);
		Floor.Add (FloorReducer);
	}
	
	void Update () {
		transform.Rotate(TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (MultiplierSpeed *Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}
	
	void OnTriggerStay (Collider collider) {
		Floor.ForEach (delegate(GroundProperty floor) {
				if (collider.gameObject.renderer.material.mainTexture == floor.FloorTexture) {
						MultiplierSpeed = floor.MultiplierSpeedTexture;
				}
		});
	}
}
