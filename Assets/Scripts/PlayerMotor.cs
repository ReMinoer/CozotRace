using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	
	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;
	public Texture asphalt;
	public Texture grass;

	private float ReducerSpeed = 1.0f;

	void Start () {
	}

	void Update () {
		transform.Rotate(ReducerSpeed *TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (ReducerSpeed *Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}

	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.renderer.material.mainTexture == grass) {
			ReducerSpeed = 2.0f;
		} 
		else if (collider.gameObject.renderer.material.mainTexture == asphalt) {
			ReducerSpeed = 0.5f;
		}
	}
}
