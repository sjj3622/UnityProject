using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedController : MonoBehaviour
{
    [Header("생성할 피 프리팹들 (여러 개 가능)")]
    public GameObject[] bleedPrefabs;

    [Header("각 피 프리팹의 생성 확률 (%) — 총합 100")]
    public float[] bleedProbabilities = { 60f, 20f, 15f, 5f };

    [Header("피를 생성할 기준 오브젝트 (SpriteRenderer 포함)")]
    public SpriteRenderer targetSprite;

    [Header("초기 생성 피 개수")]
    public int initialBleedCount = 20;

    private List<GameObject> currentBleeds = new List<GameObject>();

    void Start()
    {
        // 배열 길이 확인
        if (bleedPrefabs.Length != bleedProbabilities.Length)
        {
            Debug.LogError(" bleedPrefabs와 bleedProbabilities의 길이가 일치하지 않습니다!");
            return;
        }

        // 초기 피 생성
        SpawnBleeds(initialBleedCount);

        // 코루틴 시작
        StartCoroutine(RemoveBleedRoutine());
        StartCoroutine(AddBleedRoutine());
    }

    void SpawnBleeds(int count)
    {
        if (bleedPrefabs == null || bleedPrefabs.Length == 0 || targetSprite == null)
        {
            Debug.LogWarning("bleedPrefabs 또는 targetSprite이 설정되지 않았습니다.");
            return;
        }

        Texture2D tex = targetSprite.sprite.texture;
        Rect spriteRect = targetSprite.sprite.textureRect;
        Vector2 pivot = targetSprite.sprite.pivot;
        Bounds bounds = targetSprite.bounds;

        int created = 0;
        int attempts = 0;

        while (created < count && attempts < count * 20)
        {
            attempts++;

            float randX = Random.Range(bounds.min.x, bounds.max.x);
            float randY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 worldPos = new Vector3(randX, randY, targetSprite.transform.position.z);
            Vector3 localPos = targetSprite.transform.InverseTransformPoint(worldPos);

            float ppu = targetSprite.sprite.pixelsPerUnit;
            int texX = Mathf.RoundToInt(spriteRect.x + pivot.x + localPos.x * ppu);
            int texY = Mathf.RoundToInt(spriteRect.y + pivot.y + localPos.y * ppu);

            if (texX >= spriteRect.x && texX < spriteRect.xMax &&
                texY >= spriteRect.y && texY < spriteRect.yMax)
            {
                Color pixelColor = tex.GetPixel(texX, texY);
                if (pixelColor.a > 0.1f)
                {
                    // 가중 랜덤으로 bleedPrefab 선택
                    GameObject randomPrefab = GetWeightedRandomPrefab();

                    GameObject bleed = Instantiate(randomPrefab, worldPos, Quaternion.identity, targetSprite.transform);
                    bleed.name = randomPrefab.name;
                    currentBleeds.Add(bleed);
                    created++;
                }
            }
        }

        // Debug.Log($"{created}개의 피 생성 완료 (시도 횟수 {attempts})");
    }

    //  가중 확률 랜덤 선택 함수
    GameObject GetWeightedRandomPrefab()
    {
        float total = 0;
        foreach (float p in bleedProbabilities)
            total += p;

        float randomValue = Random.Range(0, total);
        float cumulative = 0;

        for (int i = 0; i < bleedPrefabs.Length; i++)
        {
            cumulative += bleedProbabilities[i];
            if (randomValue <= cumulative)
                return bleedPrefabs[i];
        }

        // fallback (이론상 도달 X)
        return bleedPrefabs[bleedPrefabs.Length - 1];
    }

    IEnumerator RemoveBleedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (currentBleeds.Count > 0)
            {
                int index = Random.Range(0, currentBleeds.Count);
                GameObject toRemove = currentBleeds[index];
                currentBleeds.RemoveAt(index);
                Destroy(toRemove);
            }
        }
    }

    IEnumerator AddBleedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SpawnBleeds(1);
        }
    }
}
