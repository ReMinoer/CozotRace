using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorNumberButton : MonoBehaviour {

	public Text _numberText;
	
	public void NextSelectionNumber() {
		int nextInt = int.Parse(_numberText.text) + 1;
		_numberText.text = "" + nextInt;

		if (int.Parse (_numberText.text) > 8)
			_numberText.text = "8";
	}
	
	public void PreviousSelectionNumber() {
		int previousInt = int.Parse(_numberText.text) - 1;
		_numberText.text = "" + previousInt;
		if (int.Parse (_numberText.text) < 2)
			_numberText.text = "2";
	}
}
