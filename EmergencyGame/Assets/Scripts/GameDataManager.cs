using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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


    public IEnumerator UploadGameData()
    {
        Debug.Log("저장시작");
        string url = "http://localhost:8080/game/save";

        string json = JsonConvert.SerializeObject(gameData); // 배열 포함 시 안정적

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.Log("전송 실패: " + request.error);
            }
            else
            {
                Debug.Log("전송 성공: " + request.downloadHandler.text);
            }
        }
        Debug.Log("저장끝");
    }

}