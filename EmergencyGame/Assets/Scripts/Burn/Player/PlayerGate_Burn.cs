using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_Burn : MonoBehaviour
{
    public GameObject gate0; // Burn 씬에 있는 게이트 0
    public float teleportOffset = 1f;
    public float teleportCooldown = 1f;

    private HashSet<GameObject> cooldownGates = new HashSet<GameObject>();

    private void Start()
    {
        // 플레이어 오브젝트 씬 전환 유지
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (cooldownGates.Contains(gate0)) return;

        StartCoroutine(TeleportToHouseFire(collision.gameObject));
    }

    private IEnumerator TeleportToHouseFire(GameObject player)
    {
        // gate0 쿨다운 등록
        cooldownGates.Add(gate0);

        // 씬 전환 전 상태 저장
        BurngpManager.gameState = "BStart";

        // HouseFire 씬으로 이동
        SceneManager.LoadScene("HouseFire");

        // 씬 로드가 완료될 때까지 대기
        yield return new WaitUntil(() => GameObject.Find("playermove1-2") != null);

        // 새 씬에서 gate1 위치 찾기
        GameObject gate1InScene = GameObject.Find("playermove1-2");
        if (gate1InScene != null)
        {
            Vector3 offset = new Vector3(teleportOffset, 0, 0);
            player.transform.position = gate1InScene.transform.position + offset;

            // gate1 쿨다운 등록
            PlayerGate_HouseFire houseFireScript = gate1InScene.GetComponentInParent<PlayerGate_HouseFire>();
            if (houseFireScript != null)
            {
                houseFireScript.StartCooldownTemp(gate1InScene);
            }
        }

        // gate0 쿨다운 해제
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate0);
    }
}
