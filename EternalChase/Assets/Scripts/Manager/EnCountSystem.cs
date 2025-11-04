<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
ï»¿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnCountSystem : MonoBehaviour
{
    [Header("Target")]
<<<<<<< HEAD
    [SerializeField] Transform player;  // Ä³¸¯ÅÍ transform
    Rigidbody2D playerRb;  // Ä³¸¯ÅÍ Áß·ÂÄÄÆ÷³ÍÆ® - ¿òÁ÷ÀÓ Ã¼Å©
    [SerializeField] MonoBehaviour playerControl; // Ä³¸¯ÅÍ ÄÁÆ®·Ñ·¯ ½ºÅ©¸³Æ®

    [Header("Enemy Rule")]
    [SerializeField] float checkInterval = 1.0f; // 1ÃÊ¿¡ ÇÑ¹ø¾¿ Àû°ú Á¶¿ì
    [SerializeField] float encounterProb = 0.8f; // 10% È®·ü
    [SerializeField] string battleMapName = "Battle";

    [Header("UI / Preview")]
    [SerializeField] LogUI log; // ·Î±× Ãâ·Â ½ºÅ©¸³Æ®
    [SerializeField] GameObject[] enemyPrefab; // Á¶¿ìÇÑ Àû Àá±ñ º¸¿©ÁÖ±â ¿ë
    [SerializeField] Sprite[] enemySprite; // Á¶¿ìÇÑ Àû ½ºÇÁ¶óÀÌÆ®
    [SerializeField] float previewDuration = 2.0f; // Á¶¿ìÇÑÀû 2ÃÊ ³ëÃâ
=======
    [SerializeField] Transform Player; // ìºë¦­í„° transform
    Rigidbody2D PlayerRb; // ìºë¦­í„° ì¤‘ë ¥ì»´í¬ë„ŒíŠ¸ - ì›€ì§ì„ ì²´í¬
    [SerializeField] MonoBehaviour PlayerControl; // ìºë¦­í„° ì»¨íŠ¸ë¡¤ëŸ¬ ìŠ¤í¬ë¦½íŠ¸

    [Header("Enemy Rule")]
    [SerializeField] float checkInterval = 1.0f; // 1ì´ˆì— í•œë²ˆì”© ì ê³¼ ì¡°ìš°
    [SerializeField] float encounterProb = 0.50f; // 10í”„ë¡œ í™•ë¥ 
    [SerializeField] string battleMapName = "Battle";

    [Header("UI / Preview")]
    [SerializeField] LogUI log; // ë¡œê·¸ ì¶œë ¥ ìŠ¤í¬ë¦½íŠ¸
    [SerializeField] GameObject[] enemyPrefab; // ì¡°ìš°í•œ ì  ì ê¹ ë³´ì—¬ì£¼ê¸° ìš©
    [SerializeField] Sprite[] enemySprite; // ì¡°ìš°í•œ ì  ìŠ¤í”„ë¼ì´íŠ¸
    [SerializeField] float previewDuration = 2.0f; // ì¡°ìš°í•œì  2ì´ˆ ë…¸ì¶œ
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    [SerializeField] Vector3 previewOffset = new Vector3(0, 1.5f, 0);

    bool isMoving;
    bool isEncountering;
    float timer;
<<<<<<< HEAD
    SPUM_Prefabs spum;

    private void Awake()
    {
        isMoving = true;
        spum = player.GetComponent<SPUM_Prefabs>();
    }

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (!isMoving || isEncountering) return; //Àû°ú Á¶¿ì Çß°Å³ª ¿òÁ÷ÀÌÁö¾ÊÀ¸¸é

        // ¿òÁ÷ÀÓ °¨Áö - Ä³¸¯ÅÍÀÇ ¼Óµµ·Î ÃøÁ¤
        bool isRunning = playerRb && playerRb.velocity.sqrMagnitude > 0.01f;
        if (!isRunning) return; //¿òÁ÷ÀÌÁö ¾Ê°í ÀÖ´Ù.
=======

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

        if (!isMoving || isEncountering) return; // ì ê³¼ ì¡°ìš° í–ˆê±°ë‚˜ ì›€ì§ì´ì§€ì•Šìœ¼ë©´

        
        
        bool isRunning = PlayerRb && PlayerRb.velocity.sqrMagnitude > 0.01f;
       

        if (!isRunning) return; //ì›€ì§ì´ì§€ ì•Šê³  ìˆë‹¤/
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;
<<<<<<< HEAD
        if( Random.value < encounterProb)
        {
            // 10% È®·ü·Î Àû°ú Á¶¿ì ÇÔ
            StartCoroutine( DoEncounter() );
        }
    }

    IEnumerator DoEncounter() // Àû°ú Á¶¿ìÇÑ °æ¿ì µ¿ÀÛ ¸Ş¼­µå
    {
        //Ä³¸¯ÅÍ Á¶ÀÛ Á¤Áö ,  ·Î±× Ãâ·Â ,  Àû ¹Ì¸®º¸±â ¸Ê¿¡ Ãâ·Â, ¹èÆ²¾À ÀÌµ¿
        isEncountering = true;

        // Ä³¸¯ÅÍ Á¶ÀÛ Àá½Ã Á¤Áö
        if(playerControl) playerControl.enabled = false;
        if (playerRb) playerRb.velocity = Vector2.zero;

        // ·Î±× Ãâ·Â
        if (log) log.Print("ÀûÀ» ¸¸³µ´Ù : ÀüÅõ ÁØºñ...");

        // Àû ¹Ì¸®º¸±â Ãâ·Â
        // Àû ÇÁ¸®Æé È®·üÀû ¼±ÅÃ
        int[] enemyWeight = { 60, 25, 12, 3 };

        int index = GetWeightIndex(enemyWeight); // °¡ÁßÄ¡ ¸Ş¼­µå
        Debug.Log("ÀÎµ¦½º : "+index);
        GameObject chosen = enemyPrefab[index]; // °¡ÁßÄ¡¿¡ ÀÇÇØ ³ª¿Â ÀÎµ¦½º Àû¿ë

        //¹èÆ² ¾À¿¡ ³Ñ°ÜÁÙ ½º³À¼¦ »ı¼º
        EnemySnapshot snap = new EnemySnapshot();
        // ÀûÀÇ ÀÌ¸§, hp¿Í ½ºÅÈÀº ³ªÁß¿¡ Ãß°¡ ¿©±â¿¡ 

        snap.prefab = chosen;
        Debug.Log("¼±ÅÃµÊ : " + snap);
        GameManager.Instance.BattleContext.enemy = snap; // ¼±ÅÃµÈ ÀûÇÁ¸®Æé Àü¿ªÀ¸·Î ÀúÀå
        
        GameObject preview = Instantiate(chosen);

        var sr = preview.GetComponentInChildren<SpriteRenderer>();
        if( sr && enemySprite != null && enemySprite.Length >0)
        {
            sr.sprite = enemySprite[Random.Range(0, enemySprite.Length)];
        }
            // Ä³¸¯ÅÍ ±ÙÃ³¿¡ ³ªÅ¸³»±â
        preview.transform.position = player.position + previewOffset;

        //===================================================

        var animList = spum.StateAnimationPairs[PlayerState.IDLE.ToString()];
        spum.PlayAnimation(PlayerState.IDLE, 0);

        //================================================
        // 2ÃÊ ´ë±â -  2ÃÊ µ¿¾È Àû ¹Ì¸®º¸±â Ãâ·ÂÀ» À§ÇØ
        yield return new WaitForSeconds(previewDuration);

        if (preview) Destroy(preview); // 2ÃÊµÚ ¹Ì¸®º¸±â Á¦°Å

        SceneManager.LoadScene(battleMapName); // ÀüÅõ ¸ÊÀ¸·Î ÀÌµ¿

    }


    private int GetWeightIndex(int[] enemyWeight)
    {
        int total = 0;
        for(int i =0; i< enemyWeight.Length; i++)
=======
        if (Random.value < encounterProb)
        {
            // 10% í™•ë¥ ë¡œ ì ê³¼ ì¡°ìš° í•¨
            StartCoroutine(DoEncounter());
        }
    }

    IEnumerator DoEncounter() // ì ê³¼ ì¡°ìš°í•œ ê²½ìš° ë™ì‘ ë©”ì„œë“œ
    {
        
        // ìºë¦­í„° ì¡°ì‘ ì •ì§€ , ë¡œê·¸ ì¶œë ¥ , ì  ë¯¸ë¦¬ë³´ê¸° ë§µì— ì¶œë ¥, ë°°í‹€ì”¬ ì´ë™
        isEncountering = true;

        // ìºë¦­í„° ì¡°ì‘
        if (PlayerControl) PlayerControl.enabled = false;
        if (PlayerRb) PlayerRb.velocity = Vector2.zero;

        // ë¡œê·¸ ì¶œë ¥
        if (log) log.Print("ì ì„ ë§Œë‚¬ë‹¤ : ì „íˆ¬ì¤€ë¹„...");



        // ì  ë¯¸ë¦¬ë³´ê¸° ì¶œë ¥
        // ì  í”„ë¦¬í© í™•ë¥ ì  ì„ íƒ
        int[] enemyWeight = { 40, 20, 15, 14, 10, 1 };

        int index = GetWeightIndex(enemyWeight); //ê°€ì¤‘ì¹˜ ë©”ì„œë“œ

        GameObject chosen = enemyPrefab[index]; // ê°€ì¤‘ì¹˜ì— ì˜í•´ ë‚˜ì˜¨ ì¸í…ìŠ¤ ì ìš©

        //GameObject preview = null;
        
        //ë°°í‹€ ì”¬ì— ë„˜ê²¨ì¤„ ìŠ¤ëƒ…ìƒ· ìƒì„±
        EnemySnapshot snap = new EnemySnapshot();
        //ì ì˜ ì´ë¦„, hpì™€ ìŠ¤íƒ¯ì€ ë‚˜ì¤‘ì— ì¶”ê°€ ì—¬ê¸°ì— 

        snap.prefab = chosen;

        GameManager.Instance.BattleContext.enemy = snap;

       
            GameObject preview = Instantiate(chosen);

            var sr = preview.GetComponentInChildren<SpriteRenderer>();
            if (sr && enemySprite != null && enemySprite.Length > 0)
            {
                sr.sprite = enemySprite[Random.Range(0, enemySprite.Length)];
            }
            preview.transform.position = Player.position + previewOffset;
        
        //2ì´ˆëŒ€ê¸° - 2ì´ˆ ë™ì•ˆ ì  ë¯¸ë¦¬ë³´ê¸° ì¶œë ¥ì„ ìœ„í•´
        yield return new WaitForSeconds(previewDuration);

        if (preview) Destroy(preview); //  2ì´ˆë’¤ ë¯¸ë¦¬ë³´ê¸° ì œê±°

        SceneManager.LoadScene(battleMapName); // ì „íˆ¬ ë§µìœ¼ë¡œ ì´ë™
    }

    private int GetWeightIndex(int[] enemyWeight)
    {
        int total = 0;
        for(int i=0; i < enemyWeight.Length; i++)
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
            total += enemyWeight[i];

        int roll = Random.Range(0, total);
        int acc = 0;
<<<<<<< HEAD
        for(int i =0;i< enemyWeight.Length; i++)
=======
        for(int i = 0; i <enemyWeight.Length; i++)
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
        {
            acc += enemyWeight[i];
            if (roll < acc) return i;
        }

        return enemyWeight.Length - 1;
<<<<<<< HEAD
    }


}
=======
        
    }



}
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
