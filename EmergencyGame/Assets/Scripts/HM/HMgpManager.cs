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
    public GameObject Playerfeb;
    public Transform StartGate;

    public bool isClearing = false;  // 중복 실행 방지
    private bool isPlayerSpawned = false; //플레이어 중복 소환 방지
    public bool isStart = false;

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
        isClearing = false;
        // 항상 isClearing 초기화

        // player 참조 항상 최신으로 갱신
        if (Playerfeb != null)
        {
            
            if (gameState == null && !isPlayerSpawned && scene.name == "HM")
            {

                if (Playerfeb != null && StartGate != null)
                {
                    
                    GameObject spawnedPlayer = Instantiate(Playerfeb, StartGate.position, Quaternion.identity);
                    spawnedPlayer.tag = "Player"; // 필요하면 태그 부여
                    player = spawnedPlayer.transform; // 최신 참조 연결
                    isPlayerSpawned = true;

                    Debug.Log("플레이어 새로 소환 완료: " + player.position);
                }
            }


            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log("player 재연결 성공: " + player);
            }
            else
            {
                Debug.LogWarning("Player 태그 오브젝트 없음!");
                return;
            }
           
        }

        // 기존 스폰 위치 이동 로직
        if (scene.name == "HMGamePlaying")
        {
            GameObject gate = GameObject.FindWithTag("Gate");
            if (gate != null)
            {
                player.position = gate.transform.position;
                Debug.Log("플레이어 스폰 :" + player.position);
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
        if (gameState == null && !isClearing && !isStart)
        {
            // 현재 씬에서 Gate 찾기
            GameObject gate = GameObject.FindWithTag("Gate");
            if (gate != null)
            {
                // StartGate 갱신
                StartGate = gate.transform;
                Debug.Log(gate.transform);
                // 플레이어 좌표 이동
                player.position = StartGate.position;
                Debug.Log(player.position);
            }
            else
            {
                Debug.LogWarning("현재 씬에는 StartGate 가 없습니다!");
            }
            player.position = StartGate.position;
            isStart = true;
        }


        GameObject endGateObj = GameObject.FindWithTag("EndGate");
        GameObject Enemy = GameObject.FindWithTag("Enemy");

        if (endGateObj != null && player != null && Enemy == null)
        {
            float distance = Vector3.Distance(player.position, endGateObj.transform.position);

            Debug.Log("거리 :"+distance);
            //Debug.Log("플레이어 위치 :"+ player.position);
            //Debug.Log("EndGate pos = " + endGateObj.transform.position);
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
