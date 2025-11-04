using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnCountSystem : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform Player; // 캐릭터 transform
    Rigidbody2D PlayerRb; // 캐릭터 중력컴포넌트 - 움직임 체크
    [SerializeField] MonoBehaviour PlayerControl; // 캐릭터 컨트롤러 스크립트

    [Header("Enemy Rule")]
    [SerializeField] float checkInterval = 1.0f; // 1초에 한번씩 적과 조우
    [SerializeField] float encounterProb = 0.50f; // 10프로 확률
    [SerializeField] string battleMapName = "Battle";

    [Header("UI / Preview")]
    [SerializeField] LogUI log; // 로그 출력 스크립트
    [SerializeField] GameObject[] enemyPrefab; // 조우한 적 잠깐 보여주기 용
    [SerializeField] Sprite[] enemySprite; // 조우한 적 스프라이트
    [SerializeField] float previewDuration = 2.0f; // 조우한적 2초 노출
    [SerializeField] Vector3 previewOffset = new Vector3(0, 1.5f, 0);

    bool isMoving;
    bool isEncountering;
    float timer;

    public void Awake()
    {
        isMoving = true;
    }


    void Start()
    {
        PlayerRb = Player.GetComponent<Rigidbody2D>();
        
    }


    void Update()
    {

        if (!isMoving || isEncountering) return; // 적과 조우 했거나 움직이지않으면

        
        
        bool isRunning = PlayerRb && PlayerRb.velocity.sqrMagnitude > 0.01f;
       

        if (!isRunning) return; //움직이지 않고 있다/

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;
        if (Random.value < encounterProb)
        {
            // 10% 확률로 적과 조우 함
            StartCoroutine(DoEncounter());
        }
    }

    IEnumerator DoEncounter() // 적과 조우한 경우 동작 메서드
    {
        
        // 캐릭터 조작 정지 , 로그 출력 , 적 미리보기 맵에 출력, 배틀씬 이동
        isEncountering = true;

        // 캐릭터 조작
        if (PlayerControl) PlayerControl.enabled = false;
        if (PlayerRb) PlayerRb.velocity = Vector2.zero;

        // 로그 출력
        if (log) log.Print("적을 만났다 : 전투준비...");



        // 적 미리보기 출력
        // 적 프리펩 확률적 선택
        int[] enemyWeight = { 40, 20, 15, 14, 10, 1 };

        int index = GetWeightIndex(enemyWeight); //가중치 메서드

        GameObject chosen = enemyPrefab[index]; // 가중치에 의해 나온 인텍스 적용

        //GameObject preview = null;
        
        //배틀 씬에 넘겨줄 스냅샷 생성
        EnemySnapshot snap = new EnemySnapshot();
        //적의 이름, hp와 스탯은 나중에 추가 여기에 

        snap.prefab = chosen;

        GameManager.Instance.BattleContext.enemy = snap;

       
            GameObject preview = Instantiate(chosen);

            var sr = preview.GetComponentInChildren<SpriteRenderer>();
            if (sr && enemySprite != null && enemySprite.Length > 0)
            {
                sr.sprite = enemySprite[Random.Range(0, enemySprite.Length)];
            }
            preview.transform.position = Player.position + previewOffset;
        
        //2초대기 - 2초 동안 적 미리보기 출력을 위해
        yield return new WaitForSeconds(previewDuration);

        if (preview) Destroy(preview); //  2초뒤 미리보기 제거

        SceneManager.LoadScene(battleMapName); // 전투 맵으로 이동
    }

    private int GetWeightIndex(int[] enemyWeight)
    {
        int total = 0;
        for(int i=0; i < enemyWeight.Length; i++)
            total += enemyWeight[i];

        int roll = Random.Range(0, total);
        int acc = 0;
        for(int i = 0; i <enemyWeight.Length; i++)
        {
            acc += enemyWeight[i];
            if (roll < acc) return i;
        }

        return enemyWeight.Length - 1;
        
    }



}