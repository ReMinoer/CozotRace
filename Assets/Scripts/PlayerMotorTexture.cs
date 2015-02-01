using UnityEngine;
using System.Collections;

public class PlayerMotorTexture : MonoBehaviour {

	public struct GroundProperty {
		public Texture texture;
		public float ReducerSpeedTexture;
	}

	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;
	public GroundProperty[] floor;
	
	private float ReducerSpeed = 1.0f;
	
	void Start () {
	}
	
	void Update () {
		transform.Rotate(TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (ReducerSpeed *Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}
	
	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.renderer.material.mainTexture == floor[0].texture) {
			ReducerSpeed = floor[0].ReducerSpeedTexture;
		} 
		else if (collider.gameObject.renderer.material.mainTexture == floor[1].texture) {
			ReducerSpeed = floor[1].ReducerSpeedTexture;
		}
	}
}
