using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int userId;
    public int[] starLevels = new int[4]; // 씬 4개의 별점 저장
    public int starLevelsSavedCount = 0; // 실제 저장된 별점 개수
}