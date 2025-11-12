using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedController : MonoBehaviour
{
    [Header("생성할 피 프리팹들 (여러 개 가능)")]
    public GameObject[] bleedPrefabs;

    [Header("각 피 프리팹의 생성 확률 (%) — 총합 100")]
    public float[] bleedProbabilities = { 60f, 25f, 14f, 1f };

    [Header("피를 생성할 기준 오브젝트 (SpriteRenderer 포함)")]
    public SpriteRenderer targetSprite;

    [Header("초기 생성 피 개수")]
    public int initialBleedCount = 20;

    private List<GameObject> currentBleeds = new List<GameObject>();

    

    void Start()
    {
       
        if (BDgpManger.gameState == null)
        {
            Debug.LogError("씬에 DBgpManger가 없습니다!");
            return;
        }

        if (BDgpManger.gameState != "GameStart")
        {
            Debug.Log("게임이 시작되지 않았습니다. gameState = " + BDgpManger.gameState);
            return;
        }

        // 배열 길이 확인
        if (bleedPrefabs.Length != bleedProbabilities.Length)
        {
            Debug.LogError("bleedPrefabs와 bleedProbabilities의 길이가 일치하지 않습니다!");
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

        if (BDgpManger.gameState != "GameStart")
            return; // 게임이 시작되지 않았으면 피 생성 금지

        if(BDgpManger.gameState == "GameOver")
        {

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
                    GameObject randomPrefab = GetWeightedRandomPrefab();

                    GameObject bleed = Instantiate(randomPrefab, worldPos, Quaternion.identity, targetSprite.transform);
                    bleed.name = randomPrefab.name;
                    currentBleeds.Add(bleed);
                    created++;

                    if (bleed.name.Contains("blood4"))
                    {
                        BloodObject bleedObj = bleed.GetComponent<BloodObject>();
                        if (bleedObj == null)
                            bleedObj = bleed.AddComponent<BloodObject>();

                        bleedObj.removableByClick = true;
                        bleedObj.holdTimeRequired = 5f;
                    }

                    // 피 개수 체크
                    if (currentBleeds.Count > 30)
                    {
                        DBgpManger.gameState = "GameOver";
                        Debug.Log("피가 30개 초과! 게임 종료.");
                        return;
                    }
                }
            }
        }
    }

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

        return bleedPrefabs[bleedPrefabs.Length - 1];
    }

    IEnumerator RemoveBleedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            if (currentBleeds.Count > 0)
            {
                int index = Random.Range(0, currentBleeds.Count);
                GameObject toRemove = currentBleeds[index];
                currentBleeds.RemoveAt(index);
                StartCoroutine(FadeToBlackAndDestroy(toRemove, 1f));
            }
        }
    }

    IEnumerator FadeToBlackAndDestroy(GameObject obj, float duration)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float r = Mathf.Lerp(originalColor.r, 0f, t);
                float g = Mathf.Lerp(originalColor.g, 0f, t);
                float b = Mathf.Lerp(originalColor.b, 0f, t);
                sr.color = new Color(r, g, b, originalColor.a);
                yield return null;
            }
        }

        Destroy(obj);
    }

    IEnumerator AddBleedRoutine()
    {
        while (BDgpManger.gameState == "GameStart")
        {
            yield return new WaitForSeconds(1f);
            SpawnBleeds(1);
            if (BDgpManger.gameState == "GameOver")
                yield break; // 게임 종료 시 코루틴 중단
        }
    }
}
