using UnityEngine;
using System.Collections;

public class Trails : MonoBehaviour {
	protected Vector3 velocity2D;
	public float vit;
	public float seuil;
	// Use this for initialization
	void Start () {
		GetComponent<TrailRenderer> ().enabled = false;
		vit = 0;
		//seuil = 3.8f;
	}

	// Update is called once per frame
	void Update () {
		velocity2D = gameObject.GetComponentInParent<Rigidbody> ().velocity;
		velocity2D.y = 0;
		float speed = velocity2D.magnitude;
		if (speed < seuil) {
			GetComponent<TrailRenderer> ().enabled = false;
		} else {
			GetComponent<TrailRenderer> ().enabled = true;
		}
		vit = speed;

	}
}
