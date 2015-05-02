using UnityEngine;
using System.Collections;

public class ReactorBehaviour : MonoBehaviour {

	public GameObject Vehicle;

	// Use this for initialization
	void Start () {
		Vehicle.GetComponent<VehicleMotor>().StateChanged += DrivingStateChanged;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DrivingStateChanged(object sender, VehicleMotor.StateChangedEventArgs e) {
		ParticleSystem syst = GetComponent<ParticleSystem> ();
		if (syst != null) {
						if (e.State.Forward > 0)
								syst.Play ();
						else
								syst.Stop ();
						
				}
}
}
