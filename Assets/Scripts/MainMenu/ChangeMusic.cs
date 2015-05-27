using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

	public AudioClip level3music;
	public AudioClip level4music;
	public AudioClip level5music;

	private AudioSource source;
	private AudioClip level0music;

	void Awake () 
	{
		source = GetComponent<AudioSource> ();
		level0music = source.clip;
	}

	void OnLevelWasLoaded (int level)
	{
		if(level == 0) {
			source.clip = level0music;
			source.Play();
		}
		if(level == 3) {
			source.clip = level3music;
			source.Play();
		}
		if (level == 4) 
		{
			source.clip = level4music;
			source.Play();
		}

		if(level == 5) {
			source.clip = level5music;
			source.Play();
		}
	}
}
