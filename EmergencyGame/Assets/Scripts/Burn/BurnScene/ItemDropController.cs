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
    public float sharedSpeed = 5f;

    public delegate void ItemCollected(int index, BPlayerController player);
    public event ItemCollected OnCollected;

    public delegate void PatientCollected(int index, PatientController patient);
    public event PatientCollected OnPatientCollected;

    // 위치별 생성된 아이템 관리
    private Dictionary<Transform, GameObject> spawnedItems = new Dictionary<Transform, GameObject>();

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
            StopAllCoroutines();
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
            // 현재 빈 위치 목록 확보
            List<Transform> availableDrops = new List<Transform>();
            foreach (Transform drop in ItemDrops)
            {
                if (!spawnedItems.ContainsKey(drop) || spawnedItems[drop] == null)
                    availableDrops.Add(drop);
            }

            if (availableDrops.Count > 0)
            {
                // spawnCount보다 빈 위치가 적으면 빈 위치만큼만 스폰
                int spawnAmount = Mathf.Min(spawnCount, availableDrops.Count);

                for (int i = 0; i < spawnAmount; i++)
                {
                    SpawnRandomItem(availableDrops);
                    yield return new WaitForSeconds(0.05f); // 동시 중복 방지 딜레이
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomItem(List<Transform> availableDrops)
    {
        if (ItemDrops.Length == 0 || availableDrops.Count == 0) return;

        int itemIndex = GetRandomItemIndex();
        if (itemIndex == -1) return;

        // 랜덤 위치 선택 후 목록에서 제거
        int randomIdx = Random.Range(0, availableDrops.Count);
        Transform dropPos = availableDrops[randomIdx];
        availableDrops.RemoveAt(randomIdx);

        Vector3 pos = dropPos.position;
        pos.z = 0;

        GameObject obj = Instantiate(ItemPrefab[itemIndex], pos, Quaternion.identity);
        ItemController ic = obj.GetComponent<ItemController>();
        ic.itemIndex = itemIndex;

        spawnedItems[dropPos] = obj;

        Debug.Log($"[아이템 스폰] 게이트: {dropPos.name}, 아이템: {ItemPrefab[itemIndex].name}, 인덱스: {itemIndex}");

        // 이벤트 연결
        ic.OnCollected += (idx, p) => {
            OnPlayerCollect(idx, p);
            spawnedItems[dropPos] = null;
        };
        ic.OnPatientCollected += (idx, t) => {
            OnPatientCollect(idx, t);
            spawnedItems[dropPos] = null;
        };
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

    public void OnPlayerCollect(int index, BPlayerController player)
    {
        ApplySharedSpeed(index);
        OnCollected?.Invoke(index, player);
    }

    public void OnPatientCollect(int index, PatientController patient)
    {
        ApplySharedSpeed(index);
        OnPatientCollected?.Invoke(index, patient);
    }

    private void ApplySharedSpeed(int index)
    {
        if (index >= 0 && index <= 4)
        {
            sharedSpeed = Mathf.Max(0f, sharedSpeed - 1f);
        }
        else if (index >= 5 && index <= 9)
        {
            sharedSpeed += 1f;
            int invIndex = index - 5;
            if (invIndex >= 0 && invIndex < inventory.Length)
            {
                Image img = inventory[invIndex].GetComponent<Image>();
                if (img != null) img.color = new Color(1f, 1f, 1f, 1f);
            }
            specialCollected[invIndex] = true;
            CheckSpecialClear();
        }

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
                // 20초마다 spawnCount 증가
                if (timerController.timerDuration - currentTime >= 20f)
                {
                    spawnCount++;
                    timerController.timerDuration -= 20f;
                    Debug.Log($"spawnCount 증가 → {spawnCount}");
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
