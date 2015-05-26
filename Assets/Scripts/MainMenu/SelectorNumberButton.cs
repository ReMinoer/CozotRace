using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorNumberButton : MonoBehaviour {

	public Text _numberText;
	
	public GameObject _dataRace;
	
	public void start() {
		int numberPlayer = int.Parse (_numberText.text);
		Debug.Log (numberPlayer);
		_dataRace.GetComponent<DataRace> ().numberPlayers = numberPlayer;
	}

	public void NextSelectionNumber() {
		int nextInt = int.Parse(_numberText.text) + 1;
		_numberText.text = "" + nextInt;

		if (int.Parse (_numberText.text) > 8) {
			_numberText.text = "8";
			_dataRace.GetComponent<DataRace> ().numberPlayers = nextInt - 1;
		} else {
			_dataRace.GetComponent<DataRace> ().numberPlayers = nextInt;
		}
	}
	
	public void PreviousSelectionNumber() {
		int previousInt = int.Parse(_numberText.text) - 1;
		_numberText.text = "" + previousInt;
		if (int.Parse (_numberText.text) < 2) {
			_numberText.text = "2";
			_dataRace.GetComponent<DataRace> ().numberPlayers = previousInt+1;
		} else {
			_dataRace.GetComponent<DataRace> ().numberPlayers = previousInt;
		}
	}
}
