using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMgpManager : MonoBehaviour
{
    public static string gameState = null;

    public Transform player; // 씬의 플레이어 오브젝트 연결

    void Awake()
    {
        // 씬 전환 후에도 파괴되지 않게
        DontDestroyOnLoad(this.gameObject);

        // 씬 전환 감지
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // HMGamePlaying 씬으로 이동했을 때
        if (scene.name == "HMGamePlaying" && player != null)
        {
            // Tag가 "Gate"인 오브젝트 찾기
            GameObject gate = GameObject.FindWithTag("Gate");
            if (gate != null)
            {
                // 플레이어 위치를 Gate 위치로 이동
                player.position = gate.transform.position;
            }
            else
            {
                Debug.LogWarning("Gate 오브젝트를 찾을 수 없습니다!");
            }
        }
    }

    void OnDestroy()
    {
        // 씬 전환 후 이벤트 중복 방지
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {

    }
}
