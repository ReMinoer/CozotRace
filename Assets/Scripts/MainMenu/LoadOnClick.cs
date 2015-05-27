using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

	public GameObject _loadingImage;
	public Text _text;

	private string[] newText = {"", "EasyPlain", "GrassHills", "SpringLoop"};

	public void LoadScene(int level) 
	{
		Debug.Log (level);
		_loadingImage.SetActive (true);
		Application.LoadLevel (level);
		_text.text = newText[level-1];
	}
}
