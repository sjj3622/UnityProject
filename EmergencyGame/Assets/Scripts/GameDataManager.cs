using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    //public GameData gameData = new GameData(); // GameData를 싱글톤에서 사용
    public GameData gameData;


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


    private void Start()
    {
        // 게임 시작 시 세션 확인 → toss → 나중에 게임 종료 시 Upload 준비
        StartCoroutine(InitializeGame());
    }

    // 1️⃣ 게임 시작 초기화 코루틴 (모든 시작용 코루틴 연결)
    private IEnumerator InitializeGame()
    {
        // 1. 세션에서 userId 가져오기
        yield return StartCoroutine(GetUserIdFromSession());

        // 2. Toss 호출 (userId 전달)
        yield return StartCoroutine(TossUserId(sessionUserId));

        // 이제 게임 진행 가능, 게임 종료 후 UploadGameData() 호출
    }

    private string sessionUserId; // 세션에서 가져온 userId 저장






    public IEnumerator GetUserIdFromSession()
    {
        string url = "http://localhost:8080/game/save";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.Log("세션 확인 실패: " + request.error);
            }
            else
            {
                string userId = request.downloadHandler.text;
                Debug.Log("세션 userId: " + userId);
                // 이후 game/toss 호출 가능
                StartCoroutine(TossUserId(userId));
            }
        }
    }

    // 2️⃣ game/toss 호출 (단순 userId 전달)
    private IEnumerator TossUserId(string userId)
    {
        string url = "http://localhost:8080/game/toss";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.Log("Toss 실패: " + request.error);
            }
            else
            {
                Debug.Log("Toss 성공, userId: " + request.downloadHandler.text);
                // 이제 게임 종료 후 loadGameData 호출 가능
            }
        }
    }

    // 3️⃣ 게임 종료 후 실제 데이터 서버에 저장 (/game/load)
    public IEnumerator UploadGameData()
    {
        string url = "http://localhost:8080/game/load";

        string json = JsonConvert.SerializeObject(gameData);

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
                Debug.Log("게임 데이터 전송 실패: " + request.error);
            }
            else
            {
                Debug.Log("게임 데이터 전송 성공: " + request.downloadHandler.text);
            }
        }

        Debug.Log("전송 JSON: " + json);
    }



}