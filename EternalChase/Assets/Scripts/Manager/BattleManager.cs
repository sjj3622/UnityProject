<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
=======
ï»¿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
<<<<<<< HEAD
    public enum ActionType { NONE, ATTACK, MAGIC, DEF, AVOID }
    public enum Turn { PlayerAttack, EnemyAttack }
    SPUM_Prefabs spum, enSpum;
    // ÇÃ·¹ÀÌ¾î 
    [Header("Player UI")]
    [SerializeField] Button attackBtn; // ¹°¸®°ø°İ
    [SerializeField] Button magicBtn; //¸¶¹ı°ø°İ
    [SerializeField] Button defenseBtn; //¹æ¾î
    [SerializeField] Button avoidBtn; // È¸ÇÇ
=======

    public enum ActionType { NONE, ATTACK, MAGIC, DEF, AVOID }
    public enum Turn { PlayerAttack, EnemyAttack }

    //í”Œë ˆì´ì–´
    [Header("Player UI")]
    [SerializeField] Button attackBtn; // ë¬¼ë¦¬ê³µê²©
    [SerializeField] Button magicBtn; // ë§ˆë²•ê³µê²©
    [SerializeField] Button defenseBtn; // ë§‰ê¸°
    [SerializeField] Button avoidBtn; // í¬í”¼
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

    [SerializeField] Image imgAttack;
    [SerializeField] Image imgMagic;
    [SerializeField] Image imgDef;
    [SerializeField] Image imgAvoid;

<<<<<<< HEAD
    // Àû
    [Header("Enemy UI")]
    [SerializeField] Image imgAttacken;
    [SerializeField] Image imgMagicen;
    [SerializeField] Image imgDefen;
    [SerializeField] Image imgAvoiden;

=======
    [Header("Enemy UI")]
    [SerializeField] Button EattackBtn; // ë¬¼ë¦¬ê³µê²©
    [SerializeField] Button EmagicBtn; // ë§ˆë²•ê³µê²©
    [SerializeField] Button EdefenseBtn; // ë§‰ê¸°
    [SerializeField] Button EavoidBtn; // í¬í”¼

    [SerializeField] Image imgEAttack;
    [SerializeField] Image imgEMagic;
    [SerializeField] Image imgEDef;
    [SerializeField] Image imgEAvoid;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

    [Header("HP")]
    [SerializeField] Slider playerHpBar;
    [SerializeField] Slider enemyHpBar;
<<<<<<< HEAD
    [SerializeField] Text countDownText;

    [Header("LOG")]
    [SerializeField] LogUI log;

    float chooseTime = 5f; // ¼±ÅÃ½Ã°£5ÃÊ

    //ÀÓ½Ã·Î ´É·ÂÄ¡
    int PhyPower = 10, magicPower = 10;
    int enPhyPower = 10, enMagicPower = 10;

    int counterDamage = 10;

    int playerHp, playermaxHp;
    int enemyHp;

    bool playerLocked; // ÇÃ·¹ÀÌ¾î°¡ ¼±ÅÃÇß´Â°¡
    bool roundRunning;

    ActionType playerChoice = ActionType.NONE;
    ActionType enemyChoice = ActionType.NONE;
    Turn currentTurn = Turn.PlayerAttack;


    private void Awake()
    {
        playermaxHp = GameManager.playerMaxHp;  // ÇÃ·¹ÀÌ¾îÀÇ ÃÖ´ë hp
        playerHp = GameManager.playerHp;
        enemyHp = 20; // ÀûÀÇ ±âº» hp
        spum = GameObject.FindGameObjectWithTag("Player").GetComponent<SPUM_Prefabs>();

        // ½½¶óÀÌµå¿¡ Àû¿ë
        playerHpBar.maxValue = playermaxHp; // ½½¶óÀÌµå ÃÖ´ë°ª ¼³Á¤
        playerHpBar.value = playerHp; // Ã³À½ ½ÃÀÛ½Ã ½½¶óÀÌµå´Â ²Ë Ã¤¿ì±â
        enemyHpBar.maxValue = enemyHp;
        enemyHpBar.value = enemyHp;

        //¹öÆ° Äİ¹é
=======

    float chooseTime = 5f; // ì„ íƒì‹œê°„ 5ì´ˆ
    // ì„ì‹œë¡œ ëŠ¥ë ¥ì¹˜
    int PhyPower = 10, magicPower = 10;
    int enPhyPower = 10, enMagicPower = 10;

    int counterdamage = 5;

    int playerHp, enemyHp;

    bool playerLocked; // í”Œë ˆì´ì–´ê°€ ì„ íƒí–ˆëŠ”ê°€
    bool roundRunning;

    ActionType playerChoise = ActionType.NONE;
    ActionType enemyChoice = ActionType.NONE;
    Turn currenTurn = Turn.PlayerAttack;

    private void Awake()
    {
        playerHp = GameManager.playerMaxHp; // í”Œë ˆì´ì–´ì˜ ìµœëŒ€ hp
        enemyHp = 50; // ì ì˜ ê¸°ë³¸ hp

        // ìŠ¬ë¼ì´ë“œì— ì ìš©
        playerHpBar.maxValue = playerHp; // ìŠ¬ë¼ì´ë“œ ìµœëŒ€ê°’ ì„¤ì •
        playerHpBar.value = playerHp; // ì²˜ìŒ ì‹œì‘ì‹œ ìŠ¬ë¼ì´ë“œëŠ” ê½‰ì±„ìš°ê¸°
        enemyHpBar.maxValue = enemyHp;
        enemyHpBar.value = enemyHp;

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
        attackBtn.onClick.AddListener(() => PlayerChoose(ActionType.ATTACK));
        magicBtn.onClick.AddListener(() => PlayerChoose(ActionType.MAGIC));
        defenseBtn.onClick.AddListener(() => PlayerChoose(ActionType.DEF));
        avoidBtn.onClick.AddListener(() => PlayerChoose(ActionType.AVOID));
    }

<<<<<<< HEAD
    void Start()
    {
        enSpum = EnemyBoot.enemy.GetComponent<SPUM_Prefabs>();
        enSpum.OverrideControllerInit();

        PlayerController.gameState = "Battle";
=======

    void Start()
    {

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    }


    void Update()
    {

    }

<<<<<<< HEAD
    void PlayerChoose(ActionType type) // ÇÃ·¹ÀÌ¾î Ä³¸¯ÅÍ°¡ ÀÚ½ÅÀÇ ÅÏ»óÈ²¿¡ ¼±ÅÃÇÏ±â
    {
        if (!roundRunning) return; // ÀÌ¹Ì ¹öÆ°À» Å¬¸¯ÇÑ°æ¿ì - ¼±ÅÃ ´ë±â ½Ã°£ Á¾·á

        if (currentTurn == Turn.PlayerAttack)//ÇöÀç ÇÃ·¹ÀÌ¾îÀÇ ÅÏ¿¡ ¸ÂÃçµ¿ÀÛ
=======
    void PlayerChoose(ActionType type)
    {
        if (!roundRunning) return; // ì´ë¯¸ ë²„íŠ¼ í´ë¦­í•œê²½ìš°

        if (currenTurn == Turn.PlayerAttack) // í˜„ì¬ í”Œë ˆì´ì–´ì˜ í„´ì— ë§ì¶°ë™ì‘
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
        {
            if (type != ActionType.ATTACK && type != ActionType.MAGIC) return;
        }
        else
        {
            if (type != ActionType.DEF && type != ActionType.AVOID) return;
        }
<<<<<<< HEAD
        Debug.Log(type);
        playerChoice = type;

=======

        playerChoise = type;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    }

    private void OnEnable()
    {
        StartCoroutine(battleLoop());
    }
<<<<<<< HEAD

    //  ÀüÅõ ¸Ş¼­µå, °ø°İ <-> ¹æ¾î ±³´ë ÀüÅõ
=======
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    IEnumerator battleLoop()
    {
        while (playerHp > 0 && enemyHp > 0)
        {
<<<<<<< HEAD
            // ¶ó¿îµå ÁØºñ
            ResetAllParentColors(); // ¹öÆ°ÀÇ »ö»óÀ» ÃÊ±â»óÅÂ·Î µ¹¸®±â
            playerChoice = ActionType.NONE;
            enemyChoice = ActionType.NONE;
            roundRunning = true;



            // ÅÏ¿¡µû¶ó ¹öÆ° ºñÈ°¼ºÈ­/È°¼ºÈ­
            if (currentTurn == Turn.PlayerAttack)// ÇÃ·¹ÀÌ¾îÀÇ °ø°İ ÅÏ
            {
                EnableButton(att: true, mag: true, def: false, av: false);
                
                
                
                
                
                enemyChoice = (Random.value < 0.5f) ? ActionType.DEF : ActionType.AVOID;


            }
            else  // Àû °ø°İ ÅÏ
=======
            //ë¼ìš´ë“œ ì¤€ë¹„
            ResetAllParentColors(); // ë²„íŠ¼ì˜ ìƒ‰ìƒì„ ì´ˆê¸°ìƒíƒœë¡œ ëŒë¦¬ê¸°
            playerChoise = ActionType.NONE;
            enemyChoice = ActionType.NONE;
            roundRunning = true;

            //í„´ì—ë”°ë¼ ë²„íŠ¼ ë¹„í™œì„±í™”/í™œì„±í™”
            if (currenTurn == Turn.PlayerAttack) // í”Œë ˆì´ì–´ì˜ ê³µê²© í„´
            {
                EnableButton(att: true, mag: true, def: false, av: false);
                enemyChoice = (Random.value < 0.5f) ? ActionType.DEF : ActionType.AVOID;
            }
            else // ì  ê³µê²© í„´
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
            {
                EnableButton(att: false, mag: false, def: true, av: true);
                enemyChoice = (Random.value < 0.5f) ? ActionType.ATTACK : ActionType.MAGIC;
            }
<<<<<<< HEAD
            Debug.Log(playerChoice);


            // ¼±ÅÃ ´ë±â ½Ã°£ 5ÃÊ
            float t = 0f;
            while (t < chooseTime)  // ¼±ÅÃ ´ë±â ½Ã°£ÀÎ 5ÃÊº¸´Ù ÀÛ´Ù¸é °è¼Ó ´ë±âÇÏ±âÀ§ÇÑ 
                                    // ¹İº¹¹®
            {
                t += Time.deltaTime; // deltaTime À» ÀÌ¿ëÇÏ¿© ½Ã°£ÃøÁ¤
                float remain = Mathf.Max(0f, chooseTime - t);
                countDownText.text = remain.ToString("F1"); // F1 ¼Ò¼öÁ¡ ÇÑÀÚ¸®
                yield return null; //¹İº¹¹®ÀÌ ³¡³¯¶§±îÁö  °è¼Ó ´ë±â ÇÏ±â 
            }
            Debug.Log("½Ã°£ Á¾·á");
            // ¼±ÅÃ ½Ã°£ Á¾·á 
            //  ¼±ÅÃ µÈ °ø°İ ¶Ç´Â ¹æ¾î ÀÇ  ¹öÆ° »ö»ó Ç¥Çö

            countDownText.text = string.Empty;

            // ¼±ÅÃ½Ã°£µ¿¾È ÇÃ·¹ÀÌ¾î°¡ ¹Ì¼±ÅÃÇÑ°æ¿ì
            if (playerChoice == ActionType.NONE)
            {
                playerChoice = (currentTurn == Turn.PlayerAttack) ? ActionType.ATTACK : ActionType.DEF;
            }
            LockSelect(); // ¼±ÅÃµÈ ¹öÆ° »ö»ó

            // ÇÇÇØ ÀÔÈ÷±â 
            if (currentTurn == Turn.PlayerAttack)
                DamageResolve(true, playerChoice, enemyChoice);
            else
                DamageResolve(false, playerChoice, enemyChoice);
            // ÇÇÇØ¿¡ µû¸¥ hp¹Ù Ç¥Çö
            playerHpBar.value = playerHp;
            enemyHpBar.value = enemyHp;

            //½ÂÆĞ ¿©ºÎ  hp 0 »óÈ²
            if (playerHp <= 0 || enemyHp <= 0) break;

            // ÅÏ±³´ë
            currentTurn = (currentTurn == Turn.PlayerAttack) ? Turn.EnemyAttack : Turn.PlayerAttack;
            roundRunning = false;


            yield return new WaitForSeconds(1.5f);// ´ÙÀ½ ÅÏ ½ÃÀÛÀü¿¡ 1.5ÃÊ ´ë±â
        }

        if (playerHp <= 0) // ÇÃ·¹ÀÌ¾îÀÇ ÆĞ¹è - °ÔÀÓ¿À¹ö ¾ÀÀ¸·Î ÀÌµ¿
        {
            AnimationActive(spum, PlayerState.DEATH);
            float t = 0f;
            while (t < 3.0f)
            {
                t += Time.deltaTime; // deltaTime À» ÀÌ¿ëÇÏ¿© ½Ã°£ÃøÁ¤
                yield return null; //¹İº¹¹®ÀÌ ³¡³¯¶§±îÁö  °è¼Ó ´ë±â ÇÏ±â 
            }

            SceneManager.LoadScene("GameOver");
        }
        else // ÇÃ·¹ÀÌ¾îÀÇ ½Â¸® ¿ùµå¸ÊÀ¸·Î ÀÌµ¿
        {

            AnimationActive(enSpum, PlayerState.DEATH);
            float t = 0f;
            while (t < 1.5f)
            {
                t += Time.deltaTime; // deltaTime À» ÀÌ¿ëÇÏ¿© ½Ã°£ÃøÁ¤
                yield return null; //¹İº¹¹®ÀÌ ³¡³¯¶§±îÁö  °è¼Ó ´ë±â ÇÏ±â 
            }
            Debug.Log("ÇÃ·¹ÀÌ¾î ÇÇ :" + playerHp);

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


        if (isPlayerAttack) // ÇÃ·¹ÀÌ¾î °ø°İ ÅÏÀÌ³Ä
        {
            bool isMagic = (pa == ActionType.MAGIC);
            ActionType counterType = isMagic ? ActionType.AVOID : ActionType.DEF;
            int attackPower = isMagic ? magicPower : PhyPower;


            // °ø°İ ÆÇÁ¤
            if (ea == counterType)
            {
                Debug.Log("1.ÇÃ·¹ÀÌ¾î ¸¶¹ı°ø°İ Çß´Â°¡? F ->" + isMagic);
                Debug.Log("¸ó½ºÅÍ Å¸ÀÔ -> È¸ÇÇ: T ¹æ¾î:F  ->" + counterType);


                if (log) log.Print("ÇÃ·¹ÀÌ¾î °ø°İ ½ÇÆĞ");
                playerHp -= counterDamage;
                AnimationActive(spum, PlayerState.DAMAGED);
                AnimationActive(enSpum, PlayerState.ATTACK);
            }
            else
            {
                Debug.Log("2.ÇÃ·¹ÀÌ¾î ¸¶¹ı°ø°İ Çß´Â°¡? F ->" + isMagic);
                Debug.Log("¸ó½ºÅÍ Å¸ÀÔ -> È¸ÇÇ: F ¹æ¾î:T  ->" + counterType);

             
                if (log) log.Print("ÇÃ·¹ÀÌ¾î °ø°İ ¼º°ø");
                enemyHp -= attackPower;
                StartCoroutine(PlayerAttackMove());

                //AnimationActive(spum, PlayerState.ATTACK);
                //AnimationActive(enSpum, PlayerState.DAMAGED);
            }
        }
        else  // ÇÃ·¹ÀÌ¾î°¡ ¹æ¾îÅÏÀÎ°æ¿ì
        {
            bool isMagic = (ea == ActionType.MAGIC);
            ActionType counterType = isMagic ? ActionType.AVOID : ActionType.DEF;
            int attackPower = isMagic ? enMagicPower : enPhyPower;

            // °ø°İ ÆÇÁ¤
            if (pa == counterType)
            {

                if (log) log.Print("ÇÃ·¹ÀÌ¾î Ä«¿îÅÍ ¼º°ø");
                enemyHp -= counterDamage;
                AnimationActive(spum, PlayerState.ATTACK);
                AnimationActive(enSpum, PlayerState.DAMAGED);
            }
            else
            {

                if (log) log.Print("ÇÃ·¹ÀÌ¾î Ä«¿îÅÍ ½ÇÆĞ");
                playerHp -= attackPower;
                StartCoroutine(EnemyAttackMove());

                //AnimationActive(spum, PlayerState.DAMAGED);
                //AnimationActive(enSpum, PlayerState.ATTACK);
            }
        }


        IEnumerator PlayerAttackMove()
        {
            // ÇÃ·¹ÀÌ¾î À§Ä¡ ÀúÀå
            Transform playerTr = spum.transform;
            Vector3 startPos = playerTr.position;

            // Àû À§Ä¡ ±âÁØ (Àû ¾ÕÂÊ ¾à°£)
            Vector3 targetPos = EnemyBoot.enemy.transform.position + new Vector3(-1.0f, 0, 0); // Àû ¿ŞÂÊ ¾Õ

            // ¾ÕÀ¸·Î ÀÌµ¿
            float moveTime = 0.3f;
            float t = 0;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                playerTr.position = Vector3.Lerp(startPos, targetPos, t / moveTime);
                yield return null;
            }

            // °ø°İ ¾Ö´Ï¸ŞÀÌ¼Ç
            AnimationActive(spum, PlayerState.ATTACK);
            AnimationActive(enSpum, PlayerState.DAMAGED);
            yield return new WaitForSeconds(0.5f); // Å¸°İ Å¸ÀÌ¹Ö ´ë±â

            // ¿ø·¡ ÀÚ¸®·Î º¹±Í
            t = 0;
            while (t < moveTime)
            {
                t += Time.deltaTime;
                playerTr.position = Vector3.Lerp(targetPos, startPos, t / moveTime);
                yield return null;
            }

            // ´ë±â »óÅÂ·Î º¯°æ
            AnimationActive(spum, PlayerState.IDLE);
        }

        IEnumerator EnemyAttackMove()
        {
            Transform enemyTr = enSpum.transform;
            Vector3 startPos = enemyTr.position;
            Vector3 targetPos = spum.transform.position + new Vector3(1.0f, 0, 0); // ÇÃ·¹ÀÌ¾î ¾ÕÂÊ

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
        // ¹öÆ°¸¸ ÀÖ´Â°æ¿ì
        // attackBtn.GetComponent<Image>().color = 
        //    enemyChoice == ActionType.ATTACK ? Color.black : Color.white;

        // ÇÃ·¹ÀÌ¾î °¡ ¼±ÅÃÇÑ ¹öÆ° °ËÁ¤À¸·Î
        imgAttack.color = playerChoice == ActionType.ATTACK ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgMagic.color = playerChoice == ActionType.MAGIC ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgDef.color = playerChoice == ActionType.DEF ? Color.white : new Color(1f, 1f, 1f, 0f); 
        imgAvoid.color = playerChoice == ActionType.AVOID ? Color.white : new Color(1f, 1f, 1f, 0f); 

        // Àû ¼±ÅÃ °ËÁ¤À¸·Î
        imgAttacken.color = enemyChoice == ActionType.ATTACK ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgMagicen.color = enemyChoice == ActionType.MAGIC ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgDefen.color = enemyChoice == ActionType.DEF ? Color.white : new Color(1f, 1f, 1f, 0f);
        imgAvoiden.color = enemyChoice == ActionType.AVOID ? Color.white : new Color(1f, 1f, 1f, 0f);


        // ÇÃ·¹ÀÌ¾î ¹öÆ° ÀüºÎ Àá±İ
        EnableButton(false, false, false, false);
    }


    void EnableButton(bool att, bool mag, bool def, bool av)  // ÇÃ·¹ÀÌ¾î ¹öÆ° È°¼º ºñÈ°¼º
=======

            //ì„ íƒ ëŒ€ê¸° ì‹œê°„ 5ì´ˆ
            float t = 0f;
            while (t < chooseTime) // ì„ íƒ ëŒ€ê¸° ì‹œê°„ì¸ 5ì´ˆë³´ë‹¤ ì‘ë‹¤ë©´ ê³„ì† ëŒ€ê¸°í•˜ê¸°ìœ„í•œ
                                   // ë°˜ë³µë¬¸
            {
                t += Time.deltaTime; // deltaTimeì„ ì´ìš©í•˜ì—¬ ì‹œê°„ì¸¡ì •
                yield return null; // ë°˜ë³µë¬¸ì´ ëë‚ ë•Œê¹Œì§€ ê³„ì† ëŒ€ê¸° í•˜ê¸°

            }

            // ì„ íƒ ì‹œê°„ ì¢…ë£Œ
            // ì„ íƒëœ ê³µê²© ë˜ëŠ” ë°©ì–´ ì˜ ë²„íŠ¼ ìƒ‰ìƒ í‘œí˜„
            LockSelect(); // ì„ íƒëœ ë²„íŠ¼ ìƒ‰ìƒ

        }
    }

    void LockSelect()
    {
        // í”Œë ˆì´ì–´ ê°€ ì„ íƒí•œ ë²„íŠ¼ ê²€ì •ìœ¼ë¡œ
        // ì  ì„ íƒ ê²€ì •ìœ¼ë¡œ
        // ë²„íŠ¼ë§Œ ìˆëŠ” ê²½ìš° * 8
        //ì  ë²„íŠ¼ ê²€ì •ìƒ‰ìœ¼ë¡œ *4
        //attackBtn.GetComponent<Image>().color = 
        //    enemyChoice == ActionType.ATTACK ? Color.black : Color.white;
        //í”Œë ˆì´ì–´ ë²„íŠ¼ ê²€ì •ìƒ‰ìœ¼ë¡œ *4
        //attackBtn.GetComponent<Image>().color =
        //   playerChoise == ActionType.ATTACK ? Color.black : Color.white;




        // ì´ë¯¸ì§€ë¡œ í•œ ê²½ìš° * 8
        // í”Œë ˆì´ì–´ê°€ ì„ íƒí•œ ë²„íŠ¼ ê²€ì •ìœ¼ë¡œ
        imgAttack.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgMagic.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgDef.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;
        imgAvoid.color = playerChoise == ActionType.ATTACK ? Color.black :Color.white;

        //ì  ì„ íƒ ê²€ì •ìœ¼ë¡œ
        imgEAttack.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEMagic.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEDef.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;
        imgEAvoid.color = playerChoise == ActionType.ATTACK ? Color.black : Color.white;



        // í”Œë ˆì´ì–´ ë²„íŠ¼ ì „ë¶€ ì ê¸ˆ
        EnableButton(false, false, false, false);

    }

    void EnableButton(bool att, bool mag, bool def, bool av)
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    {
        attackBtn.interactable = att;
        magicBtn.interactable = mag;
        defenseBtn.interactable = def;
        avoidBtn.interactable = av;
<<<<<<< HEAD
    }

    void ResetAllParentColors() // ¹öÆ° »ö»ó ¿ø·¡ »óÅÂ·Î µ¹¸®±â
    {
        // ¹öÆ°¸¸ ÀÖ´Â°æ¿ì
        //attackBtn.GetComponent<Image>().color = Color.white;

        //¹öÆ°ÀÇ ¹è°æÀ¸·Î ÀÌ¹ÌÁö°´Ã¼ ÀÖ´Â°æ¿ì
=======

    }

    void ResetAllParentColors()
    {
        //ë²„íŠ¼ë§Œ ìˆëŠ”ê²½ìš°
        //attackBtn.GetComponent<Image>().color = Color.white;
        //ë²„íŠ¼ì˜ ë°°ê²½ìœ¼ë¡œ ì´ë¯¸ì§€ ê°ì²´ê°€ìˆëŠ”ê²½ìš°
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
        imgAttack.color = Color.white;
        imgMagic.color = Color.white;
        imgDef.color = Color.white;
        imgAvoid.color = Color.white;

<<<<<<< HEAD
        imgAttacken.color = Color.white;
        imgMagicen.color = Color.white;
        imgDefen.color = Color.white;
        imgAvoiden.color = Color.white;
    }

}
/*
    5ÃÊÀÇ ½Ã°£µ¿¾È  °ø°İ ¶Ç´Â ¹æ¾î¸¦ ¼±ÅÃÇØ¾ß ÇÑ´Ù. 
    ¼±ÅÃµÈ ±â´É¿¡ È­¸é Ç¥½Ã ,
    ¿¬Ãâ

    ÀûÀº  ·£´ıÇÏ°Ô ¼±ÅÃÇÑ´Ù.  
    
    ÀüÅõ¹æ½ÄÀº  ¹°¸®°ø°İÀ» ¸·À»¼ö ÀÖ´Â°Ç ¹æ¾îÀÌ°í  , ¸¶¹ı°ø°İÀ» ¸·À»¼ö ÀÖ´Â°Ç È¸ÇÇ
    ¸¸¾à ³»°¡ ¹°¸®°ø°İ¼±ÅÃÇÏ°í ÀûÀÌ ¹æ¾î¸¦ ¼±ÅÃÇß´Ù¸é  °ø°İ½ÇÆĞ·Î  ÆĞ³ÎÆ¼ hp -5 °¨¼Ò
        °ø°İ ¼º°ø½Ã °ø°İ·Â ¸¸Å­ hp °¨¼Ò
=======
        imgEAttack.color = Color.white;
        imgEMagic.color = Color.white;
        imgEDef.color = Color.white;
        imgEAvoid.color = Color.white;


    }


}
/*

    5ì´ˆì˜ ì‹œê°„ë™ì•ˆ ê³µê²© ë˜ëŠ” ë°©ì–´ë¥¼ ì„ íƒí•´ì•¼í•œë‹¤
    ì„ íƒëœ ê¸°ëŠ¥ì— í™”ë©´ í‘œì‹œ
    ì—°ì¶œ
    ì ì€ ëœë¤í•˜ê²Œ ì„ íƒí•œë‹¤
    ì „íˆ¬ë°©ì‹ì€ ë¬¼ë¦¬ê³µê²©ì„ ë§‰ì„ìˆ˜ ìˆëŠ”ê±´ ë°©ì–´ì´ê³  , ë§ˆë²•ê³µê²©ì„ ë§‰ì„ìˆ˜ ìˆëŠ”ê±´ í¬í”¼
    ë§Œì•½ ë‚´ê°€ ë¬¼ë¦¬ê³µê²©ì„ íƒí•˜ê³  ì ì´ ë°©ì–´ë¥¼ ì„ íƒí–ˆë‹¤ë©´ ê³µê²©ì‹¤íŒ¨ë¡œ íŒ¨ë„í‹° hp -5ê°ì†Œ
    ê³µê²© ì„±ê³µì‹œ ê³µê²©ë ¥ ë§Œí¼ hp ê°ì†Œ

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
 */