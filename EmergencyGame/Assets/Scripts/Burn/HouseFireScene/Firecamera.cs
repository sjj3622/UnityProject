using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecamera : MonoBehaviour
{
    private Transform player; // 플레이어는 private로 동적으로 찾음
    public float followSpeed = 5f; // 카메라 이동 속도

    void Update()
    {
        // 플레이어가 없으면 찾아오기
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            
            if (playerObj != null)
                player = playerObj.transform;
            else
                return; // 플레이어 없으면 Update 종료
        }

        Vector3 targetPos = transform.position;

        // 플레이어 Y값에 따라 층 결정
        if (player.position.y < 10f) // 1층
        {
            targetPos.y = 0f;
            targetPos.x = Mathf.Clamp(player.position.x, -7.3f, 1.2f);
        }
        else if (player.position.y < 30f) // 2층
        {
            targetPos.y = 20f;
            targetPos.x = Mathf.Clamp(player.position.x, -1.3f, 1.3f);
        }
        else if (player.position.y < 47f) // 3층
        {
            targetPos.y = 38f;
            targetPos.x = Mathf.Clamp(player.position.x, -1.3f, 1.3f);
        }
        else // 4층
        {
            targetPos.y = 56f;
            targetPos.x = Mathf.Clamp(player.position.x, -1.3f, 1.3f);
        }

        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
