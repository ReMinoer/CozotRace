using UnityEngine;
using System.Collections;

public class Controler : MonoBehaviour {

	public int value;
	public int valueMax;

	public void NextSelection() {
		value += 1;
		if (value > valueMax)
			value = 0;
	}
	
	public void PreviousSelection() {
		value -= 1;
		if (value < 0)
			value = valueMax;
	}
}
