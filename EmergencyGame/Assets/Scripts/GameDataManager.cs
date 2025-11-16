using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public GameData gameData = new GameData(); // GameData를 싱글톤에서 사용

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 타이틀 씬에서 Instance가 없을 때 생성
    public static void EnsureExists()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("GameDataManager");
            obj.AddComponent<GameDataManager>();
        }
    }

    public void SetStar(int sceneIndex, int stars)
    {
        if (sceneIndex >= 0 && sceneIndex < gameData.starLevels.Length)
        {
            gameData.starLevels[sceneIndex] = stars;
            if (sceneIndex >= gameData.starLevelsSavedCount)
                gameData.starLevelsSavedCount = sceneIndex + 1;
        }
    }

    public int GetStar(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < gameData.starLevels.Length)
        {
            return gameData.starLevels[sceneIndex];
        }
        return 0;
    }

    internal void SetStar(int sceneIndex, object starCount)
    {
        throw new NotImplementedException();
    }
}
