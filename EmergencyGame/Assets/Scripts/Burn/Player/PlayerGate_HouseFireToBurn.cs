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
    public float offsetX = 1f;
    public float cooldownTime = 1f;

    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();

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

        StartCoroutine(TeleportToBurn(col.gameObject));
    }

    private IEnumerator TeleportToBurn(GameObject player)
    {
        cooldownSet.Add(houseGate0);

        // 상태 저장(필요하다면)
        BurngpManager.gameState = "BStart"; // 필요시 수정

        // Burn 씬으로 전환
        SceneManager.LoadScene("Burn");

        // Burn 씬에서 도착 게이트가 나타날 때까지 대기
        yield return new WaitUntil(() => GameObject.Find(burnGateName) != null);

        GameObject gateInBurn = GameObject.Find(burnGateName);

        if (gateInBurn != null)
        {
            Vector3 offset = new Vector3(offsetX, 0, 0);
            player.transform.position = gateInBurn.transform.position + offset;
        }

        // 게이트 쿨다운 (HouseFire쪽 gate0)
        yield return new WaitForSeconds(cooldownTime);
        cooldownSet.Remove(houseGate0);
    }
}
