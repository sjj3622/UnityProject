using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_Burn : MonoBehaviour
{
    public GameObject gate0; // Burn 씬에 있는 게이트 0
    public GameObject gate1; // HouseFire 씬에 있는 게이트 1 위치를 미리 참조하거나 로딩 후 설정
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
        
        Debug.Log(collision.CompareTag("Player"));
        
        if (!collision.CompareTag("Player")) return;
        
        if (cooldownGates.Contains(gate0)) return;
        
        Debug.Log(cooldownGates.Contains(gate0));


        StartCoroutine(TeleportToHouseFire(collision.gameObject));
    }

    private IEnumerator TeleportToHouseFire(GameObject player)
    {

        
        cooldownGates.Add(gate0);

        // 씬 전환
        BurngpManager.gameState = "BStart";
        SceneManager.LoadScene("HouseFire");

        // 씬 전환 후 프레임 한 번 대기
        yield return null;

        // 새 씬에서 게이트1 위치 찾기
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

        // gate0 쿨다운
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate0);
    }
}
