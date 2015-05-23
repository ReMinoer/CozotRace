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

        foreach (AiVehicleData data in GameManager.AisData)
        {
            GameObject gameObject = data.Instantiate();
            gameObject.transform.position = GameManager.StartGrid.StartPositions[i].position;
            gameObject.transform.rotation = GameManager.StartGrid.StartPositions[i].rotation;
            GameManager.AddContestant(gameObject);
            i++;
        }

        foreach (PlayerVehicleData data in GameManager.PlayersData)
        {
            GameObject gameObject = data.Instantiate();
            gameObject.transform.position = GameManager.StartGrid.StartPositions[i].position;
            gameObject.transform.rotation = GameManager.StartGrid.StartPositions[i].rotation;
            GameManager.AddContestant(gameObject);
            i++;
        }

    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new CountdownGameState(GameManager));
    }
}
