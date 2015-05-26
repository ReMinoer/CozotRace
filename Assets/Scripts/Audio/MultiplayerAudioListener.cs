using UnityEngine;
using System.Collections.Generic;

public class MultiplayerAudioListener : MonoBehaviour
{
    public List<Camera> Cameras;
	
	void Update ()
	{
	    if (Cameras.Count == 0)
	        return;

	    Vector3 position = Vector3.zero;
	    foreach (Camera cam in Cameras)
            position += cam.transform.position;

        transform.position = position / Cameras.Count;
	}
}
