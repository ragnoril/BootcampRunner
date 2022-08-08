using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> Levels;
    public string[] LevelNames;

    public Level CurrentLevel;
    public int CurrentLevelId;

    public void StartLevelManager()
    {
        CurrentLevelId = PlayerPrefs.GetInt("CurrentLevel", 0);

        RestartLevel();
    }

    public void RestartLevel()
    {
        if (CurrentLevel != null)
            Destroy(CurrentLevel.gameObject);


        //GameObject goLevel = Instantiate(Levels[CurrentLevelId], Vector3.zero, Quaternion.identity);
        var prefab = Resources.Load<GameObject>(LevelNames[CurrentLevelId]);
        GameObject goLevel = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        CurrentLevel = goLevel.GetComponent<Level>();

        GameManager.Instance.Events.LevelStarted();
    }

    public void GoToNextLevel()
    {
        CurrentLevelId += 1;        
        if (CurrentLevel != null)
            Destroy(CurrentLevel.gameObject);

        //GameObject goLevel = Instantiate(Levels[CurrentLevelId], Vector3.zero, Quaternion.identity);
        var prefab = Resources.Load<GameObject>(LevelNames[CurrentLevelId]);
        GameObject goLevel = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        CurrentLevel = goLevel.GetComponent<Level>();

        GameManager.Instance.Events.LevelStarted();
    }

}
