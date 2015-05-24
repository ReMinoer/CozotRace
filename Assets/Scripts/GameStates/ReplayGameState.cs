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
        GameManager.Resume();
    }

    public override void Update()
    {
    }
}
