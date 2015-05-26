using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class ReplayInput : MonoBehaviour
{
	public List<DrivingState> ListDState;
	private DrivingState current;

	static List<DrivingState> DeserializeFromFile(string fileName)
	{
		XmlSerializer deserializer = new XmlSerializer(typeof(List<DrivingState>));
		TextReader textReader = new StreamReader(fileName);
		List<DrivingState> listDState = (List<DrivingState>)deserializer.Deserialize(textReader);
		textReader.Close();

        return listDState;
	}

	void Start ()
    {
		current = null;
	}
	
	void FixedUpdate ()
	{
	    bool valid = false;
	    do
        {
            valid = false;
            if (ListDState.Count != 0 && (float)GameManager.Instance.Chronometer.TotalSeconds >= ListDState.First().Time)
            {
			    current = ListDState.First();
			    ListDState.RemoveAt(0);
                valid = true;
            }
        } while (valid);

	    if (ListDState.Count == 0)
	    {
	        gameObject.AddComponent<AiInput>();
	        Destroy(this);
	    }

		if (current != null)
			GetComponent<VehicleMotor> ().ChangeState (current);
	}
	
}
