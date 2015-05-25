using UnityEngine;
using System.Collections;


public class VehicleSound : MonoBehaviour {
    public AudioSource motorSound;
    public AudioSource AccelerationSound;
    public AudioSource BrakingSound;
    float lastForward;
	// Use this for initialization
	void Start () {
        lastForward = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        motorSound.Play();
        GetComponent<VehicleMotor>().StateChanged += OnVehicleStateChanged;
    }
    public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
    {
        
        // Traitement en fonction de args.State
        //GetComponent<VehicleMotor>().SpeedEvolution != 0
       
        
        
        if(args.State.Forward > 0.0 && lastForward == 0)
        {
            AccelerationSound.Play();
        }
        lastForward = args.State.Forward;
        
        if(args.State.Forward == 0)
        {
            AccelerationSound.Stop();
        }
    }
}
