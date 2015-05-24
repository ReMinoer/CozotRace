using UnityEngine;
using System.Collections;

public class ReactorBehaviour : MonoBehaviour {

	public GameObject Vehicle;
	private int maxP;
	private float maxER;
	private ParticleSystem syst;
	private Color firstColor;
	private VehicleMotor vehicleMotor;
	private ParticleSystem.Particle []ParticleList;

	// Use this for initialization
	void Start () {
		vehicleMotor = Vehicle.GetComponent<VehicleMotor> ();
		vehicleMotor.StateChanged += DrivingStateChanged;
		syst = GetComponent<ParticleSystem> ();
		ParticleList = new ParticleSystem.Particle[syst.particleCount];
		maxP = syst.maxParticles;
		maxER = syst.emissionRate;
		firstColor = syst.startColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DrivingStateChanged(object sender, VehicleMotor.StateChangedEventArgs e) {
		Color startColor, lastColor;
		if (syst != null) {
			if (e.State.Forward > 0) {
				syst.Play ();
			}
			else {
				syst.Stop ();
			}
			ParticleList = new ParticleSystem.Particle[syst.particleCount];
			syst.GetParticles(ParticleList);
			if(vehicleMotor.isBoosted) {
				//changer en bleu
				startColor = Color.cyan;
				lastColor = Color.blue;
			}
			else {
				startColor=firstColor;
				lastColor=Color.black;
			}
			for(int i = 0; i < ParticleList.Length; ++i)
			{
				float LifeProcentage = (ParticleList[i].lifetime / ParticleList[i].startLifetime);
				ParticleList[i].color = Color.Lerp(startColor, lastColor, LifeProcentage);
			}  
			syst.SetParticles(ParticleList, syst.particleCount);
		}
}
}
