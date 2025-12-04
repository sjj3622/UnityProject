using System;
using UnityEngine;

public class blockController1 : MonoBehaviour
{
    public GameObject TileMaps0;
    public GameObject player;
    public GameObject[] blocks;
    public GameObject Chat;
    public GameObject Challenge;

    private Vector2[] blockSizes;
    private Color[] originalColors;

    private string[] keys;  // 서버에서 받은 Keys


    void Start()
    {
        // 🔥 GameDataManager에서 키 불러오기
        keys = GameDataManager.Instance.gameData.Keys;

        if (keys == null || keys.Length == 0)
        {
            Debug.LogError("Keys가 GameDataManager에서 불러와지지 않았습니다!");
        }

        // 배열 초기화
        blockSizes = new Vector2[blocks.Length];
        originalColors = new Color[blocks.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            SpriteRenderer sr = blocks[i].GetComponent<SpriteRenderer>();
            Collider2D col = blocks[i].GetComponent<Collider2D>();

            if (sr != null)
            {
                originalColors[i] = sr.color;
                sr.color = Color.black; // 기본적으로 잠김
            }

            blockSizes[i] = sr != null ? sr.bounds.size : Vector2.one;

            // 🔥 처음에는 모든 블럭 충돌 ON (닫힘 상태)
            if (col != null)
                col.enabled = true;
        }
    }



    void Update()
    {
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;

            for (int i = 0; i < blocks.Length; i++)
            {
                SpriteRenderer sr = blocks[i].GetComponent<SpriteRenderer>();
                Collider2D col = blocks[i].GetComponent<Collider2D>();

                if (sr == null) continue;

                bool hasKey = keys != null && keys.Length > i && keys[i] == "open";
                // ★ key00~key33 대신 서버가 "open"/"close" 같은 값 전달한다고 가정
                //   아니면 빈칸이 아니면 열림으로 처리할 수도 있음


                // 🔥 키가 있으면: 블럭 활성화(원래 색), 충돌 OFF
                if (hasKey)
                {
                    sr.color = originalColors[i];
                    if (col != null) col.enabled = false;
                }
                else
                {
                    // 🔥 키가 없으면: 블럭 잠김
                    sr.color = Color.black;
                    if (col != null) col.enabled = true;
                }


                // 플레이어가 블럭 위에 올라갔을 때 처리
                Vector3 blockPos = blocks[i].transform.position;
                float halfWidth = blockSizes[i].x / 2f;
                float halfHeight = blockSizes[i].y / 2f;

                bool inside =
                    playerPos.x >= blockPos.x - halfWidth && playerPos.x <= blockPos.x + halfWidth &&
                    playerPos.y >= blockPos.y - halfHeight && playerPos.y <= blockPos.y + halfHeight;

                if (inside && hasKey)
                {
                    // 원래 색 유지
                    sr.color = originalColors[i];

                    // SPACE 누르면 회전
                    if (Input.GetKeyDown(KeyCode.Space))
                        blocks[i].transform.Rotate(0f, 0f, -90f);
                }
            }
        }

        // Chat 거리 처리
        if (player == null || Chat == null) return;

        float distance = Vector2.Distance(player.transform.position, Challenge.transform.position);
        Chat.SetActive(distance < 1f);
    }
}
