﻿using UnityEngine;
using System.Collections;

public class IntroGameState : GameState
{
	public IntroGameState(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void Init()
    {
        GameManager.Pause();
    }

    public override void Update()
    {
        GameManager.ChangeStateDiffered(new CountdownGameState(GameManager));
    }
}
