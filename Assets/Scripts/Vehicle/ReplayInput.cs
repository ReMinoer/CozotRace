using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class ReplayInput : MonoBehaviour {

	public List<DrivingState> ListDState;
	private DrivingState current;
	static List<DrivingState> DeserializeFromFile(string fileName)
	{
		XmlSerializer deserializer = new XmlSerializer(typeof(List<DrivingState>));
		TextReader textReader = new StreamReader(fileName);
		List<DrivingState> ListDState; 
		ListDState = (List<DrivingState>)deserializer.Deserialize(textReader);
		textReader.Close();
		
		return ListDState;
	}

	void Awake () {
		ListDState = DeserializeFromFile (@"C:\test.xml");

	}

	// Use this for initialization
	void Start () {
		current = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (ListDState.Count != 0) {
			if (Time.realtimeSinceStartup >= ListDState.First().Time) {
				current = ListDState.First();
				ListDState.RemoveAt(0);
			}
		}
		if (current != null) {
			GetComponent<VehicleMotor> ().ChangeState (current);
		}

	}
	
}
