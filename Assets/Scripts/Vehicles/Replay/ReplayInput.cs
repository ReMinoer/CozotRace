using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class ReplayInput : MonoBehaviour
{
    public List<DrivingState> ListDState;
    private DrivingState _current;

    static private List<DrivingState> DeserializeFromFile(string fileName)
    {
        var deserializer = new XmlSerializer(typeof(List<DrivingState>));
        TextReader textReader = new StreamReader(fileName);
        var listDState = (List<DrivingState>)deserializer.Deserialize(textReader);
        textReader.Close();

        return listDState;
    }

    private void Start()
    {
        _current = null;
    }

    private void FixedUpdate()
    {
        bool valid;
        do
        {
            valid = false;
            if (ListDState.Count != 0 && (float)GameManager.Instance.Chronometer.TotalSeconds >= ListDState.First().Time)
            {
                _current = ListDState.First();
                ListDState.RemoveAt(0);
                valid = true;
            }
        } while (valid);

        if (ListDState.Count == 0)
        {
            gameObject.AddComponent<AiInput>();
            Destroy(this);
        }

        if (_current != null)
            GetComponent<VehicleMotor>().ChangeState(_current);
    }
}