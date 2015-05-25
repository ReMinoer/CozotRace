using UnityEngine;
using System.Collections;

public class FinishedGameState : GameState
{
    public FinishedGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Resume();

        GameManager.PauseUi.gameObject.SetActive(false);
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new ReplayGameState(GameManager));
    }
}
