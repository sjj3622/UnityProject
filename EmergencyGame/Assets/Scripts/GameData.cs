using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gameId;
    public int userId;
    public int[] starLevels;
    public int starLevelsSavedCount;
    public string[] Keys;  // ← Keys를 GameData 안에 저장
}
