using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public GameData gameData;


    private string apiUrl = "http://localhost:8080/api/game/data/";
    public void LoadGameData(int userId)
    {
        StartCoroutine(GetGameDataCoroutine(userId));
    }

    private IEnumerator GetGameDataCoroutine(int userId)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + userId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            GameData data = JsonUtility.FromJson<GameData>(json);
            gameData.userId = data.userId;
            gameData.starLevels = data.starLevels;
            gameData.starLevelsSavedCount = data.starLevelsSavedCount;

            Debug.Log("User ID: " + data.userId);
            Debug.Log("Star Levels: " + string.Join(",", data.starLevels));
            Debug.Log("Saved Count: " + data.starLevelsSavedCount);

            // 이제 data.starLevels와 data.starLevelsSavedCount를 게임에 적용 가능
        }
        else
        {
            Debug.LogError("Error fetching game data: " + request.error);
        }
    }




    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // gameData 초기화
            if (gameData == null)
            {
                gameData = new GameData();
                gameData.starLevels = new int[4]; // 예: 씬 4개라면
            }

            // 커맨드라인에서 userId 읽기
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && int.TryParse(args[1], out int userId))
            {
                gameData.userId = userId;
                Debug.Log("서버에서 전달받은 userId: " + userId);
            }
            else
            {
                gameData.userId = 0; // 기본값
                Debug.Log("userId가 전달되지 않았습니다.");
            }

            LoadGameData(gameData.userId);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            return gameData.starLevels[sceneIndex];
        return 0;
    }

    public IEnumerator UploadGameData()
    {
        Debug.Log("Update start");
        string url = "http://localhost:8080/game/save";

        string json = JsonUtility.ToJson(gameData);

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
