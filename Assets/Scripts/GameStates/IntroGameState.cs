using UnityEngine;
using System.Collections;

public class IntroGameState : GameState
{
	public IntroGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();

        int i = 0;

        int aiCount = 0;
        foreach (AiVehicleData data in GameManager.AisData)
        {
            GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[i]);
            gameObject.GetComponent<Contestant>().PlayerName = "AI n°" + (aiCount + 1);
            GameManager.AddContestant(gameObject);
            aiCount++;
            i++;
        }

        int playerCount = 0;
        foreach (PlayerVehicleData data in GameManager.PlayersData)
        {
            GameObject gameObject = data.Instantiate(GameManager.StartGrid.StartPositions[i]);
            gameObject.GetComponent<Contestant>().PlayerName = "Player " + (playerCount + 1);
            GameManager.AddContestant(gameObject);
            playerCount++;
            i++;
        }

        CameraManager.Instance.Start();
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new CountdownGameState(GameManager));
    }
}
