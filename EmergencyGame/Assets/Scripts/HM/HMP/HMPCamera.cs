using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMPCamera : MonoBehaviour
{
    public Transform Player;   // 따라갈 플레이어
    public float smoothSpeed = 0.125f;  // 부드럽게 이동시키는 정도
    public Vector3 offset;     // 카메라 위치 보정

    void LateUpdate()
    {
        if (Player == null) return;

        Vector3 desiredPosition = Player.position + offset;

        // z 값은 기존 카메라 값 유지
        desiredPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
