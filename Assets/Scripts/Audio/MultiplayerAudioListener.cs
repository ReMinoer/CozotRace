using System.Collections.Generic;
using UnityEngine;

public class MultiplayerAudioListener : MonoBehaviour
{
    public List<Camera> Cameras;

    private void Update()
    {
        if (Cameras.Count == 0)
            return;

        Vector3 position = Vector3.zero;
        foreach (Camera cam in Cameras)
            position += cam.transform.position;

        transform.position = position / Cameras.Count;
    }
}