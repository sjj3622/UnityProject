using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMgpManager : MonoBehaviour
{
    private static string _gameState;

    public static string gameState
    {
        get => _gameState;
        set
        {
            Debug.Log($"gameState 변경: 이전값 = {_gameState}, 새값 = {value}\n스택: {System.Environment.StackTrace}");
            _gameState = value;
        }
    }

    public Transform player;
    private bool isClearing = false;  // 중복 실행 방지

    void Awake()
    {
        Debug.Log("HMgpManager Awake! gameState = " + gameState);
        if (FindObjectsOfType<HMgpManager>().Length > 1)
        {
            Destroy(gameObject);  // 이미 존재하면 자신을 삭제
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HMGamePlaying" && player != null)
        {
            GameObject gate = GameObject.FindWithTag("Gate");
            if (gate != null)
            {
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        GameObject endGateObj = GameObject.FindWithTag("EndGate");
        GameObject Enemy = GameObject.FindWithTag("Enemy");

       

        if (endGateObj != null && player != null && Enemy == null)
        {
            float distance = Vector3.Distance(player.position, endGateObj.transform.position);

            
            Debug.Log("EndGate pos = " + endGateObj.transform.position);
            if (distance < 1f && !isClearing)
            {
                Debug.Log("플레이어가 EndGate에 도착했습니다!");

                gameState = "HMClear";
                // 2초간 위로 상승 + 씬 이동 실행
                StartCoroutine(PlayerClearAction());
            }
        }
    }

    IEnumerator PlayerClearAction()
    {
        isClearing = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = 0;   // 🔥 중력 제거

        float duration = 2f;
        float elapsed = 0f;
        float riseSpeed = 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (player != null)
            {
                player.position += Vector3.up * riseSpeed * Time.deltaTime;
            }

            yield return null;
        }

        // 씬 이동
        SceneManager.LoadScene("HM");
    }

}
