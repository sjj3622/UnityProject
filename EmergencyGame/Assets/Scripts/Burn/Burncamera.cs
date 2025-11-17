using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Burncamera : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    private Camera cam;
    private bool followPlayer = false;

    private Tilemap mapTilemap; // Tilemap 자체
    private Bounds mapBounds; // 실제 타일 영역 Bounds

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = 10f;

        GameObject mapObj = GameObject.Find("-201");
        if (mapObj == null)
        {
            Debug.LogError("맵 오브젝트를 찾지 못했습니다!");
            return;
        }

        mapTilemap = mapObj.GetComponent<Tilemap>();
        if (mapTilemap == null)
        {
            Debug.LogError("Tilemap 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        CalculateTilemapBounds(); // 실제 타일 영역 Bounds 계산
    }

    void Update()
    {
        if (player != null && !followPlayer)
        {
            followPlayer = true;
            cam.orthographicSize = 5f; // 플레이어 등장 시 카메라 축소
        }
    }

    void LateUpdate()
    {
        if (followPlayer && mapTilemap != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            float camHalfWidth = cam.orthographicSize * cam.aspect;
            float camHalfHeight = cam.orthographicSize;

            float minX = mapBounds.min.x + camHalfWidth;
            float maxX = mapBounds.max.x - camHalfWidth;
            float minY = mapBounds.min.y + camHalfHeight;
            float maxY = mapBounds.max.y - camHalfHeight;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    // 실제 타일 영역 계산
    private void CalculateTilemapBounds()
    {
        if (mapTilemap == null) return;

        BoundsInt cellBounds = mapTilemap.cellBounds;
        Vector3Int min = cellBounds.max; // 최대값부터 시작
        Vector3Int max = cellBounds.min; // 최소값부터 시작

        foreach (Vector3Int pos in cellBounds.allPositionsWithin)
        {
            if (mapTilemap.HasTile(pos))
            {
                min = Vector3Int.Min(min, pos);
                max = Vector3Int.Max(max, pos);
            }
        }

        // Tilemap 좌표 -> 월드 좌표 변환
        Vector3 minWorld = mapTilemap.CellToWorld(min);
        Vector3 maxWorld = mapTilemap.CellToWorld(max + Vector3Int.one); // +1로 타일 크기 포함
        mapBounds = new Bounds();
        mapBounds.SetMinMax(minWorld, maxWorld);
    }
}
