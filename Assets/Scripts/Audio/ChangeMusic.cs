using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public AudioClip Level2Music;
    public AudioClip Level3Music;
    public AudioClip Level4Music;
    public AudioClip Level5Music;
    private AudioClip _level0Music;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _level0Music = _source.clip;
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                _source.clip = _level0Music;
                break;
            case 2:
                _source.clip = Level2Music;
                break;
            case 3:
                _source.clip = Level3Music;
                break;
            case 4:
                _source.clip = Level4Music;
                break;
            case 5:
                _source.clip = Level5Music;
                break;
            default:
                return;
        }

        _source.Play();
    }
}