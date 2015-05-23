using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : DesignPattern.Singleton<GameManager>
{
    public GameState State;

    public List<PlayerVehicleData> PlayersData = new List<PlayerVehicleData>();
    public List<AiVehicleData> AisData = new List<AiVehicleData>();
    public StartGrid StartGrid;

    public List<GameObject> Contestants { get; private set; }

    public TimeSpan Countdown
    {
        get { return TimeSpan.FromSeconds(6 - _countdown); }
    }

    public TimeSpan Chronometer
    {
        get { return TimeSpan.FromSeconds(_chronometer); }
    }

    public bool CountdownEnabled { get; set; }
    public bool ChronometerEnabled { get; set; }

    private float _countdown;
    private float _chronometer;

    private bool _differedChangeStateRequest;
    private GameState _stateRequested;

    protected GameManager()
    {
        Contestants = new List<GameObject>();
    }

    void Awake()
    {
        State = new IntroGameState(this);
    }

    void Start()
    {
        State.Init();
		ResetChrono ();
    }

    void Update()
    {
        State.Update();

        if (CountdownEnabled)
            _countdown += Time.unscaledDeltaTime;

        if (ChronometerEnabled)
            _chronometer += Time.unscaledDeltaTime;

        Contestants.Sort(new PositionComparer());

        if (_differedChangeStateRequest)
        {
            ChangeState(_stateRequested);
            _differedChangeStateRequest = false;
        }
    }

    public void Resume()
    {
        ChronometerEnabled = true;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        ChronometerEnabled = false;
        Time.timeScale = 0;
    }

    public void AddContestant(GameObject contestant)
    {
        Contestants.Add(contestant);
    }

    public void ResetChrono()
    {
        _chronometer = 0;
    }

    public void ResetCountdown()
    {
        _countdown = 0;
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        State.Init();
    }

    public void ChangeStateDiffered(GameState newState)
    {
        _stateRequested = newState;
        _differedChangeStateRequest = true;
    }

    private class PositionComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            return
                - x.GetComponent<ProgressTracker>()
                    .ProgressDistance.CompareTo(y.GetComponent<ProgressTracker>().ProgressDistance);
        }
    }
}
