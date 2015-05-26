using UnityEngine;
using System.Collections;
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

        var replayManager = Object.FindObjectOfType<ReplayManager>();
        if (replayManager != null)
        {
            int j = 0;
            foreach (ReplayRecorder replayRecorder in replayManager.Recorders.Values)
            {
                var data = new ReplayVehicleData
                {
                    DrivingStates = replayRecorder.ListDState
                };
                GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[j]);
                GameManager.AddContestant(gameObject);
                j++;
            }

            Object.Destroy(replayManager.gameObject.GetComponent<DontDestroyOnLoad>());
            Object.Destroy(replayManager.gameObject);

            var replayCameraSystem = Factory<ReplayCameraSystem>.New("Cameras/ReplayCamera");
            replayCameraSystem.Target = GameManager.Contestants[0].transform;
            GameManager.MultiplayerAudioListener.Cameras.Add(replayCameraSystem.gameObject.GetComponent<Camera>());

            return;
        }

        var replayManagerObject = new GameObject("ReplayManager");
        replayManagerObject.AddComponent<DontDestroyOnLoad>();
        replayManager = replayManagerObject.AddComponent<ReplayManager>();

        int i = 0;

        int aiCount = 0;
        foreach (AiVehicleData data in GameManager.AisData)
        {
            GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[i]);
            gameObject.GetComponent<Contestant>().PlayerName = "AI n°" + (aiCount + 1);
            GameManager.AddContestant(gameObject);
            replayManager.AddRecorder(gameObject.GetComponent<VehicleMotor>());
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
}
