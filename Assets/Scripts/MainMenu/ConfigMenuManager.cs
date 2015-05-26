using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {

	public GameObject[] PanelList;

	public string[] mapNames = {"EasyPlain", "GrassHills", "SpringLoop"};
	public string[] vehiclesName;
	public int[] numberPlayers = {1, 2, 3, 4};
	public int[] numberVehicles = {2, 3, 4, 5, 6, 7 ,8};

	void start() {
		Debug.Log ("Piou");
	}

	void update() {
		Debug.Log("piou");
		for (int i= 0; i < PanelList.Length; i++) 
		{
			PanelList[i].GetComponentInChildren<Text>().text = "" + PanelList[i].GetComponentInChildren<Controler>().value;
		}
	}
}
