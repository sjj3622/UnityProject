using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoot : MonoBehaviour
{
    [SerializeField] Transform enemyAnchor; // 적이 나타날 위치
    [SerializeField] GameObject enemyPrefab;

    EnemySnapshot enemyshot;
    public static GameObject enemy;
    private void Awake()
    {
        enemyshot = GameManager.Instance.BattleContext.enemy; // 월드맵에서 조우한 적
        if (enemyshot == null) // 적 조우하지 않고 배틀맵에 왔다면
        {
            SceneManager.LoadScene("WorldMap");
            return;
        }

        enemyPrefab = enemyshot.prefab;
        enemy = Instantiate(enemyPrefab, enemyAnchor.position, Quaternion.identity);
    }
    void Start()
    {
        


    }

    
    void Update()
    {
        
    }
}
