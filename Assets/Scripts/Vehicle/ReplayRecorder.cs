using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class ReplayRecorder : MonoBehaviour {

	public List<DrivingState> ListDState = new List<DrivingState>();

	public void SerializeToFile(string fileName)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(List<DrivingState>));
		TextWriter textWriter = new StreamWriter(fileName);
		serializer.Serialize(textWriter, ListDState);
		textWriter.Close();
	}

	public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
	{ 
		if (ListDState.Count == 0 || args.State.HasChange (ListDState.Last())) {
			ListDState.Add(args.State);		
		}
	}

	void Awake()
	{
		GetComponent<VehicleMotor> ().StateChanged += OnVehicleStateChanged;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy () {
		//SerializeToFile (@"C:\test.xml");
	}

}
