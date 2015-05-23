using UnityEngine;
using System.Collections;

public class ReplayGameState : GameState
{
    public ReplayGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();
    }

    public override void Update()
    {
    }
}
