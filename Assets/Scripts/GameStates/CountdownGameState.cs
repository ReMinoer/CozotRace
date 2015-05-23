using UnityEngine;
using System.Collections;

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
        if (GameManager.Countdown.Ticks < 0)
        {
            GameManager.CountdownEnabled = false;
            GameManager.ChangeStateDiffered(new PlayGameState(GameManager));
        }
    }
}
