using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BDTimerController : MonoBehaviour
{
    //private BDgpManger gameManager;


    public Text timerText;
    public float timerDuration = 180f;
    public float totalTimer = 0f;
    public float timer;

    public bool timerRunning = false;

    void Start()
    {
        //gameManager = FindAnyObjectByType<BDgpManger>();
        timerText.text = "";
        timer = timerDuration;

    }


    void Update()
    {
        // 게임 시작 상태 체크
        if (BDgpManager.gameState == "BDStart" && !timerRunning)
        {
            timerRunning = true;
            Debug.Log("타이머 시작됨");
        }

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
                Debug.Log("타이머 종료됨");
            }

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
    }
}
