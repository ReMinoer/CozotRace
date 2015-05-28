using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DesignPattern;

public class IntroGameState : GameState
{
	public IntroGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
		GameManager.Pause();
		
		GameManager.AudioMessageSource.clip = GameManager.CountdownSound;
		GameManager.AudioMessageSource.Play();

        var replayManager = Object.FindObjectOfType<ReplayManager>();
        if (replayManager != null)
        {
            int j = 0;
            foreach (ReplayRecorder replayRecorder in replayManager.Recorders.Values)
            {
                var data = new ReplayVehicleData
                {
                    Model = replayManager.VehiclesData[j].Model,
                    DrivingStates = replayRecorder.ListDState
                };
                GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[j]);
                GameManager.AddContestant(gameObject);
                j++;
            }

            Object.Destroy(replayManager.gameObject.GetComponent<DontDestroyOnLoad>());
            Object.Destroy(replayManager.gameObject);

            ReplayCameraSystem replayCameraSystem = Factory<ReplayCameraSystem>.New("Cameras/ReplayCamera");
            replayCameraSystem.Target = GameManager.Contestants[0].transform;
            GameManager.MultiplayerAudioListener.Cameras.Add(replayCameraSystem.gameObject.GetComponent<Camera>());

            return;
        }

        var replayManagerObject = new GameObject("ReplayManager");
        replayManagerObject.AddComponent<DontDestroyOnLoad>();
        replayManager = replayManagerObject.AddComponent<ReplayManager>();

        int i = 0;

        string[] aiNames = GenerateAiNames(GameManager.AisData.Count);

        int aiCount = 0;
        foreach (AiVehicleData data in GameManager.AisData)
        {
            GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[i]);
            gameObject.GetComponent<Contestant>().PlayerName = aiNames[aiCount];
            GameManager.AddContestant(gameObject);
            replayManager.AddRecorder(gameObject.GetComponent<VehicleMotor>());
            replayManager.VehiclesData.Add(data);
            aiCount++;
            i++;
        }

        int playerCount = 0;
        foreach (PlayerVehicleData data in GameManager.PlayersData)
        {
            GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[i]);
            gameObject.GetComponent<Contestant>().PlayerName = "Player " + (playerCount + 1);
            GameManager.AddContestant(gameObject);
            replayManager.AddRecorder(gameObject.GetComponent<VehicleMotor>());
            replayManager.VehiclesData.Add(data);
            playerCount++;
            i++;
        }

        foreach (Camera camera in Object.FindObjectsOfType<Camera>())
            GameManager.MultiplayerAudioListener.Cameras.Add(camera);

        CameraManager.Instance.Start();
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new CountdownGameState(GameManager));
    }

    static private string[] AiNamesCollection =
    {
        "Rémi", "Frédéric", "Tristan", "Maxime", "Valentin", "Océane", "Johan",
        "Clément", "Lacarte", "Daniel", "Florent", "Danchi", "Paul", "Adrien"
    };

    private static string[] GenerateAiNames(int count)
    {
        string[] result = new string[count];
        int[] indexes = new int[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, AiNamesCollection.Length);
            } while (indexes.Contains(randomIndex));
            indexes[i] = randomIndex;
            result[i] = AiNamesCollection[randomIndex];
        }

        return result;
    }
}
