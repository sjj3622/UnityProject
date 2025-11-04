<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    public static GameManager Instance;

    public BattleContext BattleContext = new BattleContext();

    public static int playerMaxHp = 50;
    public static int playerHp = 50;


<<<<<<< HEAD
=======

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
<<<<<<< HEAD
        }
    }

=======
        } 
    }
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    void Start()
    {
        
    }

<<<<<<< HEAD
    
=======
    // Update is called once per frame
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    void Update()
    {
        
    }
}
