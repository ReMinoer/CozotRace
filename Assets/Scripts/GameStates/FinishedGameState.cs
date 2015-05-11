﻿using UnityEngine;
using System.Collections;

public class FinishedGameState : GameState
{
    public FinishedGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new ReplayGameState(GameManager));
    }
}