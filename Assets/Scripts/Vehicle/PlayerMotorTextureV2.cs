using UnityEngine;

public class PlayerMotorTextureV2 : MonoBehaviour
{
	public float Speed = 6.0f;
	public float TurnSpeed = 200.0f;

	//private GroundDetectorV2 _groundDetector;

    void Start()
    {/*
        GroundDetectorSystem groundDetectorSystem = GetComponentInChildren<GroundDetectorSystem>();
		if (groundDetectorSystem != null)
			_groundDetector = groundDetectorSystem.Central;
		else
			_groundDetector = GetComponentInChildren<GroundDetectorV2> ();*/
    }

	void Update ()
	{/*
		float speedCoeff = 1;
		if (_groundDetector != null)
		{
			GroundProperty ground = _groundDetector.Ground;
	        speedCoeff = ground != null ? ground.SpeedCoeff : 1;
		}*/
		
		TerrainTexture terrainTexture = GetComponent<TerrainTexture>();
		float speedCoeff;
		if (terrainTexture != null)
			speedCoeff = Map.Instance.Grounds [GetComponent<TerrainTexture>().GetMainTexture (transform.position)].SpeedCoeff;
		else
			speedCoeff = 1;

		float horizontalInput = Input.GetAxis("Horizontal2");
		float verticalInput = Input.GetAxis("Vertical2");
		transform.Rotate(horizontalInput * TurnSpeed * Vector3.up * Time.deltaTime, Space.World);
		transform.Translate (verticalInput * speedCoeff * Speed * Vector3.back * Time.deltaTime);
	}
}
