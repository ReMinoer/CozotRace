using System;
using UnityEngine;
using System.Collections.Generic;
using DesignPattern;

public class GameManager : DesignPattern.Singleton<GameManager>
{
    public GameState State;

    public List<PlayerVehicleData> PlayersData = new List<PlayerVehicleData>();
    public List<AiVehicleData> AisData = new List<AiVehicleData>();
    public StartGrid StartGrid;
    public CameraManager CameraManager;

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

    public List<Contestant> FinishedContestants { get; private set; }
    private int _finishPlayerCount;

    private bool _differedChangeStateRequest;
    private GameState _stateRequested;

    protected GameManager()
    {
        Contestants = new List<GameObject>();
        FinishedContestants = new List<Contestant>();
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

    public void EndRace(Contestant contestant)
    {
        PlayerInput playerInput = contestant.gameObject.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            _finishPlayerCount++;

            Destroy(playerInput);
            contestant.gameObject.AddComponent<AiInput>();

            var raceUiManager = contestant.gameObject.GetComponentInChildren<RaceUiManager>();
            if (raceUiManager != null)
            {
                Destroy(raceUiManager.gameObject);
                EndRaceUiManager ui = Factory<EndRaceUiManager>.New("Ui/EndRaceUi");
                ui.VehicleNumber = Contestants.Count;
                ui.GetComponent<Canvas>().worldCamera = contestant.gameObject.GetComponentInChildren<Camera>();
                ui.gameObject.transform.SetParent(contestant.gameObject.transform, false);

                foreach (Contestant finishedContestant in FinishedContestants)
                    ui.AddToRanking(finishedContestant);
            }

            if (_finishPlayerCount >= PlayersData.Count)
                ChangeStateDiffered(new FinishedGameState(this));
        }

        if (!FinishedContestants.Contains(contestant))
        {
            FinishedContestants.Add(contestant);

            foreach (GameObject o in Contestants)
            {
                var endRaceUiManager = o.GetComponentInChildren<EndRaceUiManager>();
                if (endRaceUiManager != null)
                    endRaceUiManager.AddToRanking(contestant);
            }
        }
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
