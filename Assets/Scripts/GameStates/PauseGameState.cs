using UnityEngine;
using System.Collections;

public class PauseGameState : GameState
{
	public PauseGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();

        GameManager.PauseUi.gameObject.SetActive(true);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.ChangeStateDiffered(new PlayGameState(GameManager));
        if (Input.GetKeyDown(KeyCode.Return))
            Application.LoadLevel(Application.loadedLevel);
    }
}
