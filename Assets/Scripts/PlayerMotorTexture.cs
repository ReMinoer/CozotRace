using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotorTexture : MonoBehaviour {

	public struct GroundProperty {
		public Texture FloorTexture { get; set; }
		public float MultiplierSpeedTexture { get; set; }
	}

	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;

	public List<GroundProperty> Floor = new List<GroundProperty>() {
		new GroundProperty() {FloorTexture = new Texture(), MultiplierSpeedTexture = 0.5f},
		new GroundProperty() {FloorTexture = new Texture(), MultiplierSpeedTexture = 2.0f}
	};

	private float MultiplierSpeed = 1.0f;
	
	void Start () {
	}
	
	void Update () {
		transform.Rotate(TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (MultiplierSpeed *Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}
	
	void OnTriggerStay (Collider collider) {
		foreach (GroundProperty floor in Floor) {
			if (collider.gameObject.renderer.material.mainTexture == floor.FloorTexture) {
					MultiplierSpeed = floor.MultiplierSpeedTexture;
			}
		}
	}
}
