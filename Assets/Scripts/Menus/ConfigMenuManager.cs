using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour
{
    public RaceData RaceData;
    public Controler NumberPlayerControler;
    public Controler NumberLapsControler;
    public Controler NumberVehicleControler;
    public Controler RaceControler;
    public Controler[] VehicleControlers = new Controler[4];
    public Transform RaceTransform;
    public Transform[] VehicleTransforms = new Transform[4];
    public Text RaceName;
    public Text CounterPlayers;
    public Text CounterLaps;
    public Text CounterVehicles;
    private readonly GameObject[] _currentVehicles = new GameObject[4];
    private readonly string[] _lastVehicles = new string[4];
    private readonly string[] _mapNames = {"EasyPlain", "GrassHills", "SpringLoop", "WindyMounts"};
    private readonly int[] _numberPlayers = {1, 2, 3, 4};
    private readonly int[] _numberLaps = {1, 2, 3, 4, 5};
    private readonly int[] _numberVehicles = {1, 2, 3, 4, 5, 6, 7, 8};
    private GameObject _currentTerrain;
    private string _lastRace;

    static public readonly string[] VehicleNames =
    {
        "car_blue", "car_cyan", "car_green", "car_lightGreen", "car_orange",
        "car_pink", "car_purple", "car_yellow"
    };

    public void LaunchRace()
    {
        Application.LoadLevel(RaceData.Level);
    }

    private void Awake()
    {
        RaceControler.ValueMax = _mapNames.Length - 1;
        foreach (Controler controler in VehicleControlers)
            controler.ValueMax = VehicleNames.Length - 1;
        NumberVehicleControler.ValueMax = _numberVehicles.Length - 1;
        NumberPlayerControler.ValueMax = _numberPlayers.Length - 1;
        NumberLapsControler.ValueMax = _numberLaps.Length - 1;
    }

    private void Update()
    {
        // Race
        int raceValue = RaceControler.Value;
        RaceName.text = _mapNames[raceValue];

        if (_lastRace != _mapNames[raceValue])
        {
            Destroy(_currentTerrain);

            _currentTerrain = Instantiate(Resources.Load("Terrains/" + _mapNames[raceValue])) as GameObject;
            if (_currentTerrain == null)
                throw new NullReferenceException();
            _currentTerrain.transform.SetParent(RaceTransform, false);
            _currentTerrain.transform.Translate(-_currentTerrain.GetComponent<Terrain>().terrainData.size / 2);
        }
        _lastRace = _mapNames[raceValue];

        RaceData.Level = _mapNames[raceValue];

        // Vehicle
        for (int i = 0; i < VehicleControlers.Length; i++)
        {
            int vehicleValue = VehicleControlers[i].Value;

            if (_lastVehicles[i] != VehicleNames[vehicleValue])
            {
                Destroy(_currentVehicles[i]);

                _currentVehicles[i] =
                    Instantiate(Resources.Load("Vehicles/Models/" + VehicleNames[vehicleValue].ToLower())) as
                        GameObject;
                if (_currentVehicles[i] == null)
                    throw new NullReferenceException();
                _currentVehicles[i].transform.SetParent(VehicleTransforms[i], false);
                Destroy(_currentVehicles[i].GetComponentInChildren<ReactorBehaviour>());
            }
            _lastVehicles[i] = VehicleNames[vehicleValue];
            RaceData.Models[i] = VehicleNames[vehicleValue];
        }

        // NumberVehicles
        int numberVehicleValue = NumberVehicleControler.Value;
        CounterVehicles.text = "" + _numberVehicles[numberVehicleValue];
        RaceData.VehicleCount = _numberVehicles[numberVehicleValue];

        // NumberPlayers
        int numberPlayerValue = NumberPlayerControler.Value;
        if (_numberPlayers[numberPlayerValue] > _numberVehicles[numberVehicleValue])
        {
            NumberPlayerControler.Value = numberVehicleValue;
            numberPlayerValue = numberVehicleValue;
        }

        if (_numberPlayers[numberPlayerValue] > PlayerInput.GetNumberOfGamepads() + 2)
        {
            NumberPlayerControler.Value = PlayerInput.GetNumberOfGamepads() + 1;
            numberPlayerValue = PlayerInput.GetNumberOfGamepads() + 1;
        }

        CounterPlayers.text = "" + _numberPlayers[numberPlayerValue];
        RaceData.PlayerCount = _numberPlayers[numberPlayerValue];

        // Laps
        int numberTurnValue = NumberLapsControler.Value;
        CounterLaps.text = "" + _numberLaps[numberTurnValue];
        RaceData.Laps = _numberLaps[numberTurnValue];
    }
}