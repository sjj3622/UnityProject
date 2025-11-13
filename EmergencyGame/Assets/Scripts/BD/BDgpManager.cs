using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BDgpManager : MonoBehaviour
{
    public static string gameState;

    public GameObject ClearPanel;
    public GameObject OverPanel;

    private BleedController bleedController;
    private BDScoreController bdscoreController;
    private BDCountController bdscountController;
    private BDTimerController bdtimerController;
    private Mouse Mouse;
    private BDStarScore bdstarScore;
    private BDcamera bdcamera;

    private bool isTimer = false;
    private bool isCount = false;


    void Start()
    {

        // 씬에 있는 BleedController 자동 찾기
        bleedController = FindAnyObjectByType<BleedController>();
        bdscoreController = FindAnyObjectByType<BDScoreController>();
        bdscountController = FindAnyObjectByType<BDCountController>();
        bdtimerController = FindAnyObjectByType<BDTimerController>();
        Mouse = FindAnyObjectByType<Mouse>();
        bdstarScore = FindObjectOfType<BDStarScore>(true);
        bdcamera = FindAnyObjectByType<BDcamera>();

        Debug.Log(BDgpManager.gameState);

        if (ClearPanel != null && OverPanel != null)
        {
            ClearPanel.SetActive(false);
            OverPanel.SetActive(false);
        }

    }


    void Update()
    {
        // 시간과 카운트가 처음 시작이 0으로 시작해서 짚어넣음
        if (bdscountController.bloodCount >= 1) isCount = true;

        if (bdtimerController.timer >= 1) isTimer = true;

        if (bdscoreController.score >= bdscoreController.goalScore)
        {
            // 스코어가 100점 이상시 게임 클리어
            Debug.Log("스코어 100점");
            BDgpManager.gameState = "BDClear";
            
            AllStop();

            SceneManager.LoadScene("Bleeding");


        }

        if (bdscountController.bloodCount <= 0 && isCount)
        {
            Debug.Log("게임 클리어");
            BDgpManager.gameState = "BDClear";
            // 피 오브젝트를 빠르게 없앨시 게임 클리어
            Debug.Log("BDgpManager.gameState :" + BDgpManager.gameState);
            AllStop();
            StartCoroutine(Wait());
            
           
        }


        if (bdscountController.bloodCount > 30)
        {
            // 피의 오브젝트가 30개 이상인 경우 게임 오버
            BDgpManager.gameState = "BDOver";
            Debug.Log("게임오버");

            AllStop();

        }


        if (bdtimerController.timer <= 0 && isTimer)
        {
            Debug.Log("타임오버");
            //타임오버 되면 게임 오버
            BDgpManager.gameState = "BDOver";

            AllStop();

        }


    }
    void AllStop()
    {
        if (bleedController != null)
        {
            bleedController.ClearAllBleeds();
        }

        Mouse.enabled = false;

        //bdcamera.enabled = false;

        bdtimerController.timerRunning = false;

        if (BDgpManager.gameState == "BDOver")
        {
            OverPanel.SetActive(true);
            ClearPanel.SetActive(false);
        }
        if (BDgpManager.gameState == "BDClear")
        {
            ClearPanel.SetActive(true);
            OverPanel.SetActive(false);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SceneManager.LoadScene("Bleeding");
    }
}



