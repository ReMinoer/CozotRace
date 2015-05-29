using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{
    public List<DrivingState> ListDState = new List<DrivingState>();

    public void SerializeToFile(string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<DrivingState>));
        TextWriter textWriter = new StreamWriter(fileName);
        serializer.Serialize(textWriter, ListDState);
        textWriter.Close();
    }

    public void OnVehicleStateChanged(object sender, VehicleMotor.StateChangedEventArgs args)
    {
        //if (ListDState.Count == 0 || args.State.HasChange (ListDState.Last()))
        ListDState.Add(args.State);
    }

    private void Awake()
    {
        GetComponent<VehicleMotor>().StateChanged += OnVehicleStateChanged;
    }
}