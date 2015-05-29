using UnityEngine;

public class CountdownGameState : GameState
{
    public CountdownGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();

        GameManager.CountdownEnabled = true;
        GameManager.ResetCountdown();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var replayCameraSystem = Object.FindObjectOfType<ReplayCameraSystem>();
            if (replayCameraSystem != null)
                GameManager.ChangeStateDiffered(new FinishedGameState(GameManager));
        }

        if (GameManager.Countdown.Seconds < 1)
        {
            GameManager.CountdownEnabled = false;
            GameManager.ChangeStateDiffered(new PlayGameState(GameManager));
        }
    }
}