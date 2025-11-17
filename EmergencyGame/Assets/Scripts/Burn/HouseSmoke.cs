using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSmoke : MonoBehaviour
{
    public GameObject[] houseSmokes;

    void Start()
    {
        // 1. 처음에는 모두 활성화
        for (int i = 0; i < houseSmokes.Length; i++)
        {
            houseSmokes[i].SetActive(true);
        }

        // 2. 일정 시간 후 랜덤 활성화 시작
        StartCoroutine(RandomToggleSmoke());
    }

    IEnumerator RandomToggleSmoke()
    {
        while (true)
        {
            int total = houseSmokes.Length;
            int minActive = Mathf.CeilToInt(total * 2f / 3f); // 최소 2/3 활성

            // 새 활성화 리스트 준비
            List<int> activeIndexes = new List<int>();

            // 2/3 개수만큼 랜덤 활성화
            List<int> indexPool = new List<int>();
            for (int i = 0; i < total; i++)
            {
                indexPool.Add(i);
            }

            // 랜덤하게 2/3만큼 뽑기
            for (int i = 0; i < minActive; i++)
            {
                int random = Random.Range(0, indexPool.Count);
                activeIndexes.Add(indexPool[random]);
                indexPool.RemoveAt(random);
            }

            // 전체 순회하며 활성/비활성 업데이트
            for (int i = 0; i < total; i++)
            {
                bool active = activeIndexes.Contains(i);
                houseSmokes[i].SetActive(active);
            }

            // 1~3초 사이 랜덤 간격으로 반복
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }
    }
}
