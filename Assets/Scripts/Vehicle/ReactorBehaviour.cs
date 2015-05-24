using UnityEngine;
using System.Collections;

public class ReactorBehaviour : MonoBehaviour {

	public GameObject Vehicle;
	private int maxP;
	private float maxER;
	private ParticleSystem syst;
	private VehicleMotor vehicleMotor;
	private ParticleSystem.Particle []ParticleList;
	private Color firstColor, startColor, midColor, lastColor;

	// Use this for initialization
	void Start () {
		vehicleMotor = Vehicle.GetComponent<VehicleMotor> ();
		vehicleMotor.StateChanged += DrivingStateChanged;
		syst = GetComponent<ParticleSystem> ();
		ParticleList = new ParticleSystem.Particle[syst.particleCount];
		syst.GetParticles(ParticleList);
		firstColor = new Color(206/255f,27/255f,27/255f,1);
		startColor=new Color(197/255f,233/255f,32/255f,1);
		midColor = new Color(188/255f,6/255f,32/255f,1);
		lastColor = new Color(2/255f,2/255f,2/255f,1);
		maxP = syst.maxParticles;
		maxER = syst.emissionRate;
	}
	
	// Update is called once per frame
	void Update () {
		/*for(int i = 0; i < ParticleList.Length; ++i)
		{
			float LifeProcentage = ((ParticleList[i].startLifetime-ParticleList[i].lifetime) / ParticleList[i].startLifetime)*100f;
			if(LifeProcentage<5.3)
				ParticleList[i].color = Color.Lerp(firstColor, startColor, LifeProcentage/5.3f);
			else if(LifeProcentage<79.1)
				ParticleList[i].color = Color.Lerp(startColor, midColor, (LifeProcentage-5.3f)/(79.1f-5.3f));
			else
				ParticleList[i].color = Color.Lerp(midColor, lastColor, (LifeProcentage-79.1f)/(100f-79.1f));
		}  
		syst.SetParticles(ParticleList, syst.particleCount);*/
	}
	
	void DrivingStateChanged(object sender, VehicleMotor.StateChangedEventArgs e) {
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
				firstColor = new Color(27/255f,27/255f,206/255f,1);
				startColor=new Color(32/255f,216/255f,233/255f,1);
				midColor = new Color(72/255f,4/255f,251/255f,1);
				syst.startColor=firstColor;
			}
			else {
				firstColor = new Color(206/255f,27/255f,27/255f,1);
				startColor=new Color(197/255f,233/255f,32/255f,1);
				midColor = new Color(188/255f,6/255f,32/255f,1);
				syst.startColor=firstColor;
			}
		}

}
}
