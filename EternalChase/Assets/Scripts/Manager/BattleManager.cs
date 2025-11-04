using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public enum ActionType { NONE, ATTACK, MAGIC, DEF, AVOID }
    public enum Turn { PlayerAttack, EnemyAttack }

    //플레이어
    [Header("Player UI")]
    [SerializeField] Button attackBtn; // 물리공격
    [SerializeField] Button magicBtn; // 마법공격
    [SerializeField] Button defenseBtn; // 막기
    [SerializeField] Button avoidBtn; // 희피

    [SerializeField] Image imgAttack;
    [SerializeField] Image imgMagic;
    [SerializeField] Image imgDef;
    [SerializeField] Image imgAvoid;

    [Header("Enemy UI")]
    [SerializeField] Button EattackBtn; // 물리공격
    [SerializeField] Button EmagicBtn; // 마법공격
    [SerializeField] Button EdefenseBtn; // 막기
    [SerializeField] Button EavoidBtn; // 희피

    [SerializeField] Image imgEAttack;
    [SerializeField] Image imgEMagic;
    [SerializeField] Image imgEDef;
    [SerializeField] Image imgEAvoid;

    [Header("HP")]
    [SerializeField] Slider playerHpBar;
    [SerializeField] Slider enemyHpBar;

    float chooseTime = 5f; // 선택시간 5초
    // 임시로 능력치
    int PhyPower = 10, magicPower = 10;
    int enPhyPower = 10, enMagicPower = 10;

    int counterdamage = 5;

    int playerHp, enemyHp;

    bool playerLocked; // 플레이어가 선택했는가
    bool roundRunning;

    ActionType playerChoise = ActionType.NONE;
    ActionType enemyChoice = ActionType.NONE;
    Turn currenTurn = Turn.PlayerAttack;

    private void Awake()
    {
        playerHp = GameManager.playerMaxHp; // 플레이어의 최대 hp
        enemyHp = 50; // 적의 기본 hp

        // 슬라이드에 적용
        playerHpBar.maxValue = playerHp; // 슬라이드 최대값 설정
        playerHpBar.value = playerHp; // 처음 시작시 슬라이드는 꽉채우기
        enemyHpBar.maxValue = enemyHp;
        enemyHpBar.value = enemyHp;

        attackBtn.onClick.AddListener(() => PlayerChoose(ActionType.ATTACK));
        magicBtn.onClick.AddListener(() => PlayerChoose(ActionType.MAGIC));
        defenseBtn.onClick.AddListener(() => PlayerChoose(ActionType.DEF));
        avoidBtn.onClick.AddListener(() => PlayerChoose(ActionType.AVOID));
    }


    void Start()
    {

    }


    void Update()
    {

    }

    void PlayerChoose(ActionType type)
    {
        if (!roundRunning) return; // 이미 버튼 클릭한경우

        if (currenTurn == Turn.PlayerAttack) // 현재 플레이어의 턴에 맞춰동작
        {
            if (type != ActionType.ATTACK && type != ActionType.MAGIC) return;
        }
        else
        {
            if (type != ActionType.DEF && type != ActionType.AVOID) return;
        }

        playerChoise = type;
    }

    private void OnEnable()
    {
        StartCoroutine(battleLoop());
    }
    IEnumerator battleLoop()
    {
        while (playerHp > 0 && enemyHp > 0)
        {
            //라운드 준비
            ResetAllParentColors(); // 버튼의 색상을 초기상태로 돌리기
            playerChoise = ActionType.NONE;
            enemyChoice = ActionType.NONE;
            roundRunning = true;

            //턴에따라 버튼 비활성화/활성화
            if (currenTurn == Turn.PlayerAttack) // 플레이어의 공격 턴
            {
                EnableButton(att: true, mag: true, def: false, av: false);
                enemyChoice = (Random.value < 0.5f) ? ActionType.DEF : ActionType.AVOID;
            }
            else // 적 공격 턴
            {
                EnableButton(att: false, mag: false, def: true, av: true);
                enemyChoice = (Random.value < 0.5f) ? ActionType.ATTACK : ActionType.MAGIC;
            }

            //선택 대기 시간 5초
            float t = 0f;
            while (t < chooseTime) // 선택 대기 시간인 5초보다 작다면 계속 대기하기위한
                                   // 반복문
            {
                t += Time.deltaTime; // deltaTime을 이용하여 시간측정
                yield return null; // 반복문이 끝날때까지 계속 대기 하기

            }

            // 선택 시간 종료
            // 선택된 공격 또는 방어 의 버튼 색상 표현
            LockSelect(); // 선택된 버튼 색상

        }
    }

    void LockSelect()
    {
        // 플레이어 가 선택한 버튼 검정으로
        // 적 선택 검정으로
        // 버튼만 있는 경우 * 8
        //적 버튼 검정색으로 *4
        //attackBtn.GetComponent<Image>().color = 
        //    enemyChoice == ActionType.ATTACK ? Color.black : Color.white;
        //플레이어 버튼 검정색으로 *4
        //attackBtn.GetComponent<Image>().color =
        //   playerChoise == ActionType.ATTACK ? Color.black : Color.white;




        // 이미지로 한 경우 * 8
        // 플레이어가 선택한 버튼 검정으로
        imgAttack.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgMagic.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgDef.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgAvoid.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;

        //적 선택 검정으로
        imgEAttack.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEMagic.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEDef.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEAvoid.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;



        // 플레이어 버튼 전부 잠금
        EnableButton(false, false, false, false);

    }

    void EnableButton(bool att, bool mag, bool def, bool av)
    {
        attackBtn.interactable = att;
        magicBtn.interactable = mag;
        defenseBtn.interactable = def;
        avoidBtn.interactable = av;

    }

    void ResetAllParentColors()
    {
        //버튼만 있는경우
        //attackBtn.GetComponent<Image>().color = Color.white;
        //버튼의 배경으로 이미지 객체가있는경우
        imgAttack.color = Color.white;
        imgMagic.color = Color.white;
        imgDef.color = Color.white;
        imgAvoid.color = Color.white;

        imgEAttack.color = Color.white;
        imgEMagic.color = Color.white;
        imgEDef.color = Color.white;
        imgEAvoid.color = Color.white;


    }


}
/*

    5초의 시간동안 공격 또는 방어를 선택해야한다
    선택된 기능에 화면 표시
    연출
    적은 랜덤하게 선택한다
    전투방식은 물리공격을 막을수 있는건 방어이고 , 마법공격을 막을수 있는건 희피
    만약 내가 물리공격선택하고 적이 방어를 선택했다면 공격실패로 패널티 hp -5감소
    공격 성공시 공격력 만큼 hp 감소

 */