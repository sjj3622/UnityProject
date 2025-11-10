using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    public GameObject startBtn;
    public GameObject continueBtn;




    public void gameSart()
    {

        // 새 게임 시작함으로 모든 데이터 삭제 및 초기화
        PlayerPrefs.DeleteAll();  //저장데이터 삭제
        PlayerPrefs.SetInt("PlayerHP", 3);
        PlayerPrefs.SetString("LastScene", "WorldMap");
        RoomManager.doorNumber = 0; //문번호 초기화

        SceneManager.LoadScene("WorldMap");
    }



    
    void Start()
    {
        // 이전에 게임 기록ㅇ 있다면 continue버튼 활성화 시키기
        string sceneName = PlayerPrefs.GetString("LastScene");
        if(sceneName == "") continueBtn.GetComponent<Button>().interactable = false;
        else  // 게임 기록 있음 - 활성화
            continueBtn.GetComponent<Button>().interactable = true;
    }


    public void ContinueClick()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");      // 저장된 씬 이름
        RoomManager.doorNumber = PlayerPrefs.GetInt("LastDoor");    // 문번호 
        SceneManager.LoadScene(sceneName);
    }


    
    void Update()
    {
        
    }
}
