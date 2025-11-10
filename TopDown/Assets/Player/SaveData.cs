using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]   //해당 클래스는 저장 대상이다.

public class SaveData
{

    public int aId = 0;
    public string objectTag = ""; //객체의 태그


}

public class SaveDataList
{
    public SaveData[] saveDatas;

}


