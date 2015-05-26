using UnityEngine;
using System.Collections;

public class Controler : MonoBehaviour {

	public int value;
	public int valueMax;

	public void NextSelection() {
		value += 1;
		if (value > valueMax)
			value = valueMax;
	}
	
	public void PreviousSelection() {
		value -= 1;
		if (value < 0)
			value = 0;
	}
}
