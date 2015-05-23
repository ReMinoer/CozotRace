using UnityEngine;
using DesignPattern;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraManager : Singleton<CameraManager> {

	public List<RaceUiManager> RaceUiManagers;

    public const float VirtualWidth = 1920;
    public const float VirtualHeight = 1080;

	void Start ()
	{
        if (RaceUiManagers.Count == 1)
		{
            RaceUiManagers[0].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth, VirtualHeight);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0, 1, 1);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.depth = 1;
		}
        if (RaceUiManagers.Count == 2)
        {
            RaceUiManagers[0].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth, VirtualHeight * 2);
            RaceUiManagers[1].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth, VirtualHeight * 2);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0.5f, 1, 0.5f);
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0, 1, 0.5f);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.depth = 1;
		}
        if (RaceUiManagers.Count == 3)
        {
            RaceUiManagers[0].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[1].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[2].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth, VirtualHeight * 2);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            RaceUiManagers[2].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0, 1, 0.5f);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[2].GetComponent<Canvas>().worldCamera.depth = 1;
		}
        if (RaceUiManagers.Count == 4)
        {
            RaceUiManagers[0].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[1].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[2].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[3].GetComponent<CanvasScaler>().referenceResolution = new Vector2(VirtualWidth * 2, VirtualHeight * 2);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            RaceUiManagers[2].GetComponent<Canvas>().worldCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
            RaceUiManagers[3].GetComponent<Canvas>().worldCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            RaceUiManagers[0].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[1].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[2].GetComponent<Canvas>().worldCamera.depth = 1;
            RaceUiManagers[3].GetComponent<Canvas>().worldCamera.depth = 1;
		}
	}

	void Update ()
	{
	
	}

	void OnValidate()
	{
        if (RaceUiManagers.Count > 4)
		{
            RaceUiManagers.RemoveRange(4, RaceUiManagers.Count - 4);
			Debug.LogError ("You can't have more than 4 RaceUiManagers.");
		}
	}
}
