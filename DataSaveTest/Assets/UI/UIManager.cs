using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject arrowText;
    public GameObject keyText;
    public GameObject hpImg;
    public GameObject mainImg;
    public GameObject resetBtn;

    public Sprite life0, life1, life2, life3;
    public Sprite gameOverImg, gmaeClearImg;

    int haskeys = 0;
    int hasArrow = 0;
    int hp = 0;

    public string retrySceneName;






    void Start()
    {
        UpdateItemCount();
        UpdateHp();
        Invoke("HideImage",1.0f);  // 게임스타트 이미지 1초 뒤 숨기기 메서드 실행
        resetBtn.SetActive(false); //retry 버튼 숨기기
    }

    
    void Update()
    {
        UpdateItemCount();
        UpdateHp();
        if(PlayerController.gameState == "gameEnd")
        {
            if (Input.GetButtonDown("Submit")) Retry();
            else if (Input.GetButtonDown("Cancel")) SceneManager.LoadScene("Title");

        }
    }

    void HideImage()
    {
        mainImg.SetActive(false);

    }




    void UpdateItemCount()
    {
        if(hasArrow != ItemKeeper.hasArrows)
        {
            arrowText.GetComponent<Text>().text = ItemKeeper.hasArrows.ToString();
            hasArrow = ItemKeeper.hasArrows;
        }

        if(haskeys != ItemKeeper.hasKeys)
        {
            keyText.GetComponent<Text>().text = ItemKeeper.hasKeys.ToString();
            haskeys = ItemKeeper.hasKeys;
        }
    }


    void UpdateHp()
    {
        if(PlayerController.gameState != "gmaeEnd")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (PlayerController.hp != hp)
            {
                hp = PlayerController.hp;
                if(hp <= 0)  //캐릭터가 사망
                {
                    hpImg.GetComponent<Image>().sprite = life0; //생명력 없는 이미지변경
                    resetBtn.SetActive(true);
                    mainImg.SetActive(true);
                    mainImg.GetComponent<Image>().sprite = gameOverImg;
                    
                    PlayerController.gameState = "gameEnd"; //게임 종료 오버

                }

                else if (hp == 1) hpImg.GetComponent <Image>().sprite = life1;
                
                else if (hp == 2) hpImg.GetComponent<Image>().sprite = life2;
                
                else if (hp >= 3) hpImg.GetComponent<Image>().sprite = life3;
                
            }
        }
    }


    public void Retry()
    {
        PlayerPrefs.SetInt("PlayerHP", 3);
        SceneManager.LoadScene(retrySceneName);
    }


    public void gameClear()
    {
        mainImg.SetActive(true);
        mainImg.GetComponent<Image>().sprite = gmaeClearImg;
        resetBtn.SetActive(false);
        PlayerController.gameState = "gameClear";
        Invoke("GoTitle", 4.0f); //4초뒤에 타이틀로 이동
    }

    void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
