using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDcamera : MonoBehaviour
{
    [Header("드래그 설정")]
    public float dragSpeed = 1f;

    [Header("줌 설정")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    [Header("제한 영역")]
    public Transform targetArea;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Vector3 lastMousePosition;
    private Camera cam;

    //  초기 상태 저장용
    private Vector3 initialPosition;
    private float initialZoom;
    //private bool isReset = false; // 여러 번 초기화 방지용

    void Start()
    {
        cam = GetComponent<Camera>();

        // 초기 상태 저장
        initialPosition = transform.position;
        initialZoom = cam.orthographicSize;

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

        //  특정 상태일 때 카메라 초기화
        //if (BDgpManager.gameState == "BDClear" && !isReset)
        //{
        //    ResetCamera();
        //    isReset = true; // 중복 초기화 방지
        //    this.enabled = false;
        //}
        //else if (BDgpManager.gameState != "BDClear")
        //{
        //    isReset = false; // 상태가 바뀌면 다시 초기화 가능하게
        //}
    }

    void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * Time.deltaTime;

            transform.Translate(move, Space.Self);
            lastMousePosition = Input.mousePosition;

            ClampCameraPosition();
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            ClampCameraPosition();
        }
    }

    void ClampCameraPosition()
    {
        if (targetArea == null) return;

        Vector3 camPos = transform.position;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float areaWidth = maxBounds.x - minBounds.x;
        float areaHeight = maxBounds.y - minBounds.y;

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

    //  카메라 초기화 함수
    void ResetCamera()
    {
        transform.position = initialPosition;
        cam.orthographicSize = initialZoom;
        ClampCameraPosition();
        Debug.Log("카메라가 초기 상태로 복원되었습니다.");
    }
}
