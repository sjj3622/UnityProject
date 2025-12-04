using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keys : MonoBehaviour
{
    public string[] Keys;

    void Start()
    {
        // GameDataManager에서 GameData 가져오기
        GameData data = GameDataManager.Instance.gameData;

        if (data == null)
        {
            Debug.LogError("GameDataManager에 gameData가 없습니다!");
            return;
        }

        // Keys 받아오기
        Keys = data.Keys;

        if (Keys == null)
        {
            Debug.LogError("Keys 데이터가 없습니다!");
            return;
        }

        // Keys 사용
        foreach (var k in Keys)
        {
            Debug.Log("불러온 키 : " + k);
        }
    }
}
