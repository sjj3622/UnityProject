using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{

    public static int doorNumber = 0; // 문 번호





    void Start()
    {
        //태그명이 exit 전부찾기 - 출입구 전부 찾기
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");
        for(int i=0; i < enters.Length; i++)
        {
            GameObject doorObj = enters[i];
            Exit exit = doorObj.GetComponent<Exit>();
            if (doorNumber == exit.doorNum) //캐릭터가 들어갈 문 번호 찾기
            {
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;
                if (exit.direction == ExitDirection.Up) y += 1;
                else if(exit.direction == ExitDirection.Down) y -= 1;
                else if(exit.direction == ExitDirection.Left) x -= 1;
                else if(exit.direction == ExitDirection.Right) x += 1;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y);
                break;
            }
        }

    }

    public static void LoadScene(string sceneName, int doorNum)
    {
        doorNumber = doorNum;

        // 맵 이동하기 전에 저장
        // 최근에 저장된 맵(씬) 있으면 가져와서 저장
        string nowScene = PlayerPrefs.GetString("LastScene");
        Debug.Log(" LoadScene  : " +nowScene);
        if (nowScene != "")
        {
            Debug.Log(" LoadScene   if :  잘되나?");
            SaveDataManager.SaveArrageDate(nowScene);
        }
        PlayerPrefs.SetString("LastScene", sceneName); // 이동하기 전 현재 씬이름 저장
        PlayerPrefs.SetInt("LastDoor", doorNum);       // 현재 이동하려는 문번호
        ItemKeeper.SaveItem();                         // 열쇠와 화살 수량 저장




        SceneManager.LoadScene(sceneName);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
