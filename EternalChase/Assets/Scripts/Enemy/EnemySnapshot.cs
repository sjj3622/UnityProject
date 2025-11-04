using System.Collections;
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


[System.Serializable]
public class BattleContext
{
    public EnemySnapshot enemy;
}
