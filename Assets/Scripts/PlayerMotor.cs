using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	
	public float PlayerSpeed = 4.0f;
	public float TurnSpeed = 2.0f;
	public float TurnPower = 100.0f;

	void Start () {
	}

	void Update () {
		transform.Rotate(TurnPower * Vector3.up * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * TurnSpeed * Time.deltaTime, Space.World);
		transform.Translate (Vector3.back * Input.GetAxis ("Vertical") * PlayerSpeed * Time.deltaTime);
	}
}
