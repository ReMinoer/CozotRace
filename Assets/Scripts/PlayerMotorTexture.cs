using UnityEngine;
using System.Collections;

public class PlayerMotorTexture : MonoBehaviour {
	
	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;
	public Texture asphalt;
	public float ReducerSpeedTexture1 = 2.0f;
	public Texture grass;
	public float ReducerSpeedTexture2 = 0.8f;
	
	private float ReducerSpeed = 1.0f;
	
	void Start () {
	}
	
	void Update () {
		transform.Rotate(ReducerSpeed *TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (ReducerSpeed *Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}
	
	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.renderer.material.mainTexture == grass) {
			ReducerSpeed = ReducerSpeedTexture1;
		} 
		else if (collider.gameObject.renderer.material.mainTexture == asphalt) {
			ReducerSpeed = ReducerSpeedTexture2;
		}
	}
}
