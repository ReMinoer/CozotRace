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
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new PlayGameState(GameManager));
    }
}
