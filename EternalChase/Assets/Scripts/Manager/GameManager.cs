using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BattleContext BattleContext = new BattleContext();

    public static int playerMaxHp = 50;
    public static int playerHp = 50;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
