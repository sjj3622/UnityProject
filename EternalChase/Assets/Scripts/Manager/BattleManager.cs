using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum ActionType { NONE, ATTACK, MAGIC, DEF, AVOID }
    public enum Turn { PlayerAttack, EnemyAttack }
    SPUM_Prefabs spum, enSpum;
    // 플레이어 
    [Header("Player UI")]
    [SerializeField] Button attackBtn; // 물리공격
    [SerializeField] Button magicBtn; //마법공격
    [SerializeField] Button defenseBtn; //방어
    [SerializeField] Button avoidBtn; // 회피

    [SerializeField] Image imgAttack;
    [SerializeField] Image imgMagic;
    [SerializeField] Image imgDef;
    [SerializeField] Image imgAvoid;

    // 적
    [Header("Enemy UI")]
    [SerializeField] Image imgAttacken;
    [SerializeField] Image imgMagicen;
    [SerializeField] Image imgDefen;
    [SerializeField] Image imgAvoiden;


    [Header("HP")]
    [SerializeField] Slider playerHpBar;
    [SerializeField] Slider enemyHpBar;
    [SerializeField] Text countDownText;

    [Header("LOG")]
    [SerializeField] LogUI log;

    float chooseTime = 5f; // 선택시간5초

    //임시로 능력치
    int PhyPower = 10, magicPower = 10;
    int enPhyPower = 10, enMagicPower = 10;

    int counterDamage = 10;

    int playerHp, playermaxHp;
    int enemyHp;

    bool playerLocked; // 플레이어가 선택했는가
    bool roundRunning;

    ActionType playerChoice = ActionType.NONE;
    ActionType enemyChoice = ActionType.NONE;
    Turn currentTurn = Turn.PlayerAttack;


    private void Awake()
    {
        playermaxHp = GameManager.playerMaxHp;  // 플레이어의 최대 hp
        playerHp = GameManager.playerHp;
        enemyHp = 20; // 적의 기본 hp
        spum = GameObject.FindGameObjectWithTag("Player").GetComponent<SPUM_Prefabs>();

        // 슬라이드에 적용
        playerHpBar.maxValue = playermaxHp; // 슬라이드 최대값 설정
        playerHpBar.value = playerHp; // 처음 시작시 슬라이드는 꽉 채우기
        enemyHpBar.maxValue = enemyHp;
        enemyHpBar.value = enemyHp;

        //버튼 콜백
        attackBtn.onClick.AddListener(() => PlayerChoose(ActionType.ATTACK));
        magicBtn.onClick.AddListener(() => PlayerChoose(ActionType.MAGIC));
        defenseBtn.onClick.AddListener(() => PlayerChoose(ActionType.DEF));
        avoidBtn.onClick.AddListener(() => PlayerChoose(ActionType.AVOID));
    }

    void Start()
    {
        enSpum = EnemyBoot.enemy.GetComponent<SPUM_Prefabs>();
        enSpum.OverrideControllerInit();

        PlayerController.gameState = "Battle";
    }


    void Update()
    {

    }

    void PlayerChoose(ActionType type) // 플레이어 캐릭터가 자신의 턴상황에 선택하기
    {
        if (!roundRunning) return; // 이미 버튼을 클릭한경우 - 선택 대기 시간 종료

        if (currentTurn == Turn.PlayerAttack)//현재 플레이어의 턴에 맞춰동작
        {
            if (type != ActionType.ATTACK && type != ActionType.MAGIC) return;
        }
        else
        {
            if (type != ActionType.DEF && type != ActionType.AVOID) return;
        }
        Debug.Log(type);
        playerChoice = type;

    }

    private void OnEnable()
    {
        StartCoroutine(battleLoop());
    }

    //  전투 메서드, 공격 <-> 방어 교대 전투
    IEnumerator battleLoop()
    {
        while (playerHp > 0 && enemyHp > 0)
        {
            // 라운드 준비
            ResetAllParentColors(); // 버튼의 색상을 초기상태로 돌리기
            playerChoice = ActionType.NONE;
            enemyChoice = ActionType.NONE;
            roundRunning = true;



            // 턴에따라 버튼 비활성화/활성화
            if (currentTurn == Turn.PlayerAttack)// 플레이어의 공격 턴
            {
                EnableButton(att: true, mag: true, def: false, av: false);
                
                
                
                
                
                enemyChoice = (Random.value < 0.5f) ? ActionType.DEF : ActionType.AVOID;


            }
            else  // 적 공격 턴
            {
                EnableButton(att: false, mag: false, def: true, av: true);
                enemyChoice = (Random.value < 0.5f) ? ActionType.ATTACK : ActionType.MAGIC;
            }
            Debug.Log(playerChoice);


            // 선택 대기 시간 5초
            float t = 0f;
            while (t < chooseTime)  // 선택 대기 시간인 5초보다 작다면 계속 대기하기위한 
                                    // 반복문
            {
                t += Time.deltaTime; // deltaTime 을 이용하여 시간측정
                float remain = Mathf.Max(0f, chooseTime - t);
                countDownText.text = remain.ToString("F1"); // F1 소수점 한자리
                yield return null; //반복문이 끝날때까지  계속 대기 하기 
            }
            Debug.Log("시간 종료");
            // 선택 시간 종료 
            //  선택 된 공격 또는 방어 의  버튼 색상 표현

            countDownText.text = string.Empty;

            // 선택시간동안 플레이어가 미선택한경우
            if (playerChoice == ActionType.NONE)
            {
                playerChoice = (currentTurn == Turn.PlayerAttack) ? ActionType.ATTACK : ActionType.DEF;
            }
            LockSelect(); // 선택된 버튼 색상

            // 피해 입히기 
            if (currentTurn == Turn.PlayerAttack)
                DamageResolve(true, playerChoice, enemyChoice);
            else
                DamageResolve(false, playerChoice, enemyChoice);
            // 피해에 따른 hp바 표현
            playerHpBar.value = playerHp;
            enemyHpBar.value = enemyHp;

            //승패 여부  hp 0 상황
            if (playerHp <= 0 || enemyHp <= 0) break;

            // 턴교대
            currentTurn = (currentTurn == Turn.PlayerAttack) ? Turn.EnemyAttack : Turn.PlayerAttack;
            roundRunning = false;


            yield return new WaitForSeconds(1.5f);// 다음 턴 시작전에 1.5초 대기
        }

        if (playerHp <= 0) // 플레이어의 패배 - 게임오버 씬으로 이동
        {
            AnimationActive(spum, PlayerState.DEATH);
            float t = 0f;
            while (t < 3.0f)
            {
                t += Time.deltaTime; // deltaTime 을 이용하여 시간측정
                yield return null; //반복문이 끝날때까지  계속 대기 하기 
            }

            SceneManager.LoadScene("GameOver");
        }
        else // 플레이어의 승리 월드맵으로 이동
        {

            AnimationActive(enSpum, PlayerState.DEATH);
            float t = 0f;
            while (t < 1.5f)
            {
                t += Time.deltaTime; // deltaTime 을 이용하여 시간측정
                yield return null; //반복문이 끝날때까지  계속 대기 하기 
            }
            Debug.Log("플레이어 피 :" + playerHp);

            GameManager.playerHp = playerHp;

            SceneManager.LoadScene("WorldMap");

            PlayerController.gameState = "playing";
        }


    }



    void AnimationActive(SPUM_Prefabs target, PlayerState playerState)
    {
        //===================================================


        var animList = target.StateAnimationPairs[playerState.ToString()];
        target.PlayAnimation(playerState, 0);

        //================================================
    }
    void DamageResolve(bool isPlayerAttack, ActionType pa, ActionType ea)
    {


        if (isPlayerAttack) // 플레이어 공격 턴이냐
        {
            bool isMagic = (pa == ActionType.MAGIC);
            ActionType counterType = isMagic ? ActionType.AVOID : ActionType.DEF;
            int attackPower = isMagic ? magicPower : PhyPower;


            // 공격 판정
            if (ea == counterType)
            {
                Debug.Log("1.플레이어 마법공격 했는가? F ->" + isMagic);
                Debug.Log("몬스터 타입 -> 회피: T 방어:F  ->" + counterType);


                if (log) log.Print("플레이어 공격 실패");
                playerHp -= counterDamage;
                AnimationActive(spum, PlayerState.DAMAGED);
                AnimationActive(enSpum, PlayerState.ATTACK);
            }
            else
            {
                Debug.Log("2.플레이어 마법공격 했는가? F ->" + isMagic);
                Debug.Log("몬스터 타입 -> 회피: F 방어:T  ->" + counterType);

             
                if (log) log.Print("플레이어 공격 성공");
                enemyHp -= attackPower;
                StartCoroutine(PlayerAttackMove());

                //AnimationActive(spum, PlayerState.ATTACK);
                //AnimationActive(enSpum, PlayerState.DAMAGED);
            }
        }
        else  // 플레이어가 방어턴인경우
        {
            bool isMagic = (ea == ActionType.MAGIC);
            ActionType counterType = isMagic ? ActionType.AVOID : ActionType.DEF;
            int attackPower = isMagic ? enMagicPower : enPhyPower;

            // 공격 판정
            if (pa == counterType)
            {

                if (log) log.Print("플레이어 카운터 성공");
                enemyHp -= counterDamage;
                AnimationActive(spum, PlayerState.ATTACK);
                AnimationActive(enSpum, PlayerState.DAMAGED);
            }
            else
            {

                if (log) log.Print("플레이어 카운터 실패");
                playerHp -= attackPower;
                StartCoroutine(EnemyAttackMove());

                //AnimationActive(spum, PlayerState.DAMAGED);
                //AnimationActive(enSpum, PlayerState.ATTACK);
            }
        }


        IEnumerator PlayerAttackMove()
        {
            // 플레이어 위치 저장
            Transform playerTr = spum.transform;
            Vector3 startPos = playerTr.position;

            // 적 위치 기준 (적 앞쪽 약간)
            Vector3 targetPos = EnemyBoot.enemy.transform.position + new Vector3(-1.0f, 0, 0); // 적 왼쪽 앞

            // 앞으로 이동
            float moveTime = 0.3f;
            float t = 0;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                playerTr.position = Vector3.Lerp(startPos, targetPos, t / moveTime);
                yield return null;
            }

            // 공격 애니메이션
            AnimationActive(spum, PlayerState.ATTACK);
            AnimationActive(enSpum, PlayerState.DAMAGED);
            yield return new WaitForSeconds(0.5f); // 타격 타이밍 대기

            // 원래 자리로 복귀
            t = 0;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                playerTr.position = Vector3.Lerp(targetPos, startPos, t / moveTime);
                yield return null;
            }

            // 대기 상태로 변경
            AnimationActive(spum, PlayerState.IDLE);
        }

        IEnumerator EnemyAttackMove()
        {
            Transform enemyTr = enSpum.transform;
            Vector3 startPos = enemyTr.position;
            Vector3 targetPos = spum.transform.position + new Vector3(1.0f, 0, 0); // 플레이어 앞쪽

            float moveTime = 0.3f;
            float t = 0f;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                enemyTr.position = Vector3.Lerp(startPos, targetPos, t / moveTime);
                yield return null;
            }

            AnimationActive(enSpum, PlayerState.ATTACK);
            AnimationActive(spum, PlayerState.DAMAGED);
            yield return new WaitForSeconds(0.5f);

            t = 0f;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                enemyTr.position = Vector3.Lerp(targetPos, startPos, t / moveTime);
                yield return null;
            }

            AnimationActive(enSpum, PlayerState.IDLE);
        }


    }



    void LockSelect()
    {
        // 버튼만 있는경우
        // attackBtn.GetComponent<Image>().color = 
        //    enemyChoice == ActionType.ATTACK ? Color.black : Color.white;

        // 플레이어 가 선택한 버튼 검정으로
        imgAttack.color = playerChoice == ActionType.ATTACK ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgMagic.color = playerChoice == ActionType.MAGIC ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgDef.color = playerChoice == ActionType.DEF ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgAvoid.color = playerChoice == ActionType.AVOID ? Color.white : new Color(1f, 1f, 1f, 0f); 

        // 적 선택 검정으로
        imgAttacken.color = enemyChoice == ActionType.ATTACK ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgMagicen.color = enemyChoice == ActionType.MAGIC ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgDefen.color = enemyChoice == ActionType.DEF ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgAvoiden.color = enemyChoice == ActionType.AVOID ? Color.white : new Color(1f, 1f, 1f, 0f);


        // 플레이어 버튼 전부 잠금
        EnableButton(false, false, false, false);
    }


    void EnableButton(bool att, bool mag, bool def, bool av)  // 플레이어 버튼 활성 비활성
    {
        attackBtn.interactable = att;
        magicBtn.interactable = mag;
        defenseBtn.interactable = def;
        avoidBtn.interactable = av;
    }

    void ResetAllParentColors() // 버튼 색상 원래 상태로 돌리기
    {
        // 버튼만 있는경우
        //attackBtn.GetComponent<Image>().color = Color.white;

        //버튼의 배경으로 이미지객체 있는경우
        imgAttack.color = Color.white;
        imgMagic.color = Color.white;
        imgDef.color = Color.white;
        imgAvoid.color = Color.white;

        imgAttacken.color = Color.white;
        imgMagicen.color = Color.white;
        imgDefen.color = Color.white;
        imgAvoiden.color = Color.white;
    }

}
/*
    5초의 시간동안  공격 또는 방어를 선택해야 한다. 
    선택된 기능에 화면 표시 ,
    연출

    적은  랜덤하게 선택한다.  
    
    전투방식은  물리공격을 막을수 있는건 방어이고  , 마법공격을 막을수 있는건 회피
    만약 내가 물리공격선택하고 적이 방어를 선택했다면  공격실패로  패널티 hp -5 감소
        공격 성공시 공격력 만큼 hp 감소
 */