using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gameId;
    public int userId;
    public int[] starLevels;        // 씬 4개의 별점 저장
    public int starLevelsSavedCount; // 실제 저장된 별점 개수
}
