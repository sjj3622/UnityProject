using UnityEngine;

public class HMCameraController : MonoBehaviour
{
    public GameObject Tilemap;      // Tilemap 오브젝트
    private Transform player;       // 플레이어 트랜스폼
    private Vector2 minBoundary;    // 카메라 이동 최소값
    private Vector2 maxBoundary;    // 카메라 이동 최대값
    private Camera cam;

    // Tilemap 경계 좌표 (직접 지정)
    public Vector2 topLeft = new Vector2(-8.82f, 7.89f);
    public Vector2 bottomRight = new Vector2(61.89f, -47.57f);

    void Start()
    {
        cam = GetComponent<Camera>();

        // 이전 씬에서 넘어온 플레이어 찾기
        GameObject playerObj = GameObject.Find("HMPlayer(Clone)"); // 플레이어 이름
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player를 찾을 수 없습니다!");
        }

        // 카메라 크기 계산
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        // 좌상단, 우하단 기준으로 카메라 이동 범위 계산
        minBoundary = new Vector2(topLeft.x + camWidth, bottomRight.y + camHeight);
        maxBoundary = new Vector2(bottomRight.x - camWidth, topLeft.y - camHeight);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 플레이어 위치 가져오기
        Vector3 targetPos = player.position;

        // 경계 안으로 제한
        float clampedX = Mathf.Clamp(targetPos.x, minBoundary.x, maxBoundary.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBoundary.y, maxBoundary.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
