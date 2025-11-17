using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGate : MonoBehaviour
{
    public GameObject[] PlayerGates; // 8개의 게이트
    public float teleportOffset = 1f; // 텔레포트 시 겹치지 않게 약간 이동
    public float teleportCooldown = 1f; // 1초 동안 게이트 이동 막기

    private Dictionary<GameObject, GameObject> gateConnections = new Dictionary<GameObject, GameObject>();
    private HashSet<GameObject> cooldownGates = new HashSet<GameObject>();

    void Start()
    {
        // 게이트 연결 초기화
        gateConnections[PlayerGates[0]] = PlayerGates[1];
        gateConnections[PlayerGates[1]] = PlayerGates[0];

        gateConnections[PlayerGates[2]] = PlayerGates[3];
        gateConnections[PlayerGates[3]] = PlayerGates[2];

        gateConnections[PlayerGates[4]] = PlayerGates[5];
        gateConnections[PlayerGates[5]] = PlayerGates[4];

        gateConnections[PlayerGates[6]] = PlayerGates[7];
        gateConnections[PlayerGates[7]] = PlayerGates[6];

        // 처음엔 모두 활성화
        for (int i = 0; i < PlayerGates.Length; i++)
        {
            PlayerGates[i].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 플레이어와 가장 가까운 게이트 찾기
        GameObject collidedGate = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject gate in PlayerGates)
        {
            float dist = Vector3.Distance(collision.transform.position, gate.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                collidedGate = gate;
            }
        }

        // 쿨다운 중인 게이트면 이동 막기
        if (collidedGate != null && cooldownGates.Contains(collidedGate)) return;

        // 연결된 게이트로 이동
        if (collidedGate != null && gateConnections.ContainsKey(collidedGate))
        {
            GameObject targetGate = gateConnections[collidedGate];
            Vector3 offset = new Vector3(teleportOffset, 0, 0);
            collision.transform.position = targetGate.transform.position + offset;

            // 쿨다운 등록
            StartCoroutine(StartCooldown(targetGate));
        }
    }

    private IEnumerator StartCooldown(GameObject gate)
    {
        cooldownGates.Add(gate);
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate);
    }
}
