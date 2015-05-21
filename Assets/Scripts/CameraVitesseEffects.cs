using UnityEngine;
using System.Collections;

public class CameraVitesseEffects : MonoBehaviour {

	public Transform target;
	public float distance = 0.35f;
	public float height = 0.3f;
	public float heightDamping = 10.0f;
	
	public float lookAtHeight = 0.02f;
	
	public Rigidbody parentRigidbody;
	
	public float rotationSnapTime = 0.5F;
	
	public float distanceSnapTime = 0.5F;
	public float distanceMultiplier = 0.08F;
	
	private Vector3 lookAtVector;
	
	private float usedDistance = 1.0F;
	
	float wantedRotationAngle;
	float wantedHeight;
	
	float currentRotationAngle;
	float currentHeight;
	
	Quaternion currentRotation;
	Vector3 wantedPosition;
	
	private float yVelocity = 0.0F;
	private float zVelocity = 0.0F;
	
	void Start () {
		
		lookAtVector =  new Vector3(0,lookAtHeight,0);
		
	}
	void LateUpdate () {
		
		wantedHeight = target.position.y + height;
		currentHeight = transform.position.y;
		
		wantedRotationAngle = target.eulerAngles.y;
		currentRotationAngle = transform.eulerAngles.y;

		currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);

		if (currentHeight - wantedHeight > 0.06) {
			currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);	
		} else {
			currentHeight = wantedHeight;
				}

		wantedPosition = target.position;
		wantedPosition.y = currentHeight;
		
		usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (parentRigidbody.velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime); 
		
		wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);
		
		transform.position = wantedPosition;
		
		transform.LookAt(target.position + lookAtVector);
		
	}
}
