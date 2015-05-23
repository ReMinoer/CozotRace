using UnityEngine;
using System.Collections;

public class Springboard : MonoBehaviour {

	void OnTriggerEnter(Collider player) {

		VehicleMotor vehiclemotor = player.GetComponent<VehicleMotor> ();
		if (vehiclemotor == null)
			return;
		vehiclemotor.isBoosted = true;
		vehiclemotor.ForwardSpeedMaxActual = 50f;
	}

	void OnTriggerQuit(Collider player) {
		
		VehicleMotor vehiclemotor = player.GetComponent<VehicleMotor> ();
		if (vehiclemotor == null)
			return;
		vehiclemotor.isBoosted = false;
		vehiclemotor.ForwardSpeedMaxActual = 25f;
	}

}
