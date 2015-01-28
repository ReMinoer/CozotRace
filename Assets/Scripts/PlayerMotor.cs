using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

	public float playerSpeed = 4.0f;

	void Start () {
	
		//Player spawn point
		transform.position = new Vector3 (0.0f, 0.05f, 4.0f);
	}

	void Update () {
	
		Vector3 vert = new Vector3 (0.0f, 0.0f, -1.0f);
		Vector3 rotate = new Vector3(0.0f,100.0f,0.0f);
		transform.Rotate(rotate * Input.GetAxis ("Horizontal") * Mathf.Ceil(Input.GetAxis ("Vertical")) * playerSpeed/2.0f * Time.deltaTime, Space.World);
		transform.Translate (vert * Input.GetAxis ("Vertical") * playerSpeed * Time.deltaTime);
	}
}
