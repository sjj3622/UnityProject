using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public Transform player;    // Player 오브젝트
    public Transform patient;   // Patient 오브젝트
    public float smoothSpeed = 5f;

    private Camera cam;
    private bool followPlayer = false; // 플레이어 따라가기 여부
    private Vector3 offset;             // 카메라 오프셋

    void Start()
    {
        cam = GetComponent<Camera>();

        // 시작 시 카메라 고정
        transform.position = new Vector3(0f, 0f, -10f);
        cam.orthographicSize = 8f;
    }

    void LateUpdate()
    {
        if (followPlayer && player != null)
        {
            // 부드럽게 플레이어 따라가기 (오프셋 포함)
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, -10f) + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
        }
    }

    // Player와 Patient의 충돌 시 호출될 메서드
    public void OnPlayerPatientCollision()
    {
        followPlayer = true;
        cam.orthographicSize = 2f;

        // 📍 플레이어가 화면의 왼쪽 상단에 보이도록 카메라 오프셋 설정
        // 숫자는 조정 가능 (오른쪽/아래로 카메라를 이동)
        offset = new Vector3(1.0f, 0f, 0f);

        //카메라 사이즈 로그 출력
        Debug.Log("Camera Size Changed! Current Size: " + cam.orthographicSize);
    }
}
