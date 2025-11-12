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

    private float timer;
    private bool timerRunning = false;

    void Start()
    {
        //gameManager = FindAnyObjectByType<BDgpManger>();
        timerText.text = "";
        

    }

    
    void Update()
    {
        if (BDgpManger.gameState == "BDStart")
        {
            timerRunning = false;
        }

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
            }

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        if (!timerRunning)
        {

            timer = timerDuration;
            timerRunning = true;
            Debug.Log("타이머 직접 시작됨");
        }

    }
}
