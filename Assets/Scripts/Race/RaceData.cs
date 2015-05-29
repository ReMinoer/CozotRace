using UnityEngine;

public class RaceData : MonoBehaviour
{
    public string Level { get; set; }
    public string[] Models { get; set; }
    public int VehicleCount { get; set; }
    public int PlayerCount { get; set; }
    public int Laps { get; set; }

    public RaceData()
    {
        Models = new string[4];
    }
}