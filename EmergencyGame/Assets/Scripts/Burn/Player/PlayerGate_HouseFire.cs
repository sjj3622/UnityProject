using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_HouseFire : MonoBehaviour
{
    public GameObject[] PlayerGates; // 0~7번 게이트
    public GameObject burnGate; // "playermove1-1" 인스펙터에서 연결
    public float teleportOffset = 1f;
    public float teleportCooldown = 1f;

    private Dictionary<GameObject, GameObject> gateConnections = new Dictionary<GameObject, GameObject>();
    private HashSet<GameObject> cooldownGates = new HashSet<GameObject>();

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // gateConnections 초기화
        if (sceneName == "HouseFire")
        {
            gateConnections[PlayerGates[0]] = null; // gate0 없음
        }
        else
        {
            gateConnections[PlayerGates[0]] = burnGate; // Burn 씬용
        }

        // 나머지 게이트 연결
        gateConnections[PlayerGates[1]] = PlayerGates[0];
        gateConnections[PlayerGates[2]] = PlayerGates[3];
        gateConnections[PlayerGates[3]] = PlayerGates[2];
        gateConnections[PlayerGates[4]] = PlayerGates[5];
        gateConnections[PlayerGates[5]] = PlayerGates[4];
        gateConnections[PlayerGates[6]] = PlayerGates[7];
        gateConnections[PlayerGates[7]] = PlayerGates[6];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (PlayerGates == null || PlayerGates.Length == 0) return;

        // 플레이어와 가장 가까운 게이트 찾기
        GameObject collidedGate = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject gate in PlayerGates)
        {
            if (gate == null) continue;
            float dist = Vector3.Distance(collision.transform.position, gate.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                collidedGate = gate;
            }
        }

        if (collidedGate == null || cooldownGates.Contains(collidedGate)) return;

        if (gateConnections.ContainsKey(collidedGate))
        {
            GameObject targetGate = gateConnections[collidedGate];
            if (targetGate == null) return; // 연결된 게이트가 없으면 리턴

            // Burn 씬으로 이동
            if (targetGate == burnGate)
            {
                StartCoroutine(TeleportToBurn(collision.gameObject));
            }
            else
            {
                // 일반 게이트 순간이동
                Vector3 offset = new Vector3(teleportOffset, 0, 0);
                collision.transform.position = targetGate.transform.position + offset;
                StartCoroutine(StartCooldown(collidedGate));
            }
        }
    }

    private IEnumerator TeleportToBurn(GameObject player)
    {
        // 쿨다운 적용
        cooldownGates.Add(PlayerGates[0]);

        // 씬 로드
        SceneManager.LoadScene("Burn");

        // 씬이 로드될 때까지 기다림
        yield return new WaitUntil(() => GameObject.Find(burnGate.name) != null);

        // 플레이어 위치 이동
        GameObject gateInScene = GameObject.Find(burnGate.name);
        if (gateInScene != null)
        {
            Vector3 offset = new Vector3(teleportOffset, 0, 0);
            player.transform.position = gateInScene.transform.position + offset;
        }

        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(PlayerGates[0]);
    }

    private IEnumerator StartCooldown(GameObject gate)
    {
        cooldownGates.Add(gate);
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate);
    }
    public void StartCooldownTemp(GameObject gate)
    {
        StartCoroutine(StartCooldown(gate));
    }
}
