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


    private bool hasStarted = false; // OnGameStart() 1번만 실행되게
    private List<GameObject> currentBleeds = new List<GameObject>();

    void Start()
    {
        if (BDgpManager.gameState == null)
        {
            Debug.LogError("씬에 BDgpManager가 없습니다!");
            return;
        }
    }
    void Update()
    {
        if (!hasStarted && BDgpManager.gameState == "BDStart")
        {
            hasStarted = true;
            OnGameStart();
        }
    }

    void OnGameStart()
    {
        if (bleedPrefabs.Length != bleedProbabilities.Length)
        {
            Debug.LogError("bleedPrefabs와 bleedProbabilities의 길이가 일치하지 않습니다!");
            return;
        }

        SpawnBleeds(initialBleedCount);

        StartCoroutine(RemoveBleedRoutine());
        StartCoroutine(AddBleedRoutine());
    }



    void OnDisable()
    {
        // 오브젝트가 비활성화/파괴될 때 코루틴 중지와 리스트 정리
        StopAllCoroutines();
        currentBleeds.Clear();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        currentBleeds.Clear();
    }

    void SpawnBleeds(int count)
    {
        if (bleedPrefabs == null || bleedPrefabs.Length == 0 || targetSprite == null)
        {
            Debug.LogWarning("bleedPrefabs 또는 targetSprite이 설정되지 않았습니다.");
            return;
        }

        if (BDgpManager.gameState != "BDStart")
            return;

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
           // Debug.Log($"[Remove] frame={Time.frameCount}, Count(after)={currentBleeds.Count}");
            yield return new WaitForSeconds(1.5f);

            // 리스트이 비어있지 않은지 확인
            if (currentBleeds.Count > 0)
            {
                // 안전하게 인덱스 선택
                int index = Random.Range(0, currentBleeds.Count);
                if (index < 0 || index >= currentBleeds.Count)
                    continue;

                GameObject toRemove = currentBleeds[index];

                // 리스트에서 먼저 제거
                currentBleeds.RemoveAt(index);
               // Debug.Log("currentBleeds.Count: - " + currentBleeds.Count);

                // 이미 파괴되었는지 확인
                if (toRemove == null) // Unity의 == 연산은 파괴된 오브젝트도 null로 판정
                {
                    // 이미 파괴되어 있으면 건너뜀
                    continue;
                }

                // 코루틴으로 페이드 후 파괴 처리
                StartCoroutine(FadeToBlackAndDestroy(toRemove, 1f));
            }
        }
    }

    IEnumerator FadeToBlackAndDestroy(GameObject obj, float duration)
    {
        // 시작 시 객체 살아 있는지 확인
        if (obj == null)
            yield break;

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        // 만약 SpriteRenderer가 없으면 바로 파괴
        if (sr == null)
        {
            if (obj != null) Destroy(obj);
            yield break;
        }

        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 매 프레임 파괴 여부 재확인
            if (obj == null) // 파괴된 경우 코루틴 종료
                yield break;

            // sr도 파괴됐는지 확인
            if (sr == null)
            {
                // sr이 파괴되었다면 더 이상 색을 조작할 수 없음 — 객체가 살아 있으면 그냥 파괴
                if (obj != null) Destroy(obj);
                yield break;
            }

            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 안전하게 current sr 색 변경
            Color cur = sr.color; // sr이 살아있음을 확인했으므로 안전
            float r = Mathf.Lerp(originalColor.r, 0f, t);
            float g = Mathf.Lerp(originalColor.g, 0f, t);
            float b = Mathf.Lerp(originalColor.b, 0f, t);
            sr.color = new Color(r, g, b, cur.a);

            yield return null;
        }

        // 루프 끝난 뒤에도 obj가 살아있으면 파괴
        if (obj != null)
            Destroy(obj);
    }

    IEnumerator AddBleedRoutine()
    {
        
        while (true)
        {
            //Debug.Log($"[Add] frame={Time.frameCount}, Count(before)={currentBleeds.Count - 1}, Count(after)={currentBleeds.Count}");
            yield return new WaitForSeconds(1f);
            
            if (BDgpManager.gameState != "BDStart")
                continue;

            // 피가 많을 땐 생성 금지
            if (currentBleeds.Count < 30)
            {
                SpawnBleeds(1);
               // Debug.Log("[Add] 피 생성됨. 현재 개수: " + currentBleeds.Count);
            }
        }

    }


    public void ClearAllBleeds()
    {
        // 리스트에 남아있는 모든 피 오브젝트 삭제
        foreach (GameObject bleed in currentBleeds)
        {
            if (bleed != null)
                Destroy(bleed);
        }
        currentBleeds.Clear();

        // 코루틴 중지
        StopAllCoroutines();

        // 스크립트 비활성화
        enabled = false;
    }

}
