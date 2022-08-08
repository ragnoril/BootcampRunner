using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public LevelManager Levels;

    public EventManager Events;
    public PlayerAgent Player;

    public BossAgent Boss;

    public GameObject FollowerPrefab;

    public GameObject PlayerWonPanel;
    public GameObject PlayerLostPanel;


    private void Start()
    {
        Events.OnLevelFinished += EndOfLevel;
        Events.OnLevelStarted += StartOfLevel;

        Levels.StartLevelManager();
        Player.LevelStarted();
    }

    private void OnDestroy()
    {
        Events.OnLevelFinished -= EndOfLevel;
        Events.OnLevelStarted -= StartOfLevel;
    }

    private void StartOfLevel()
    {
        PlayerLostPanel.SetActive(false);
        PlayerWonPanel.SetActive(false);
    }

    private void EndOfLevel(bool state)
    {
        PlayerLostPanel.SetActive(!state);
        PlayerWonPanel.SetActive(state);
    }

    public void NextLevel()
    {
        Levels.GoToNextLevel();
    }

    public void PlayAgain()
    {
        Levels.RestartLevel();
    }

}
