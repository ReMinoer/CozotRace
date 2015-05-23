using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorImageButton : MonoBehaviour {

	public Image _backgroundImage;
	public Sprite[] _backgroundSprites;
	public Text _raceTitle;

	public void NextSelectionImage() {
		int index = IndexCurrentImage();

		int nextIndex = (index + 1) % _backgroundSprites.Length;
		_backgroundImage.sprite = _backgroundSprites [nextIndex];

		_raceTitle.text = _backgroundImage.sprite.name;
	}

	public void PreviousSelectionImage() {
		int index = IndexCurrentImage();
		
		int nextIndex = 0;
		if (index-1 < 0)
			nextIndex = _backgroundSprites.Length - 1;
		else
			nextIndex = (index - 1) % _backgroundSprites.Length;

		_backgroundImage.sprite = _backgroundSprites [nextIndex];		
		_raceTitle.text = _backgroundImage.sprite.name;
	}

	int IndexCurrentImage() {
		int index = 0;
		int i = 0;
		while (i < _backgroundSprites.Length) {
			if (_backgroundImage.sprite == _backgroundSprites[i])
				index = i;
			i++;
		}
		
		return index;
	}
}
