using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour
{
    public DataRace DataRace;

    public Controler RaceControler;
	public Text RaceName;
    public Transform RaceTransform;
    private string _lastRace;
    private GameObject _currentTerrain;
	private string[] mapNames = {"EasyPlain", "GrassHills", "SpringLoop"};

    public Controler[] VehicleControlers = new Controler[4];
    public Transform[] VehicleTransforms = new Transform[4];
    private string[] _lastVehicles = new string[4];
    private GameObject[] _currentVehicles = new GameObject[4];
    private string[] vehicleNames = { "blue", "cyan", "green", "lightGreen", "orange", "pink", "purple", "yellow" };

    public Controler NumberVehicleControler;
    public Text CounterVehicles;
    private int[] numberVehicles = { 1, 2, 3, 4, 5, 6, 7, 8 };

    public Controler NumberPlayerControler;
	public Text CounterPlayers;
    private int[] numberPlayers = { 1, 2, 3, 4 };

    public Controler NumberTurnControler;
	public Text CounterTurn;
    private int[] numberTurns = { 1, 2, 3, 4, 5 };

	void Update()
    {
        // Race
        int raceValue = RaceControler.value;
        RaceName.text = mapNames[raceValue];

	    if (_lastRace != mapNames[raceValue])
	    {
            Destroy(_currentTerrain);

            _currentTerrain = Instantiate(Resources.Load("Terrains/" + mapNames[raceValue])) as GameObject;
            if (_currentTerrain == null)
                throw new NullReferenceException();
            _currentTerrain.transform.SetParent(RaceTransform, false);
	    }
        _lastRace = mapNames[raceValue];

        DataRace.Level = mapNames[raceValue];

        // Vehicle
	    for (int i = 0; i < VehicleControlers.Length; i++)
	    {
            int vehicleValue = VehicleControlers[i].value;

            if (_lastVehicles[i] != vehicleNames[vehicleValue])
	        {
                Destroy(_currentVehicles[i]);

                _currentVehicles[i] =
	                Instantiate(Resources.Load("Vehicles/Models/car_" + vehicleNames[vehicleValue].ToLower())) as
	                    GameObject;
                if (_currentVehicles[i] == null)
	                throw new NullReferenceException();
                _currentVehicles[i].transform.SetParent(VehicleTransforms[i], false);
                Destroy(_currentVehicles[i].GetComponentInChildren<ReactorBehaviour>());
	        }
            _lastVehicles[i] = vehicleNames[vehicleValue];
            DataRace.Models[i] = vehicleNames[vehicleValue];
	    }

        // NumberVehicles
        int numberVehicleValue = NumberVehicleControler.value;
        CounterVehicles.text = "" + numberVehicles[numberVehicleValue];
        DataRace.VehicleCount = numberVehicles[numberVehicleValue];

        // NumberPlayers

        int numberPlayerValue = NumberPlayerControler.value;
        if (numberPlayerValue > numberVehicleValue)
        {
            NumberPlayerControler.value = numberVehicleValue;
            numberPlayerValue = numberVehicleValue;
        }

		CounterPlayers.text = "" + numberPlayers [numberPlayerValue];
		DataRace.PlayerCount = numberPlayers [numberPlayerValue];

        // Laps
        int numberTurnValue = NumberTurnControler.value;
		CounterTurn.text = "" + numberTurns [numberTurnValue];
		DataRace.Laps = numberTurns [numberTurnValue];
	}

    public void LaunchRace()
    {
        Application.LoadLevel(DataRace.Level);
    }
}
