using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_HouseFire : MonoBehaviour
{
    public GameObject[] PlayerGates; // 0~7번 게이트
    public float teleportOffset = 1f;
    public float teleportCooldown = 1f;

    private Dictionary<GameObject, GameObject> gateConnections = new Dictionary<GameObject, GameObject>();
    private HashSet<GameObject> cooldownGates = new HashSet<GameObject>();

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // HouseFire 씬에는 gate0 없음
        if (sceneName == "HouseFire")
        {
            gateConnections[PlayerGates[0]] = null; // gate0 없음
        }
        else
        {
            // Burn 씬에서 gate0 연결
            gateConnections[PlayerGates[0]] = GameObject.Find("playermove1-1");
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
            if (gate == null) continue; // null 체크
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
            Debug.Log("여기1");
            // Burn 씬으로 이동
            if (targetGate.name == "playermove1-1")
            {
                Debug.Log("targetGate.name:" + targetGate.name);
                StartCoroutine(TeleportToBurn(collision.gameObject, targetGate));
            }
            else
            {
                Debug.Log("여기3");    
                // 일반 게이트 순간이동
                Vector3 offset = new Vector3(teleportOffset, 0, 0);
                collision.transform.position = targetGate.transform.position + offset;
                StartCoroutine(StartCooldown(collidedGate));
            }
        }
    }

    private IEnumerator TeleportToBurn(GameObject player, GameObject targetGate)
    {
        cooldownGates.Add(targetGate);
        SceneManager.LoadScene("Burn");
        yield return null;

        GameObject gate0InScene = GameObject.Find("playermove1-1");
        Debug.Log(gate0InScene);
        if (gate0InScene != null)
        {
            Vector3 offset = new Vector3(teleportOffset, 0, 0);
            player.transform.position = gate0InScene.transform.position + offset;
        }

        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(targetGate);
    }

    public void StartCooldownTemp(GameObject gate)
    {
        StartCoroutine(StartCooldown(gate));
    }

    private IEnumerator StartCooldown(GameObject gate)
    {
        cooldownGates.Add(gate);
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate);
    }
}
