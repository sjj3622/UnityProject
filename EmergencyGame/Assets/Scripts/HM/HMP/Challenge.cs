using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public GameObject Player;  // 플레이어 오브젝트
    public GameObject Chat;    // 활성/비활성 할 채팅 오브젝트

    void Update()
    {
        if (Player == null || Chat == null) return;

        // 거리 계산
        float distance = Vector2.Distance(Player.transform.position, transform.position);

        Debug.Log(distance);
        // 거리 조건 판단
        if (distance < 1f)
        {
            // 좌표가 완전히 동일한 경우 Chat 활성화
            Chat.SetActive(true);
        }
        else if (distance >= 1f)
        {
            // 거리가 1 이상 멀어지면 비활성화
            Chat.SetActive(false);
        }
    }
}
