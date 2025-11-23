using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    public static ItemDropController instance;


public GameObject[] ItemPrefab;
    public Transform[] ItemDrops;
    public float spawnInterval = 3f;
    private int spawnCount = 1; // 동시에 생성되는 아이템 수

    private bool isStart = false;
    private List<float> itemProbabilities = new List<float>();
    private bool[] specialCollected = new bool[5];

    private BurnTimerController timerController;

    void Awake() { instance = this; }

    void Start()
    {
        for (int i = 0; i < 10; i++)
            itemProbabilities.Add(10f);

        // TimerController 참조
        timerController = FindAnyObjectByType<BurnTimerController>();
    }

    void Update()
    {
        if (BurngpManager.gameState == "RescuerGame" && !isStart)
        {
            isStart = true;
            StartCoroutine(SpawnItemRoutine());
            StartCoroutine(SpawnCountRoutine());
            Debug.Log("아이템 스폰 시작!");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            BurngpManager.gameState = "RescuerClear";
        }
    }

    IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
                SpawnRandomItem();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomItem()
    {
        int itemIndex = GetRandomItemIndex();
        if (itemIndex == -1) { Debug.Log("스폰 가능한 아이템 없음"); return; }

        int dropIndex = Random.Range(0, ItemDrops.Length);
        Vector3 pos = ItemDrops[dropIndex].position;
        pos.z = 0;

        GameObject obj = Instantiate(ItemPrefab[itemIndex], pos, Quaternion.identity);

        ItemController ic = obj.GetComponent<ItemController>();
        ic.itemIndex = itemIndex;

        // 플레이어 충돌 시 이벤트 구독
        ic.OnCollected += HandleItemCollected;

        Debug.Log("생성됨: Item " + itemIndex + " 위치: " + pos);
    }

    int GetRandomItemIndex()
    {
        float total = 0;
        for (int i = 0; i < 10; i++)
        {
            if (i >= 5 && specialCollected[i - 5]) continue;
            total += itemProbabilities[i];
        }
        if (total <= 0) return -1;

        float rand = Random.Range(0, total);
        float sum = 0;
        for (int i = 0; i < 10; i++)
        {
            if (i >= 5 && specialCollected[i - 5]) continue;
            sum += itemProbabilities[i];
            if (rand <= sum) return i;
        }
        return -1;
    }

    public void OnItemCollected(int index)
    {
        if (index >= 5)
        {
            specialCollected[index - 5] = true;
            for (int i = 0; i < 5; i++) itemProbabilities[i] += 1;
            CheckSpecialClear();
        }
    }

    void CheckSpecialClear()
    {
        for (int i = 0; i < 5; i++)
            if (!specialCollected[i]) return;

        BurngpManager.gameState = "RescuerClear";
        Debug.Log("RescuerClear! 모든 5~9 아이템 수집 완료");
    }

    // 플레이어 속도 증가/감소 처리
    private void HandleItemCollected(int index, BPlayerController player)
    {
        if (player == null) return;


        if (index >= 0 && index <= 4)
        {
            player.speed = Mathf.Max(0f, player.speed - 1f); // 최소 속도 0
            Debug.Log("스피드 -1 :" + player.speed);
        }
        else if (index >= 5 && index <= 9)
        {
            player.speed += 1f;
            Debug.Log("스피드 +1 :"+player.speed);
        }
        // speed가 0이면 게임 오버
        if (player.speed <= 0f)
        {
            BurngpManager.gameState = "BOver";
            Debug.Log("플레이어 속도 0! 게임 오버");
        }

        OnItemCollected(index);
    }

    // Timer 기준 20초마다 spawnCount 증가
    IEnumerator SpawnCountRoutine()
    {
        float lastTime = timerController.timerDuration; // 180초 시작
        while (true)
        {
            if (timerController != null)
            {
                float currentTime = timerController.GetCurrentTime();
                // 20초 단위 경과 확인
                if (lastTime - currentTime >= 20f)
                {
                    spawnCount += 1;  // 생성 개수 증가
                    lastTime -= 20f;  // 다음 20초 단위 체크
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }


}
