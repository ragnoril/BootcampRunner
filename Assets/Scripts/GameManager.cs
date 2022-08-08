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

    public EventManager Events;
    public PlayerAgent Player;

    public BossAgent Boss;

    public GameObject FollowerPrefab;

    public Transform LevelPlayerFightStartPoint;


    private void Start()
    {
        Events.GameStarted();      
        
    }


}
