using UnityEngine;
using DesignPattern;
using System.Collections.Generic;

public class CameraManager : Singleton<CameraManager> {

	public List<Camera> Cameras;

	void Start ()
	{
		if (Cameras.Count == 0)
			Cameras.Add (Camera.main);

		if (Cameras.Count == 1)
		{
			Cameras[0].rect = new Rect(0,0,1,1);
			Cameras[0].depth = 1;
		}
		if (Cameras.Count == 2)
		{
			Cameras[0].rect = new Rect(0,0.5f,1,0.5f);
			Cameras[1].rect = new Rect(0,0,1,0.5f);
			Cameras[0].depth = 1;
			Cameras[1].depth = 1;
		}
		if (Cameras.Count == 3)
		{
			Cameras[0].rect = new Rect(0,0.5f,0.5f,0.5f);
			Cameras[1].rect = new Rect(0.5f,0.5f,0.5f,0.5f);
			Cameras[2].rect = new Rect(0,0,1,0.5f);
			Cameras[0].depth = 1;
			Cameras[1].depth = 1;
			Cameras[2].depth = 1;
		}
		if (Cameras.Count == 4)
		{
			Cameras[0].rect = new Rect(0,0.5f,0.5f,0.5f);
			Cameras[1].rect = new Rect(0.5f,0.5f,0.5f,0.5f);
			Cameras[2].rect = new Rect(0,0,0.5f,0.5f);
			Cameras[3].rect = new Rect(0.5f,0,0.5f,0.5f);
			Cameras[0].depth = 1;
			Cameras[1].depth = 1;
			Cameras[2].depth = 1;
			Cameras[3].depth = 1;
		}
	}

	void Update ()
	{
	
	}

	void OnValidate()
	{
		if (Cameras.Count > 4)
		{
			Cameras.RemoveRange (4, Cameras.Count - 4);
			Debug.LogError ("You can't have more than 4 cameras.");
		}
	}
}
