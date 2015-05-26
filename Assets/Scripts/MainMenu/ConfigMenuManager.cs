using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {

	public GameObject DataRace;

	public RectTransform RaceControler;
	public Image RaceChoiceImage;
	public Sprite[] RaceImages;
	public Text RaceName;
	public string[] mapNames = {"EasyPlain", "GrassHills", "SpringLoop"};

	public RectTransform VehicleControler;
	public Image VehicleChoiceImage;
	public Sprite[] VehicleImages;
	public Text VehicleName;
	public string[] vehicleNames;

	public RectTransform NumberPlayerControler;
	public Text CounterPlayers;
	public int[] numberPlayers = {1, 2, 3, 4};

	
	public RectTransform NumberVehicleControler;
	public Text CounterVehicles;
	public int[] numberVehicles = {2, 3, 4, 5, 6, 7 ,8};

	public RectTransform NumberTurnControler;
	public Text CounterTurn;
	public int[] numberTurns = {1, 2, 3, 4};

	void Update() {
		int _raceValue = RaceControler.GetComponent<Controler>().value;
		RaceChoiceImage.sprite = RaceImages [_raceValue];
		RaceName.text = mapNames [_raceValue];
		DataRace.GetComponent<DataRace> ().sceneToLoad = _raceValue + 2;
		
		int _vehicleValue = VehicleControler.GetComponent<Controler> ().value;
		VehicleChoiceImage.sprite = VehicleImages [_vehicleValue];
		VehicleName.text = vehicleNames [_vehicleValue];
		DataRace.GetComponent<DataRace> ().vehiclePlayer = _vehicleValue;

		int _numberPlayerValue = NumberPlayerControler.GetComponent<Controler> ().value;
		CounterPlayers.text = "" + numberPlayers [_numberPlayerValue];
		DataRace.GetComponent<DataRace> ().numberPlayers = numberPlayers [_numberPlayerValue];
		
		int _numberVehicleValue = NumberVehicleControler.GetComponent<Controler> ().value;
		CounterVehicles.text = "" + numberVehicles [_numberVehicleValue];
		DataRace.GetComponent<DataRace> ().numberVehicles = numberVehicles [_numberVehicleValue];
		
		int _numberTurnValue = NumberTurnControler.GetComponent<Controler> ().value;
		CounterTurn.text = "" + numberTurns [_numberTurnValue];
		DataRace.GetComponent<DataRace> ().numberTurns = numberTurns [_numberTurnValue];

	}
}
