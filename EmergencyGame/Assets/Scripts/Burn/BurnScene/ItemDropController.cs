using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropController : MonoBehaviour
{
    public static ItemDropController instance;

    [Header("아이템/스폰")]
    public GameObject[] ItemPrefab;
    public Transform[] ItemDrops;
    public GameObject[] inventory;
    public float spawnInterval = 3f;
    private int spawnCount = 1;

    

    private bool isStart = false;
    private List<float> itemProbabilities = new List<float>();
    private bool[] specialCollected = new bool[5];

    private BurnTimerController timerController;

    // 공유 속도
    public float sharedSpeed = 5f;

    // 이벤트
    public delegate void ItemCollected(int index, BPlayerController player);
    public event ItemCollected OnCollected;

    public delegate void PatientCollected(int index, PatientController patient);
    public event PatientCollected OnPatientCollected;

    void Awake()
    {
        instance = this;

    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
            itemProbabilities.Add(10f);

        timerController = FindAnyObjectByType<BurnTimerController>();

    }

    void Update()
    {
        if (BurngpManager.gameState == "RescuerGame" && !isStart)
        {
            isStart = true;

            // 중복 코루틴 방지
            StopAllCoroutines(); // 혹시 이미 돌고 있던 루틴 있다면 멈춤
            StartCoroutine(SpawnItemRoutine());
            StartCoroutine(SpawnCountRoutine());
            Debug.Log("아이템 스폰 시작!");

            // inventory 아이콘 초기화
            foreach (GameObject inv in inventory)
            {
                if (inv != null)
                {
                    Image img = inv.GetComponent<Image>();
                    if (img != null) img.color = new Color(1f, 1f, 1f, 0.3f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
            BurngpManager.gameState = "RescuerClear";
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
        if (ItemDrops.Length == 0) return;

        int itemIndex = GetRandomItemIndex();
        if (itemIndex == -1) return;

        // 사용할 수 있는 Drop 위치 목록
        List<int> availableDrops = new List<int>();
        for (int i = 0; i < ItemDrops.Length; i++)
            availableDrops.Add(i);

        // 실제 생성할 개수는 spawnCount와 Drop 위치 수 중 작은 값
        int spawnAmount = Mathf.Min(spawnCount, availableDrops.Count);

        for (int i = 0; i < spawnAmount; i++)
        {
            if (availableDrops.Count == 0) break;

            // 랜덤으로 Drop 위치 선택
            int randomIdx = Random.Range(0, availableDrops.Count);
            int dropIndex = availableDrops[randomIdx];
            availableDrops.RemoveAt(randomIdx); // 같은 위치 중복 방지

            // 스폰
            Vector3 pos = ItemDrops[dropIndex].position;
            pos.z = 0;

            GameObject obj = Instantiate(ItemPrefab[itemIndex], pos, Quaternion.identity);
            ItemController ic = obj.GetComponent<ItemController>();
            ic.itemIndex = itemIndex;

            // 이벤트 연결
            ic.OnCollected += (idx, p) => OnPlayerCollect(idx, p);
            ic.OnPatientCollected += (idx, t) => OnPatientCollect(idx, t);
        }
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

    // Player 이벤트 wrapper
    public void OnPlayerCollect(int index, BPlayerController player)
    {
        ApplySharedSpeed(index);
        OnCollected?.Invoke(index, player);
    }

    // Patient 이벤트 wrapper
    public void OnPatientCollect(int index, PatientController patient)
    {
        ApplySharedSpeed(index);
        OnPatientCollected?.Invoke(index, patient);
    }

    // 공유 속도 적용
    private void ApplySharedSpeed(int index)
    {
        // 속도 계산
        if (index >= 0 && index <= 4) // 기본 아이템 → 속도 감소
        {
            sharedSpeed = Mathf.Max(0f, sharedSpeed - 1f);
            
        }
        else if (index >= 5 && index <= 9) // 스페셜 아이템 → 속도 증가
        {
            sharedSpeed += 1f;
            

            // UI 업데이트
            int invIndex = index - 5;
            if (invIndex >= 0 && invIndex < inventory.Length)
            {
                Image img = inventory[invIndex].GetComponent<Image>();
                if (img != null) img.color = new Color(1f, 1f, 1f, 1f);
            }

            specialCollected[index - 5] = true;
            CheckSpecialClear();
        }


        // 속도 0 → 게임오버
        if (sharedSpeed <= 0f)
        {
            BurngpManager.gameState = "BOver";
            Debug.Log("공유 속도 0 → 게임오버!");
        }

        Debug.Log($"공유 속도 적용: {sharedSpeed}");
    }

    private void CheckSpecialClear()
    {
        for (int i = 0; i < 5; i++)
            if (!specialCollected[i]) return;

        BurngpManager.gameState = "RescuerClear";
        Debug.Log("스페셜 5~9 모두 수집 → Clear!");
    }

    IEnumerator SpawnCountRoutine()
    {
        while (true)
        {
            if (timerController)
            {
                float currentTime = timerController.GetCurrentTime();
                if (timerController.timerDuration - currentTime >= 20f)
                {
                    spawnCount++;
                    timerController.timerDuration -= 20f;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
