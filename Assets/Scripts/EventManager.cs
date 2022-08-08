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

    public Action<int> OnBossAttacked;
    public Action<FollowerAgent, int> OnFollowerAttacked;
    public Action<int> OnPlayerAttacked;

    
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

    public void BossAttacked(int damage)
    {
        //Debug.Log("boss attacked: " + damage);
        OnBossAttacked?.Invoke(damage);
    }

    public void FollowerAttacked(FollowerAgent follower, int damage)
    {
        //Debug.Log("follower attacked: " + damage);
        OnFollowerAttacked?.Invoke(follower, damage);
    }

    public void PlayerAttacked(int damage)
    {
        //Debug.Log("player attacked: " + damage);
        OnPlayerAttacked?.Invoke(damage);
    }

}
