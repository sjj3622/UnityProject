using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MINIMap : MonoBehaviour
{
    public GameObject[] minimap; // 9개 미니맵 오브젝트
    private Transform player;

    private HMCameraController hMCameraController;
    private Vector2 topLeft;
    private Vector2 bottomRight;

    private float widthThird;
    private float heightThird;

    // 깜빡임 코루틴 저장용
    private Coroutine[] blinkCoroutines;

    void Start()
    {
        hMCameraController = FindAnyObjectByType<HMCameraController>();
        topLeft = hMCameraController.topLeft;
        bottomRight = hMCameraController.bottomRight;

        // 플레이어 찾기
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다!");

        widthThird = Mathf.Abs(bottomRight.x - topLeft.x) / 3f;
        heightThird = Mathf.Abs(topLeft.y - bottomRight.y) / 3f;

        blinkCoroutines = new Coroutine[minimap.Length];

        //// 미니맵 오브젝트 배치
        //for (int y = 0; y < 3; y++)
        //{
        //    for (int x = 0; x < 3; x++)
        //    {
        //        int index = y * 3 + x;
        //        if (index >= minimap.Length) break;

        //        float posX = topLeft.x + widthThird * (x + 0.5f);
        //        float posY = topLeft.y - heightThird * (y + 0.5f); // y 반전 적용

        //        minimap[index].transform.position = new Vector3(posX, posY, 0);
        //    }
        //}
    }

    void Update()
    {
        if (player == null) return;

        Vector2 playerPos = player.position;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                int index = y * 3 + x;
                if (index >= minimap.Length) break;

                float minX = topLeft.x + widthThird * x;
                float maxX = topLeft.x + widthThird * (x + 1);

                float maxY = topLeft.y - heightThird * y;
                float minY = topLeft.y - heightThird * (y + 1);

                bool inZone = playerPos.x >= minX && playerPos.x < maxX &&
                              playerPos.y <= maxY && playerPos.y > minY;

                // 플레이어가 구역 안에 있으면 깜빡임 시작
                if (inZone)
                {
                    if (blinkCoroutines[index] == null)
                        blinkCoroutines[index] = StartCoroutine(Blink(minimap[index]));
                }
                else
                {
                    // 플레이어가 구역에서 나가면 깜빡임 중지
                    if (blinkCoroutines[index] != null)
                    {
                        StopCoroutine(blinkCoroutines[index]);
                        blinkCoroutines[index] = null;
                        minimap[index].SetActive(true); // 깜빡임 중지 후 항상 활성화
                    }
                }
            }
        }
    }

    IEnumerator Blink(GameObject obj)
    {
        while (true)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
