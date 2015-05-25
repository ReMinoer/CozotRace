using UnityEngine;
using System.Collections;

public class PlayGameState : GameState
{
	public PlayGameState(GameManager gameManager)
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
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.ChangeStateDiffered(new PauseGameState(GameManager));
    }
}
