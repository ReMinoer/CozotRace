﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DesignPattern;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : DesignPattern.Singleton<GameManager>
{
    public GameState State;
	
	public AudioSource AudioMessageSource;
	public AudioClip CountdownSound;
	public AudioClip FinishSound;

    public List<PlayerVehicleData> PlayersData = new List<PlayerVehicleData>();
    public List<AiVehicleData> AisData = new List<AiVehicleData>();
    public StartGrid StartGrid;
    public CameraManager CameraManager;
    public Canvas PauseUi;
    public MultiplayerAudioListener MultiplayerAudioListener;

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

    public bool AllPlayerFinish
    {
        get { return _finishPlayerCount >= PlayersData.Count; }
    }

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
        
        var dataRace = FindObjectOfType<DataRace>();
        if (dataRace != null)
        {
            PlayersData = new List<PlayerVehicleData>();
            for (int i = 0; i < dataRace.PlayerCount; i++)
            {
                PlayerIndex playerIndex = PlayerInput.GeneratePlayerIndex(i + 1);
                PlayersData.Add(new PlayerVehicleData {
                    Model = dataRace.Models[i],
                    PlayerIndex = playerIndex
                });
            }

            AisData = new List<AiVehicleData>();
            for (int i = 0; i < dataRace.VehicleCount - dataRace.PlayerCount; i++)
            {
                string randomModel;
                do
                {
                    randomModel = ConfigMenuManager.VehicleNames[Random.Range(0, ConfigMenuManager.VehicleNames.Length)];
                } while (PlayersData.Any(data => data.Model == randomModel));

                AisData.Add(new AiVehicleData
                {
                    Model = randomModel
                });
            }

            Race.Instance.Laps = dataRace.Laps;

            Destroy(dataRace.gameObject.GetComponent<DontDestroyOnLoad>());
            Destroy(dataRace.gameObject);
        }
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
            _chronometer += Time.deltaTime;

        var replayCameraSystem = FindObjectOfType<ReplayCameraSystem>();
        if (replayCameraSystem != null)
            replayCameraSystem.Target = Contestants[0].transform;

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

			AudioMessageSource.clip = FinishSound;
			AudioMessageSource.Play();

            Destroy(playerInput);
            contestant.gameObject.AddComponent<AiInput>();

            var raceUiManager = contestant.gameObject.GetComponentInChildren<RaceUiManager>();
            if (raceUiManager != null)
            {
                EndRaceUiManager ui = Factory<EndRaceUiManager>.New("Ui/EndRaceUi");
                ui.VehicleNumber = Contestants.Count;
                ui.GetComponent<Canvas>().worldCamera = raceUiManager.GetComponent<Canvas>().worldCamera;

                ui.GetComponent<CanvasScaler>().referenceResolution =
                    raceUiManager.GetComponent<CanvasScaler>().referenceResolution;

                Destroy(raceUiManager.gameObject);

                ui.gameObject.transform.SetParent(contestant.gameObject.transform, false);

                foreach (Contestant finishedContestant in FinishedContestants)
                    ui.AddToRanking(finishedContestant, contestant.SplitTimes.Count - 1);
            }
        }

        if (!FinishedContestants.Contains(contestant))
        {
            FinishedContestants.Add(contestant);

            foreach (GameObject o in Contestants)
            {
                var endRaceUiManager = o.GetComponentInChildren<EndRaceUiManager>();
                if (endRaceUiManager != null)
                    endRaceUiManager.AddToRanking(contestant, contestant.SplitTimes.Count - 1);
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

    public class PositionComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            return
                - x.GetComponent<ProgressTracker>()
                    .ProgressDistance.CompareTo(y.GetComponent<ProgressTracker>().ProgressDistance);
        }
    }
}
