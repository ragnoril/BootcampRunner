using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action OnGameStarted;

    public Action<FollowerAgent> OnFollowerGained;
    public Action<FollowerAgent> OnFollowerKilled;
    public Action<GateAgent> OnPlayerPassThruGate;

    public Action OnLevelStarted;
    public Action<bool> OnLevelFinished;
    public Action OnFightSetup;
    public Action OnFightStarted;



    public void GameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public void FollowerGained(FollowerAgent follower)
    {
        OnFollowerGained?.Invoke(follower);
    }

    public void FollowerKilled(FollowerAgent follower)
    {
        OnFollowerKilled?.Invoke(follower);
    }

    public void PlayerPassThruGate(GateAgent gate)
    {
        OnPlayerPassThruGate?.Invoke(gate);
    }

    public void LevelStarted()
    {
        OnLevelStarted?.Invoke();
    }

    public void LevelFinished(bool hasPlayerWon)
    {
        OnLevelFinished?.Invoke(hasPlayerWon);
    }

    public void FightSetup()
    {
        OnFightSetup?.Invoke();
    }

    public void FightStarted()
    {
        OnFightStarted?.Invoke();
    }

}
