<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
=======
ï»¿using System.Collections;
using System.Collections.Generic;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoot : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] Transform enemyAnchor; // ÀûÀÌ ³ªÅ¸³¯ À§Ä¡
    [SerializeField] GameObject enemyPrefab;

    EnemySnapshot enemyshot;
    public static GameObject enemy;
    private void Awake()
    {
        enemyshot = GameManager.Instance.BattleContext.enemy; // ¿ùµå¸Ê¿¡¼­ Á¶¿ìÇÑ Àû
        if (enemyshot == null) // Àû Á¶¿ìÇÏÁö ¾Ê°í ¹èÆ²¸Ê¿¡ ¿Ô´Ù¸é
=======
    [SerializeField] Transform enemyAnchor; //ì ì´ ë‚˜íƒ€ë‚  ìœ„ì¹˜
    [SerializeField] GameObject enemyPrefab;

    EnemySnapshot enemy;


    void Start()
    {
        enemy = GameManager.Instance.BattleContext.enemy;  // ì›”ë“œë§µì—ì„œ ì¡°ìš°í•œ ì 
        if( enemy == null)  //ì  ì¡°ìš°í•˜ì§€ ì•Šê³  ë°°í‹€ë§µì— ì™”ë‹¤ë©´ 
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
        {
            SceneManager.LoadScene("WorldMap");
            return;
        }

<<<<<<< HEAD
        enemyPrefab = enemyshot.prefab;
        enemy = Instantiate(enemyPrefab, enemyAnchor.position, Quaternion.identity);
    }
    void Start()
    {
        


    }

    
=======
        enemyPrefab = enemy.prefab;
        Instantiate(enemyPrefab, enemyAnchor.position,Quaternion.identity);

    }

    // Update is called once per frame
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    void Update()
    {
        
    }
}
