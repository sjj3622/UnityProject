using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGate_Normal : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject[] gates; // 2~7번 게이트

    [Header("Teleport Settings")]
    public float offsetDistance = 1f; // 게이트에서 나올 때의 거리
    public float cooldownTime = 1f; // 게이트 쿨타임

    private Dictionary<GameObject, GameObject> linkMap = new Dictionary<GameObject, GameObject>();
    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();

    private void Start()
    {
        InitializeGateLinks();
    }

    // 게이트 쌍 초기화
    private void InitializeGateLinks()
    {
        linkMap.Clear();

        if (gates.Length >= 6)
        {
            linkMap[gates[0]] = gates[1];
            linkMap[gates[1]] = gates[0];

            linkMap[gates[2]] = gates[3];
            linkMap[gates[3]] = gates[2];

            linkMap[gates[4]] = gates[5];
            linkMap[gates[5]] = gates[4];
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        GameObject currentGate = GetClosestGate(col.transform.position);
        if (currentGate == null || cooldownSet.Contains(currentGate)) return;
        Debug.Log("currentGate :" + currentGate);

        GameObject targetGate = linkMap.ContainsKey(currentGate) ? linkMap[currentGate] : null;
        Debug.Log("targetGate :" + targetGate);

        if (targetGate != null && !cooldownSet.Contains(targetGate))
        {
            TeleportToOppositeSide(col.gameObject, currentGate, targetGate);
            // 양쪽 게이트 모두 쿨타임 적용
            StartCoroutine(GateCooldown(currentGate));
            StartCoroutine(GateCooldown(targetGate));
        }
    }


    // 현재 게이트 제외하고 플레이어와 가장 가까운 게이트 찾기
    private GameObject GetClosestGate(Vector3 playerPos, GameObject excludeGate = null)
    {
        if (gates == null || gates.Length == 0) return null;

        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var gate in gates)
        {
            if (gate == null || gate == excludeGate) continue;
            float dist = Vector3.Distance(playerPos, gate.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = gate;
            }
        }

        return closest;
    }

    // 게이트 이동 처리 (반대쪽 출구)
    private void TeleportToOppositeSide(GameObject player, GameObject fromGate, GameObject targetGate)
    {
        // 들어온 게이트 -> 나갈 게이트 방향 벡터 계산
        Vector3 direction = (targetGate.transform.position - fromGate.transform.position).normalized;

        // 반대쪽 출구에서 나오도록 offset 적용
        Vector3 offset = direction * offsetDistance;

        player.transform.position = targetGate.transform.position + offset;
    }

    // 쿨타임 처리
    private IEnumerator GateCooldown(GameObject gate)
    {
        cooldownSet.Add(gate);
        yield return new WaitForSeconds(cooldownTime);
        cooldownSet.Remove(gate);
    }
}
