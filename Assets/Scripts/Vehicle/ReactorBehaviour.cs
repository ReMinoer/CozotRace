using UnityEngine;
using System.Collections;

public class ReactorBehaviour : MonoBehaviour {

	public GameObject Vehicle;
	private int maxP;
	private float maxER;
	private ParticleSystem syst;

	// Use this for initialization
	void Start () {
		Vehicle.GetComponent<VehicleMotor>().StateChanged += DrivingStateChanged;
		syst = GetComponent<ParticleSystem> ();
		maxP = syst.maxParticles;
		maxER = syst.emissionRate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DrivingStateChanged(object sender, VehicleMotor.StateChangedEventArgs e) {
		if (syst != null) {
			if (e.State.Forward > 0) {
				syst.Play ();
				/*if(syst.maxParticles <2*maxP) {
					syst.maxParticles = (int)(1.1*syst.maxParticles);
				}
				if(syst.emissionRate < 10*maxER) {
					syst.emissionRate = (1.5f*syst.emissionRate);
				}*/
			}
			else {
				syst.Stop ();
				/*if(syst.maxParticles>maxP) {
					syst.maxParticles = (int)(0.7*syst.maxParticles);
				}
				if(syst.emissionRate > maxER) {
					syst.emissionRate = (0.5f*syst.emissionRate);
				}*/
			}
						
		}
}
}
