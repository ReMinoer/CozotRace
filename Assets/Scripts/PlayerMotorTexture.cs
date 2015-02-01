﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotorTexture : MonoBehaviour
{
	public float Speed = 6.0f;
	public float TurnSpeed = 200.0f;

	private float _speedCoeff = 1.0f;

	void Start()
	{
	}

	void Update ()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		transform.Rotate(horizontalInput * Mathf.Ceil(verticalInput) * TurnSpeed * Vector3.up * Time.deltaTime, Space.World);
		transform.Translate (verticalInput * _speedCoeff * Speed * Vector3.back * Time.deltaTime);
	}
	
	void OnTriggerStay (Collider collider)
	{
		foreach (Map.GroundProperty ground in Map.Instance.Grounds)
			if (collider.gameObject.renderer.material.mainTexture == ground.Texture)
			{
				_speedCoeff = ground.SpeedCoeff;
				break;
			}
	}
}
