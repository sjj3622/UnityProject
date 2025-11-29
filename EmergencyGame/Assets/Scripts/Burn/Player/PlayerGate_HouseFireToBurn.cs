using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_HouseFireToBurn : MonoBehaviour
{
    [Header("Gate Settings")]
    public string burnGateName = "playermove1-1";    // Burn 씬 도착 게이트 이름
    public GameObject houseGate0;                    // HouseFire 씬에서 플레이어가 충돌하는 게이트

    [Header("Teleport Settings")]
    private float offsetX = -4f;
    public float cooldownTime = 1f;

    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();
    private GameObject playerToTeleport;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // 플레이어를 씬 전환 후에도 유지
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (cooldownSet.Contains(houseGate0)) return;

        playerToTeleport = col.gameObject;
        cooldownSet.Add(houseGate0);

        // 상태 저장(필요하다면)
        BurngpManager.gameState = "FireFighterClear"; // 필요 시 수정
        
        // 씬 로드 후 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Burn");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Burn") return;

        GameObject gateInBurn = GameObject.Find(burnGateName);

        if (gateInBurn != null && playerToTeleport != null)
        {
            Vector3 offset = new Vector3(offsetX, 0, 0);
            playerToTeleport.transform.position = gateInBurn.transform.position + offset;
            Debug.Log("Burn 이동: " + playerToTeleport.transform.position);
        }

        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 쿨다운 제거
        StartCoroutine(RemoveCooldown());
    }

    private IEnumerator RemoveCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownSet.Remove(houseGate0);
    }
}
