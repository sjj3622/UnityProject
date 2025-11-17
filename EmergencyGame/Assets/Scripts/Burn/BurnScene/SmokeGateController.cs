using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGateController : MonoBehaviour
{
    public GameObject smokegatePrefab; // 생성할 오브젝트 프리팹
    public Transform[] smokegates;     // 오브젝트가 배치될 위치 배열
    private List<int> availableIndices = new List<int>(); // 랜덤 생성에 사용될 남은 인덱스

    public int initialCount = 3;       // 처음 생성할 오브젝트 개수
    public float spawnInterval = 10f;  // 10초마다 생성

    private int totalSpawned = 0;      // 지금까지 생성된 오브젝트 수

    void Start()
    {
        // 랜덤 생성에 사용할 인덱스 초기화 (마지막 인덱스 제외)
        for (int i = 0; i < smokegates.Length - 1; i++)
        {
            availableIndices.Add(i);
        }

        // 처음 3개 생성
        for (int i = 0; i < initialCount; i++)
        {
            SpawnRandom();
        }

        // 10초마다 생성 반복
        InvokeRepeating("SpawnRandom", spawnInterval, spawnInterval);
    }

    void SpawnRandom()
    {
        // 마지막 오브젝트까지 생성했으면 마지막 위치에 생성하고 상태 변경
        if (availableIndices.Count == 0)
        {
            // 마지막 인덱스에 생성
            Instantiate(smokegatePrefab, smokegates[smokegates.Length - 1].position, Quaternion.identity);

            // 게임 상태 변경
            BurngpManager.gameState = "BOver";

            // 반복 중지
            CancelInvoke("SpawnRandom");
            return;
        }

        // 랜덤 인덱스 선택
        int randIndex = Random.Range(0, availableIndices.Count);
        int spawnIndex = availableIndices[randIndex];

        // 선택한 인덱스 제거 (중복 방지)
        availableIndices.RemoveAt(randIndex);

        // 오브젝트 생성
        Instantiate(smokegatePrefab, smokegates[spawnIndex].position, Quaternion.identity);

        totalSpawned++;
    }
}
