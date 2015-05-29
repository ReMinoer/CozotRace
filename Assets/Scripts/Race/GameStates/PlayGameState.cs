using UnityEngine;

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
        GameManager.Contestants.Sort(new GameManager.PositionComparer());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.AllPlayerFinish || Object.FindObjectOfType<ReplayCameraSystem>() != null)
                GameManager.ChangeStateDiffered(new FinishedGameState(GameManager));
            else
                GameManager.ChangeStateDiffered(new PauseGameState(GameManager));
        }
    }
}