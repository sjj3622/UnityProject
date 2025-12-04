using UnityEngine;

public class blockController2 : MonoBehaviour
{
    public GameObject player;          // 플레이어 오브젝트
    public GameObject[] blocks;        // 블록들을 배열로 연결
    private Vector2[] blockSizes;      // 각 블록의 크기 저장

    void Start()
    {
        // 블록 개수에 맞게 배열 초기화
        blockSizes = new Vector2[blocks.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            SpriteRenderer sr = blocks[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                blockSizes[i] = sr.bounds.size;
            }
            else
            {
                blockSizes[i] = Vector2.one;
            }
        }
    }

    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 playerPos = player.transform.position;

            for (int i = 0; i < blocks.Length; i++)
            {
                Vector3 blockPos = blocks[i].transform.position;
                float halfWidth = blockSizes[i].x / 2f;
                float halfHeight = blockSizes[i].y / 2f;

                if (playerPos.x >= blockPos.x - halfWidth && playerPos.x <= blockPos.x + halfWidth &&
                    playerPos.y >= blockPos.y - halfHeight && playerPos.y <= blockPos.y + halfHeight)
                {
                    // 플레이어가 블록 안에 있으면 회전
                    blocks[i].transform.Rotate(0f, 0f, -90f);
                }
            }
        }
    }
}
