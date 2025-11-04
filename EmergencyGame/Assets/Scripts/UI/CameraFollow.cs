using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 따라갈 대상 (플레이어)
    public float smoothSpeed = 0.125f;
    public Vector3 offset;          // 위치 보정용 오프셋

    private bool followActive = false; // 따라가기 활성화 여부

    void LateUpdate()
    {
        if (!followActive || target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }

    public void SetFollowTarget(Transform newTarget)
    {
        target = newTarget;
        followActive = true;
    }

    public void StopFollow()
    {
        followActive = false;
    }
}
