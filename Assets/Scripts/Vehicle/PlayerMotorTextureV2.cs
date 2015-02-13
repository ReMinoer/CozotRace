using UnityEngine;

public class PlayerMotorTextureV2 : MonoBehaviour
{
	public float Speed = 6.0f;
	public float TurnSpeed = 200.0f;

	private GroundDetectorV2 _groundDetector;

    void Start()
    {
        GroundDetectorSystem groundDetectorSystem = GetComponentInChildren<GroundDetectorSystem>();
		if (groundDetectorSystem != null)
			_groundDetector = groundDetectorSystem.Central;
		else
			_groundDetector = GetComponentInChildren<GroundDetectorV2> ();
    }

	void Update ()
	{
		float speedCoeff = 1;
		if (_groundDetector != null)
		{
			GroundProperty ground = _groundDetector.Ground;
	        speedCoeff = ground != null ? ground.SpeedCoeff : 1;
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		transform.Rotate(horizontalInput * TurnSpeed * Vector3.up * Time.deltaTime, Space.World);
		transform.Translate (verticalInput * speedCoeff * Speed * Vector3.back * Time.deltaTime);
	}
}
