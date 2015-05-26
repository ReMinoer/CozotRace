using UnityEngine;

public class DataRace : MonoBehaviour
{
    public string Level { get; set; }
    public string[] Models { get; set; }
    public int VehicleCount { get; set; }
    public int PlayerCount { get; set; }
    public int Laps { get; set; }

    public DataRace()
    {
        Models = new string[4];
    }
}
