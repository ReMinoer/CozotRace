using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour {

	public enum PlayerIndex
	{
		PlayerOne,
		PlayerTwo,
		PlayerThree,
		PlayerFour
	}

	public PlayerIndex Index;

	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		if (Players.Count == 1) {
//
//			DrivingState drivingState = new DrivingState ();
//			float horizontalInput = Input.GetAxis ("Horizontal");
//			float verticalInput = Input.GetAxis ("Vertical");
//
//			drivingState.Forward = Mathf.Clamp (verticalInput, 0, 1);
//			drivingState.Backward = -Mathf.Clamp (verticalInput, -1, 0);
//			drivingState.Turn = horizontalInput;
//
//			GetComponent<VehicleMotor> ().ChangeState (drivingState);
//		}
//
//		if (Players.Count == 2) {
//			DrivingState drivingState1 = new DrivingState ();
//			float horizontalInput1 = Input.GetAxis ("Horizontal");
//			float verticalInput1 = Input.GetAxis ("Vertical");
//			
//			drivingState1.Forward = Mathf.Clamp (verticalInput1, 0, 1);
//			drivingState1.Backward = -Mathf.Clamp (verticalInput1, -1, 0);
//			drivingState1.Turn = horizontalInput1;
//			
//			Players[0].GetComponent<VehicleMotor> ().ChangeState (drivingState1);
//
//			DrivingState drivingState2 = new DrivingState ();
//			float horizontalInput2 = Input.GetAxis ("Horizontal2");
//			float verticalInput2 = Input.GetAxis ("Vertical2");
//			
//			drivingState2.Forward = Mathf.Clamp (verticalInput2, 0, 1);
//			drivingState2.Backward = -Mathf.Clamp (verticalInput2, -1, 0);
//			drivingState2.Turn = horizontalInput2;
//			
//			Players[1].GetComponent<VehicleMotor> ().ChangeState (drivingState2);
		//		}
		DrivingState drivingState = new DrivingState ();
		float horizontalInput;
		float verticalInput;

		switch (Index)
		{
		case PlayerIndex.PlayerOne:
			horizontalInput = Input.GetAxis ("Horizontal");
			verticalInput = Input.GetAxis ("Vertical");
			break;
		case PlayerIndex.PlayerTwo:
			horizontalInput = Input.GetAxis ("Horizontal2");
			verticalInput = Input.GetAxis ("Vertical2");
			break;
		default:
			horizontalInput = Input.GetAxis ("Horizontal");
			verticalInput = Input.GetAxis ("Vertical");
			break;
		}

		drivingState.Forward = Mathf.Clamp (verticalInput, 0, 1);
		drivingState.Backward = -Mathf.Clamp (verticalInput, -1, 0);
		drivingState.Turn = horizontalInput;
		
		GetComponent<VehicleMotor> ().ChangeState (drivingState);
	}

//	void OnValidate()
//	{
//		if (Players.Count == 0)
//		{
//			Players.RemoveRange (0, Players.Count - 0);
//			Debug.LogError ("You need have 1 or 2 players.");
//		}
//
//		if (Players.Count > 2)
//		{
//			Players.RemoveRange (2, Players.Count - 2);
//			Debug.LogError ("You can't have more than 2 players.");
//		}
//	}
}
