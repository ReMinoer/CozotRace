using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

	public AudioClip level1music;
	public AudioClip level2music;
	public AudioClip level3music;

	private AudioSource source;

	void Awake () 
	{
		source = GetComponent<AudioSource> ();
	}

	void OnLevelWasLoaded (int level)
	{
		if(level == 1) {
			source.clip = level1music;
			source.Play();
		}
		if (level == 2) 
		{
			source.clip = level2music;
			source.Play();
		}

		if(level == 3) {
			source.clip = level3music;
			source.Play();
		}
	}
}
