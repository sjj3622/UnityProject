using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDcamera : MonoBehaviour
{
    [Header("드래그 설정")]
    public float dragSpeed = 1f; // 마우스 드래그 속도

    [Header("줌 설정")]
    public float zoomSpeed = 5f; // 마우스 휠 줌 속도
    public float minZoom = 2f;   // 최소 줌
    public float maxZoom = 10f;  // 최대 줌

    [Header("제한 영역")]
    public Transform targetArea; // 카메라가 움직일 수 있는 이미지 오브젝트
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Vector3 lastMousePosition;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (targetArea != null)
        {
            Renderer rend = targetArea.GetComponent<Renderer>();
            if (rend != null)
            {
                minBounds = rend.bounds.min;
                maxBounds = rend.bounds.max;
            }
            else
            {
                Debug.LogWarning("targetArea에 Renderer가 없습니다. SpriteRenderer나 MeshRenderer가 필요합니다.");
            }
        }
        else
        {
            Debug.LogWarning("targetArea가 설정되지 않았습니다.");
        }
    }

    void Update()
    {
        HandleCameraDrag();
        HandleZoom();
        ClampCameraPosition();
    }

    void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭 눌렀을 때
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) // 우클릭 드래그 중일 때
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * Time.deltaTime;

            transform.Translate(move, Space.Self);
            lastMousePosition = Input.mousePosition;

            ClampCameraPosition(); // 드래그 중에도 즉시 제한 적용
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // 줌 변경
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

            // 줌 후 카메라 위치가 영역을 벗어나지 않게 조정
            ClampCameraPosition();
        }
    }

    void ClampCameraPosition()
    {
        if (targetArea == null) return;

        Vector3 camPos = transform.position;

        // 카메라 크기 계산
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // 현재 줌 상태에서 카메라가 영역보다 커질 경우 보정
        float areaWidth = maxBounds.x - minBounds.x;
        float areaHeight = maxBounds.y - minBounds.y;

        // 만약 카메라 시야가 영역보다 커지면 중앙 고정
        if (camWidth * 2 >= areaWidth)
            camPos.x = (minBounds.x + maxBounds.x) / 2f;
        else
            camPos.x = Mathf.Clamp(camPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);

        if (camHeight * 2 >= areaHeight)
            camPos.y = (minBounds.y + maxBounds.y) / 2f;
        else
            camPos.y = Mathf.Clamp(camPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = camPos;
    }
}
