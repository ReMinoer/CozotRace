using UnityEngine;
using System.Collections;


public class VehicleSound : MonoBehaviour {
    public AudioSource motorSound;
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
        GetComponent<VehicleMotor>().StateChanged += OnVehicleStateChanged;
    }
    public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
    {
        
        // Traitement en fonction de args.State
        if(args.State.Forward > 0.0 && lastForward == 0)
        {
            motorSound.Play();
        }
        lastForward = args.State.Forward;
        //if (GetComponent<VehicleMotor>().SpeedEvolution == 0)
        if(args.State.Forward == 0)
        {
            motorSound.Stop();
        }
    }
}
