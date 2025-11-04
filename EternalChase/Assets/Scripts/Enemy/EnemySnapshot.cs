<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySnapshot 
{
    public string name;
    public int maxHP, hp, STR, INT, DEF, AGI;
    public Sprite sprite;
    public GameObject prefab;
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]

public class EnemySnapshot
{
    public string name;
    public int maxHp, hp, STR, INT, DEF, AGI;
    public Sprite sprite;
    public GameObject prefab;



}


>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
[System.Serializable]
public class BattleContext
{
    public EnemySnapshot enemy;
}
