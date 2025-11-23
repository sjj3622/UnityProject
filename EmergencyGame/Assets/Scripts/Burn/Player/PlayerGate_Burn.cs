using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_Burn : MonoBehaviour
{
    [Header("Gate Settings")]
    public string houseFireGateName = "playermove1-2";    // HouseFire 씬 도착 게이트 이름
    public GameObject burnGate0;                          // Burn 씬에서 플레이어가 충돌하는 게이트
    public GameObject playerPrefab;

    [Header("Teleport Settings")]
    public float offsetX = 1f;
    public float cooldownTime = 1f;

    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();



    private void Awake()
    {
        // 플레이어를 씬 전환 후에도 유지
        GameObject player = GameObject.FindWithTag("Player");

        Debug.Log(player);
        if (player == null)
        {
            // 없으면 생성
            player = Instantiate(playerPrefab);
            player.tag = "Player"; // 태그 보장
        }

        DontDestroyOnLoad(player);
    }

    private void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (cooldownSet.Contains(burnGate0)) return;
        if (col.gameObject != playerPrefab) return;

        StartCoroutine(TeleportToHouseFire(col.gameObject));
    }


    private IEnumerator TeleportToHouseFire(GameObject player)
    {
        cooldownSet.Add(burnGate0);

        // 상태 저장(필요하다면)
        BurngpManager.gameState = "FFStart";

        // HouseFire 씬으로 전환
        SceneManager.LoadScene("HouseFire");

        // HouseFire 씬에서 도착 게이트가 나타날 때까지 대기
        yield return new WaitUntil(() => GameObject.Find(houseFireGateName) != null);

        GameObject gateInHouseFire = GameObject.Find(houseFireGateName);

        if (gateInHouseFire != null)
        {
            Vector3 offset = new Vector3(offsetX, 0, 0);
            player.transform.position = gateInHouseFire.transform.position + offset;
        }

        // 게이트 쿨다운 (Burn쪽 gate0)
        yield return new WaitForSeconds(cooldownTime);
        cooldownSet.Remove(burnGate0);
    }
}
