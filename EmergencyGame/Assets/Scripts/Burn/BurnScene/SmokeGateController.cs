using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGateController : MonoBehaviour
{
    public GameObject smokegatePrefab;
    public Transform[] smokegates;
    private List<int> availableIndices = new List<int>();


    public int initialCount = 3;
    public float spawnInterval = 10f;

    private int totalSpawned = 0;
    private bool spawningStarted = false;

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
    }

    void Update()
    {
        // BurnTimerController의 timerRunning 확인
        if (!spawningStarted && BurnTimerControllerInstance().timerRunning)
        {
            spawningStarted = true;
            InvokeRepeating("SpawnRandom", spawnInterval, spawnInterval);
        }
    }

    void SpawnRandom()
    {
        if (availableIndices.Count == 0)
        {
            Instantiate(smokegatePrefab, smokegates[smokegates.Length - 1].position, Quaternion.identity);
            BurngpManager.gameState = "BOver";
            CancelInvoke("SpawnRandom");
            return;
        }

        int randIndex = Random.Range(0, availableIndices.Count);
        int spawnIndex = availableIndices[randIndex];
        availableIndices.RemoveAt(randIndex);

        Instantiate(smokegatePrefab, smokegates[spawnIndex].position, Quaternion.identity);
        totalSpawned++;
    }

    // BurnTimerController 찾는 간단한 함수
    BurnTimerController BurnTimerControllerInstance()
    {
        return FindObjectOfType<BurnTimerController>();
    }

}
