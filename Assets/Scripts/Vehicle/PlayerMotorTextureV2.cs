using UnityEngine;

public class PlayerMotorTextureV2 : MonoBehaviour
{
	public float Speed = 6.0f;
	public float TurnSpeed = 200.0f;

	private GroundDetectorSystem _groundDetectorSystem;

    void Start()
    {
        _groundDetectorSystem = GetComponentInChildren<GroundDetectorSystem>();
    }

	void Update ()
	{
	    GroundProperty ground = _groundDetectorSystem.Central.Ground;
        float speedCoeff = ground != null ? ground.SpeedCoeff : 1;

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		transform.Rotate(horizontalInput * TurnSpeed * Vector3.up * Time.deltaTime, Space.World);
		transform.Translate (verticalInput * speedCoeff * Speed * Vector3.back * Time.deltaTime);
	}
}
