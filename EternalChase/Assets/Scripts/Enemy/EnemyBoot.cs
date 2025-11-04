using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoot : MonoBehaviour
{
    [SerializeField] Transform enemyAnchor; //적이 나타날 위치
    [SerializeField] GameObject enemyPrefab;

    EnemySnapshot enemy;


    void Start()
    {
        enemy = GameManager.Instance.BattleContext.enemy;  // 월드맵에서 조우한 적
        if( enemy == null)  //적 조우하지 않고 배틀맵에 왔다면 
        {
            SceneManager.LoadScene("WorldMap");
            return;
        }

        enemyPrefab = enemy.prefab;
        Instantiate(enemyPrefab, enemyAnchor.position,Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
